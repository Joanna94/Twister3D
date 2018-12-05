using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

public static class Serialize {

    public static void ParseToXML(Scene scene)
    {
        DateTime date = DateTime.Now;
        string dateString = date.Year.ToString() + "_" + date.Month.ToString() + "_" + date.Day.ToString() + "_" + date.Hour.ToString() + "_" + date.Minute.ToString() + "_" + date.Second.ToString();
        string filename = "twister_" + dateString + ".xml";

        XmlSerializer serializer = new XmlSerializer(typeof(Scene));
        Stream fs = new FileStream(filename, FileMode.Create);
        XmlWriter writer = new XmlTextWriter(fs, Encoding.Unicode);
        serializer.Serialize(writer, scene);
        writer.Close();
    }
    

}
