using System.Collections.Generic;
using System.Xml.Serialization;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
{
    public System.Xml.Schema.XmlSchema GetSchema() => null;

    public void ReadXml(System.Xml.XmlReader reader)
    {
        var keySerializer = new XmlSerializer(typeof(TKey));
        var valueSerializer = new XmlSerializer(typeof(TValue));

        reader.ReadStartElement();
        while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
        {
            reader.ReadStartElement("item");

            reader.ReadStartElement("key");
            var key = (TKey)keySerializer.Deserialize(reader);
            reader.ReadEndElement();

            reader.ReadStartElement("value");
            var value = (TValue)valueSerializer.Deserialize(reader);
            reader.ReadEndElement();

            Add(key, value);
            reader.ReadEndElement();
        }
        reader.ReadEndElement();
    }

    public void WriteXml(System.Xml.XmlWriter writer)
    {
        var keySerializer = new XmlSerializer(typeof(TKey));
        var valueSerializer = new XmlSerializer(typeof(TValue));

        foreach (var kvp in this)
        {
            writer.WriteStartElement("item");

            writer.WriteStartElement("key");
            keySerializer.Serialize(writer, kvp.Key);
            writer.WriteEndElement();

            writer.WriteStartElement("value");
            valueSerializer.Serialize(writer, kvp.Value);
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}
