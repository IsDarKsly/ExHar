using UnityEngine;
using Newtonsoft.Json;

public static class Stringifier
{
    /// <summary>
    /// Stringify very simply takes a class and turns it into a string
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string Stringify<T>(T obj) 
    {
        // Convert the object to JSON
        return JsonConvert.SerializeObject(obj, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented });
    }

    /// <summary>
    /// Destringify takes a string an turns it into a specified object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T Destringify<T>(string obj) 
    {
        return JsonConvert.DeserializeObject<T>(obj, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }); // Converts JSON string back to object
    }
}
