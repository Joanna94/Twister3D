using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LZWPlib
{
    [DisallowMultipleComponent]
    public class LzwpMocap : MonoBehaviour
    {
        [Header("Data source")]
        public ushort port = 5000;
        static DTrackDataReceiver rcvr;


        [HideInInspector]
        public static Frame lastFrame = new Frame();


        // aAdjustments
        public static Quaternion rotationFix = Quaternion.Euler(90f, 180f, 0);
        public static float posScale = 0.001f;
        public static Vector3 cavePivotToTrackingZero = new Vector3(0, 1.035f, 0);


        void Start()
        {
            rcvr = new DTrackDataReceiver(port);
            rcvr.StartReceiving();
        }

        public static void UpdateData(Transform transform)
        {
            lock (rcvr.frameLock)
            {
                if (rcvr.lastFrame.successfullyParsed)
                {
                    var fr = rcvr.lastFrame;
                    // copy frame data

                    lastFrame.framecounter = fr.framecounter;
                    lastFrame.timestamp = fr.timestamp;
                    lastFrame.act_num_human = fr.act_num_human;
                    lastFrame.humans = new List<Human>();

                    foreach (var h in fr.humans)
                    {
                        Human nh = new Human() { id = h.id, numBones = h.numBones };
                        
                        for (int i = 0; i < h.bones.Length; i++)
                        {
                            nh.bones[i].quality = h.bones[i].quality;
                            nh.bones[i].rotationMatrix = h.bones[i].rotationMatrix;
                            nh.bones[i].position = Vector3.Scale(h.bones[i].position, new Vector3(1f, 1f, -1f)) * posScale 
                            + cavePivotToTrackingZero 
                            + transform.position;
                            nh.bones[i].rotation = new Quaternion(-h.bones[i].rotation.x, -h.bones[i].rotation.y, h.bones[i].rotation.z, h.bones[i].rotation.w) * rotationFix;
                            nh.bones[i].angles = h.bones[i].angles;
                        }

                        lastFrame.humans.Add(nh);
                    }
                }
            }
        }

        public static void FixFileForDTrackRecorder(string inputFile, string outputFile = null)
        {
            Debug.Log("Fixing record file started...");

            if (string.IsNullOrEmpty(outputFile))
                outputFile = Path.Combine(Path.GetDirectoryName(inputFile), Path.GetFileNameWithoutExtension(inputFile)) + "_fixed" + Path.GetExtension(inputFile);

            if (File.Exists(outputFile))
            {
                Debug.LogWarning("Output file already exists: " + outputFile);
                return;
            }

            if (!File.Exists(inputFile))
            {
                Debug.LogWarning("Cannot find input file: " + inputFile);
                return;
            }

            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                using (StreamReader sr = new StreamReader(inputFile))
                {
                    string line;
                    List<string> frameLines = new List<string>();
                    int frameCounter = -1;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith("fr "))
                        {
                            frameCounter++;

                            if (frameCounter > 1)
                            {
                                sw.WriteLine("fr " + (frameCounter - 1));
                                foreach (string s in frameLines)
                                    sw.WriteLine(s);
                                frameLines.Clear();
                            }
                        }
                        else if (frameCounter > 0)
                            frameLines.Add(line);
                    }
                }
            }

            Debug.Log("Fixed record file: " + outputFile);
        }
    }
}
