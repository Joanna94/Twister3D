using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;

namespace LZWPlib
{
    public class DTrackDataReceiver
    {
        public ushort port;
        public int receiveTimeout = 1000;  // [ms]

        public Frame lastFrame = null;

        protected UdpClient socket;

        public object frameLock = new object();


        private static char[] separatorCharSpace = new char[] { ' ' };

        public DTrackDataReceiver(ushort port)
        {
            this.port = port;

            lastFrame = new Frame();
        }

        public void StartReceiving()
        {
            socket = new UdpClient(port);
            socket.Client.ReceiveTimeout = receiveTimeout;
            socket.BeginReceive(new AsyncCallback(OnUdpData), socket);
        }

        void OnUdpData(IAsyncResult result)
        {
            UdpClient socket = result.AsyncState as UdpClient;
            IPEndPoint source = new IPEndPoint(0, 0);
            byte[] message = socket.EndReceive(result, ref source);

            ProcessFrameData(Encoding.ASCII.GetString(message));
            
            socket.BeginReceive(new AsyncCallback(OnUdpData), socket);
        }

        void ProcessFrameData(string frameData)
        {
            string[] lines = frameData.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            
            lock (frameLock)
            {
                lastFrame.successfullyParsed = true;

                foreach (string line in lines)
                {
                    if (!ParseLine(line))
                    {
                        Debug.LogError("### Parsing error ###\n" + frameData);

                        lastFrame.successfullyParsed = false;
                        break;
                    }
                }
            }
        }

        bool ParseLine(string line)
        {
            if (string.IsNullOrEmpty(line))
                return false;
            
            string[] lineSegments = line.Split(separatorCharSpace, 4);

            if (lineSegments.Length < 2)
                return false;

            if (lineSegments[0] == "fr")  // frame counter
            {
                if (!uint.TryParse(lineSegments[1], out lastFrame.framecounter))
                {
                    lastFrame.framecounter = 0;
                    return false;
                }

                return true;
            }
            else if (lineSegments[0] == "ts")  // timestamp
            {
                if (!double.TryParse(lineSegments[1], NumberStyles.AllowExponent | NumberStyles.Float, CultureInfo.InvariantCulture, out lastFrame.timestamp))
                {
                    lastFrame.timestamp = -1;
                    return false;
                }

                return true;
            }
            else if (lineSegments[0] == "6dj")  // 6dj human model data
            {
                int n, id, idj;

                if (!int.TryParse(lineSegments[1], out n))
                {
                    return false;
                }

                while (n > lastFrame.humans.Count)
                    lastFrame.humans.Add(new Human() { id = lastFrame.humans.Count, numBones = 0 });

                lastFrame.act_num_human = n;

                if (!int.TryParse(lineSegments[2], out n))
                {
                    return false;
                }

                int posBegin = 0;
                int posEnd = 0;

                for (int i = 0; i < n; i++)
                {
                    posBegin = line.IndexOf('[', posEnd);
                    posEnd = line.IndexOf(']', posBegin);

                    if (posEnd == -1)
                    {
                        return false;
                    }

                    string[] seg = line.Substring(posBegin + 1, posEnd - posBegin - 1).Split(separatorCharSpace, 2);

                    if (!int.TryParse(seg[0], out id))
                    {
                        return false;
                    }

                    if (!int.TryParse(seg[1], out lastFrame.humans[id].numBones))
                    {
                        return false;
                    }

                    for (int j = 0; j < lastFrame.humans[id].numBones; j++)
                    {
                        posBegin = line.IndexOf('[', posEnd);
                        posEnd = line.IndexOf(']', posBegin);

                        if (posEnd == -1)
                        {
                            return false;
                        }

                        seg = line.Substring(posBegin + 1, posEnd - posBegin - 1).Split(separatorCharSpace, 2);

                        if (!int.TryParse(seg[0], out idj))
                        {
                            return false;
                        }

                        if (!float.TryParse(seg[1], NumberStyles.AllowExponent | NumberStyles.Float, CultureInfo.InvariantCulture, out lastFrame.humans[id].bones[idj].quality))
                        {
                            return false;
                        }

                        posBegin = line.IndexOf('[', posEnd);
                        posEnd = line.IndexOf(']', posBegin);

                        if (posEnd == -1)
                        {
                            return false;
                        }

                        seg = line.Substring(posBegin + 1, posEnd - posBegin - 1).Split(separatorCharSpace, 6);

                        if (!float.TryParse(seg[0], NumberStyles.AllowExponent | NumberStyles.Float, CultureInfo.InvariantCulture, out lastFrame.humans[id].bones[idj].position.x))
                        {
                            return false;
                        }

                        if (!float.TryParse(seg[1], NumberStyles.AllowExponent | NumberStyles.Float, CultureInfo.InvariantCulture, out lastFrame.humans[id].bones[idj].position.y))
                        {
                            return false;
                        }

                        if (!float.TryParse(seg[2], NumberStyles.AllowExponent | NumberStyles.Float, CultureInfo.InvariantCulture, out lastFrame.humans[id].bones[idj].position.z))
                        {
                            return false;
                        }

                        if (!float.TryParse(seg[3], NumberStyles.AllowExponent | NumberStyles.Float, CultureInfo.InvariantCulture, out lastFrame.humans[id].bones[idj].angles[0]))
                        {
                            return false;
                        }

                        if (!float.TryParse(seg[4], NumberStyles.AllowExponent | NumberStyles.Float, CultureInfo.InvariantCulture, out lastFrame.humans[id].bones[idj].angles[1]))
                        {
                            return false;
                        }

                        if (!float.TryParse(seg[5], NumberStyles.AllowExponent | NumberStyles.Float, CultureInfo.InvariantCulture, out lastFrame.humans[id].bones[idj].angles[2]))
                        {
                            return false;
                        }

                        posBegin = line.IndexOf('[', posEnd);
                        posEnd = line.IndexOf(']', posBegin);

                        if (posEnd == -1)
                        {
                            return false;
                        }

                        seg = line.Substring(posBegin + 1, posEnd - posBegin - 1).Split(separatorCharSpace, 9);

                        for (int jj = 0; jj < 9; jj++)
                        {
                            if (!float.TryParse(seg[jj], NumberStyles.AllowExponent | NumberStyles.Float, CultureInfo.InvariantCulture, out lastFrame.humans[id].bones[idj].rotationMatrix[jj]))
                            {
                                return false;
                            }
                        }

                        RotateMatrixToQuaternion(lastFrame.humans[id].bones[idj].rotationMatrix, out lastFrame.humans[id].bones[idj].rotation);
                    }
                }

                return true;
            }

            return true;
        }

        private static void RotateMatrixToQuaternion(float[] m, out Quaternion q)
        {
            float tr = m[0] + m[4] + m[8];

            if (tr > 0)
            {
                float S = Mathf.Sqrt(tr + 1.0f) * 2.0f;
                q.w = 0.25f * S;
                q.x = (m[5] - m[7]) / S;
                q.y = (m[6] - m[2]) / S;
                q.z = (m[1] - m[3]) / S;
            }
            else if ((m[0] > m[4]) && (m[0] > m[8]))
            {
                float S = Mathf.Sqrt(1.0f + m[0] - m[4] - m[8]) * 2.0f;
                q.w = (m[5] - m[7]) / S;
                q.x = 0.25f * S;
                q.y = (m[3] + m[1]) / S;
                q.z = (m[6] + m[2]) / S;
            }
            else if (m[4] > m[8])
            {
                float S = Mathf.Sqrt(1.0f + m[4] - m[0] - m[8]) * 2.0f;
                q.w = (m[6] - m[2]) / S;
                q.x = (m[3] + m[1]) / S;
                q.y = 0.25f * S;
                q.z = (m[7] + m[5]) / S;
            }
            else
            {
                float S = Mathf.Sqrt(1.0f + m[8] - m[0] - m[4]) * 2.0f;
                q.w = (m[1] - m[3]) / S;
                q.x = (m[6] + m[2]) / S;
                q.y = (m[7] + m[5]) / S;
                q.z = 0.25f * S;
            }
        }
    }





    public class Frame
    {
        public bool successfullyParsed = false;

        public uint framecounter = 0;
        public double timestamp = -1;
        
        public int act_num_human = 0;
        public List<Human> humans = new List<Human>();

        public Frame()
        { }

        public override string ToString()
        {
            return string.Format("fr={0} ts={1}", framecounter, timestamp);
        }
    }

    public class Human
    {
        public int id = -1;

        public static uint DTRACKSDK_HUMAN_MAX_JOINTS = 200;

        public int numBones = 0;
        public HumanBone[] bones = new HumanBone[DTRACKSDK_HUMAN_MAX_JOINTS];

        public Human()
        {
            for (int i = 0; i < bones.Length; i++)
                this.bones[i] = new HumanBone(i);
        }
    }

    public class HumanBone
    {
        public int id = -1;
        public float quality = -1;

        public bool tracked
        {
            get
            {
                return quality > 0;
            }
        }

        public float[] rotationMatrix = new float[9];

        public Vector3 position = Vector3.zero;
        public Quaternion rotation = Quaternion.identity;
        public float[] angles = new float[3];

        public HumanBone() { }

        public HumanBone(int id)
        {
            this.id = id;
        }
    }
}
