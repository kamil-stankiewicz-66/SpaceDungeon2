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
            Debug.Log("LoadJson: Plik nie istnieje: " + filePath);
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
                Debug.Log("LoadJson: Nie można załadować obiektu do żądanego typu.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("LoadJson: Błąd podczas deserializacji: " + e.Message);
        }
        

        return false;

    }

    public static bool LoadBin<T>(string filePath, out T @struct) where T : struct
    {
        @struct = default(T);

        if (!File.Exists(filePath))
        {
            Debug.Log("LoadBin: Plik nie istnieje: " + filePath);
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
                    Debug.Log("LoadBin: Nie można załadować obiektu do żądanego typu.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("LoadBin: Błąd podczas deserializacji: " + e.Message);
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