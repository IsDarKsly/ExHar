using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class LoadClass
{

    /// <summary>
    /// Attempts to load an object of type T from a given path.
    /// </summary>
    /// <typeparam name="T">Must be a serializable class.</typeparam>
    /// <param name="path">Path to the data file.</param>
    /// <returns>Deserialized object of type T or null if an error occurs.</returns>
    public static T Load<T>(string path) where T : class
    {
        try
        {
            if (!File.Exists(path))
            {
                Debug.LogError($"File not found at path: {path}");
                return null;
            }

            string json = File.ReadAllText(path); // Reads the JSON file
            return JsonConvert.DeserializeObject<T>(json); // Converts JSON string back to object
        }
        catch (Exception e)
        {
            Debug.LogError(path);
            Debug.LogException(e);
            return null;
        }
    }

    /// <summary>
    /// Attempts to load a simple value (string or int) from a given path.
    /// </summary>
    /// <typeparam name="T">Must be either string or int to work.</typeparam>
    /// <param name="loadValue">The output value to be loaded.</param>
    /// <param name="path">Path to the data file.</param>
    public static void LoadValue<T>(out T loadValue, string path)
    {
        loadValue = default(T); // Initialize with default value

        try
        {
            if (!File.Exists(path))
            {
                Debug.LogError($"File not found at path: {path}");
                return;
            }

            string json = File.ReadAllText(path); // Reads the JSON file

            // Convert the value from JSON based on its type
            if (typeof(T) == typeof(string))
            {
                loadValue = JsonConvert.DeserializeObject<T>(json); // Deserialize string
            }
            else if (typeof(T) == typeof(int))
            {
                loadValue = (T)(object)int.Parse(json); // Parse integer
            }
            else
            {
                Debug.LogError($"Unsupported type: {typeof(T)}. Only string and int are supported.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError(path);
            Debug.LogException(e);
        }
    }

}
