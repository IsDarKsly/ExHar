using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public static class LoadClass
{
   
    /// <summary>
    /// Attempts to load an object of type T from a given path
    /// </summary>
    /// <typeparam name="T">Must be serializable class</typeparam>
    /// <param name="path">Path to data</param>
    /// <returns>Deserialized object of type T</returns>
    public static T Load<T>(string path) where T : class
    {
        try
        {
            var serializer = new XmlSerializer(typeof(T)); //Creates the xml serializer
            var fStream = new FileStream(path, FileMode.Open); //creates the FileStream
            var xml = (T)serializer.Deserialize(fStream);
            fStream.Close();
            return xml; //Returns a deserialized object
        }
        catch (Exception e)
        { 
            Debug.LogException(e);
            return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Must be either string or int to work</typeparam>
    /// <param name="loadValue">The value to be loaded</param>
    /// <param name="path"></param>
    public static void LoadValue<T>(out T loadValue, string path)
    {
        loadValue = default(T);
        try
        {
            var reader = new StreamReader(path); //Creates stream to path

            if (typeof(T) == typeof(string)) loadValue = (T)(object)reader.ReadLine(); //uses the string based function
            else loadValue = (T)(object)int.Parse(reader.ReadLine()); //uses the string based function

            reader.Close();
        }
        catch (Exception e) 
        {
            Debug.LogException(e);
        }
    }
}
