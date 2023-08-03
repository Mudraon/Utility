using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

public class GenericSerializer<T>
{
    public GenericSerializer() { }

    public void XmlSerialize(string aPath, T aData)
    {
        new GenericXmlSerializer<T>().Serialize(aPath, aData);
    }

    public T XmlDeserialize(string aPath)
    {
        return new GenericXmlSerializer<T>().Deserialize(aPath);
    }

    public void BinarySerialize(string aPath, T aData)
    {
        new GenericBinarySerializer<T>().Serialize(aPath, aData);
    }

    public T BinaryDeserialize(string aPath)
    {
        return new GenericBinarySerializer<T>().Deserialize(aPath);
    }
}

public class GenericXmlSerializer<T> : XmlSerializer
{
    public GenericXmlSerializer() : base(typeof(T)) { }

    public void Serialize(string aPath, T aData)
    {
        using (FileStream lStream = new FileStream(aPath, FileMode.Create))
        {
            base.Serialize(lStream, aData);
        }
    }

    public T Deserialize(string aPath)
    {
        if (!File.Exists(aPath))
            return default(T);

        using (FileStream lStream = new FileStream(aPath, FileMode.Open))
        {
            return (T)base.Deserialize(lStream);
        }
    }
}

public class GenericBinarySerializer<T>
{
    public void Serialize(string aPath, T aData)
    {
        BinaryFormatter lFormatter = new BinaryFormatter();
        using (FileStream lStream = new FileStream(aPath, FileMode.Create))
        {
            lFormatter.Serialize(lStream, aData);
        }
    }

    public T Deserialize(string aPath)
    {
        if (!File.Exists(aPath))
            return default(T);

        BinaryFormatter lFormatter = new BinaryFormatter();
        using (FileStream lStream = new FileStream(aPath, FileMode.Open))
        {
            return (T)lFormatter.Deserialize(lStream);
        }
    }
}
