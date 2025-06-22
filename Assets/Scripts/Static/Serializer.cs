using System;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class Serializer
{
    //loader

    public static bool LoadJson<T>(string filePath, out T @struct) where T : struct
    {
        @struct = default(T);

        if (!File.Exists(filePath))
        {
            Debug.Log("LoadJson: File does not exist: " + filePath);
            return false;
        }

        try
        {
            object loadedObject = JsonUtility.FromJson<T>(filePath);
            if (loadedObject is T)
            {
                @struct = (T)loadedObject;
                return true;
            }
            else
            {
                Debug.Log("LoadJson: Could not load object into requested type.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("LoadJson: Error while deserializing: " + e.Message);
        }
        

        return false;

    }

    public static bool LoadBin<T>(string filePath, out T @struct) where T : struct
    {
        @struct = default(T);

        if (!File.Exists(filePath))
        {
            Debug.Log("LoadBin: File does not exist: " + filePath);
            return false;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fileStream = File.Open(filePath, FileMode.Open))
        {
            try
            {
                object loadedObject = formatter.Deserialize(fileStream);
                if (loadedObject is T)
                {
                    @struct = (T)loadedObject;
                    return true;
                }
                else
                {
                    Debug.Log("LoadBin: Could not load object into requested type.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("LoadBin: Error while deserializing: " + e.Message);
            }
        }

        return false;
    }



    //saver

    public static void SaveJson(this object obj, string path)
    {
        string json = JsonUtility.ToJson(obj, true);
        File.WriteAllText(path, json);
    }

    public static void SaveBin(this object obj, string path)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
        bf.Serialize(stream, obj);
        stream.Close();
    }
}