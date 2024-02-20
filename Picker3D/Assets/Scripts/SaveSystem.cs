using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System;
using UnityEditor;

public static class SaveSystem<T>
{
    static string extensionName = "picker";

    static bool isSaveSystemOn = false;

    public static void Save(string name, T value)
    {
        if (!isSaveSystemOn)
            return;

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + name + "." + extensionName;
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, value);
        stream.Close(); 
    }

    public static T Load(string name, T nullVal)
    {
        if (!isSaveSystemOn)
            return nullVal;

        T result = nullVal;
        string path = Application.persistentDataPath + "/" + name + "." + extensionName;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            result = (T)formatter.Deserialize(stream);
            stream.Close();
        }
        return result;
    }


}


[Serializable]
public class Vector3Saver
{
    float x, y, z;

    public Vector3Saver(Vector3 vector3)
    {
        x = vector3.x;
        y = vector3.y;
        z = vector3.z;
    }

    public Vector3 GetVector3()
    {
        return new Vector3(x, y, z);
    }
}

