using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public static class SaveClass //This class will take on the duty of saving any data to paths
{
    
    public static void Save<T>(T saveObj, string path) where T : class //Save function with Generic class reference
    {
        var serializer = new XmlSerializer(typeof(T)); //Creates serializing object

        var writer = new StreamWriter(path); //creates Streamwriting object, which actually writes to a text file
        serializer.Serialize(writer, saveObj); //Serializes the object into the stream
        writer.Close(); //Closes the stream
    }

    public static void SaveValue(object saveObj, string path) //Save function that only takes a value instead of an entire class
    {
        var writer = new StreamWriter(path); //Creates the streamwriting object
        writer.Write(saveObj); //Writes the calue into the stream
        writer.Close(); //Closes the stream
    }

}
