using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using Newtonsoft.Json;
 
public static class SaveClass //This class will take on the duty of saving any data to paths
{

    /// <summary>
    /// Saves a serializable object to the specified path using JSON serialization.
    /// </summary>
    /// <typeparam name="T">The type of the object being saved.</typeparam>
    /// <param name="saveObj">The object to save.</param>
    /// <param name="path">The file path to save the object to.</param>
    public static void Save<T>(T saveObj, string path) where T : class
    {
        // Convert the object to JSON
        string json = JsonConvert.SerializeObject(saveObj, Formatting.Indented);

        // Write the JSON string to the file
        using (var writer = new StreamWriter(path))
        {
            writer.Write(json);
        }
    }

    /// <summary>
    /// Saves a simple value to the specified path.
    /// </summary>
    /// <param name="saveObj">The value to save.</param>
    /// <param name="path">The file path to save the value to.</param>
    public static void SaveValue(object saveObj, string path)
    {
        // Convert the value to JSON
        string json = JsonConvert.SerializeObject(saveObj, Formatting.Indented);

        // Write the JSON string to the file
        using (var writer = new StreamWriter(path))
        {
            writer.Write(json);
        }
    }

}
