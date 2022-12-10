using MSCLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using UnityEngine;

namespace KiljuBox
{
    internal class SaveUtility
    {
        public static void Save<T>(T saveData)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                StreamWriter output = new StreamWriter(SaveUtility.path);
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "    ",
                    NewLineOnAttributes = false,
                    OmitXmlDeclaration = true
                };
                XmlWriter xmlWriter = XmlWriter.Create(output, settings);
                xmlSerializer.Serialize(xmlWriter, saveData);
                xmlWriter.Close();
            }
            catch (Exception ex)
            {
                ModConsole.Error(SaveUtility.modName + ": " + ex.ToString());
            }
        }

        public static T Load<T>()
        {
            T result;
            try
            {
                bool flag = File.Exists(SaveUtility.path);
                if (flag)
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    XmlReader xmlReader = XmlReader.Create(SaveUtility.path);
                    result = (T)xmlSerializer.Deserialize(xmlReader);
                }
                else
                {
                    result = (T)Activator.CreateInstance(typeof(T));
                }
            }
            catch (Exception ex)
            {
                ModConsole.Error(SaveUtility.modName + ": " + ex.ToString());
                result = (T)Activator.CreateInstance(typeof(T));
            }
            return result;
        }

        public static void Remove()
        {
            bool flag = File.Exists(SaveUtility.path);
            if (flag)
            {
                File.Delete(SaveUtility.path);
                ModConsole.Print(SaveUtility.modName + ": Savefile found and deleted, mod is reset.");
            }
            else
            {
                ModConsole.Print(SaveUtility.modName + ": Savefile not found, mod is already reset.");
            }
        }

        private static string modName = typeof(SaveUtility).Namespace;
        private static string path = Path.Combine(Application.persistentDataPath, SaveUtility.modName + ".xml");
    }
}
