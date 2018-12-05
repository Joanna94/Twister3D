using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

public static class Deserialize {

    public static Scene ParseFromXML(string filename)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Scene));
        Scene scene = new Scene();

        using(Stream reader = new FileStream(filename, FileMode.Open))
        {
            scene = (Scene)serializer.Deserialize(reader);
        }

        return scene;
    }
}
