using System.IO;
using UnityEngine;

public static class PATH
{
    //main
    public static string gameFolder = Application.persistentDataPath;
    private const string ext = "save";


    /// <summary>
    /// only get
    /// </summary>

    public static string PLAYERDATA_FILE
    {
        get => CreatePath(gameFolder, "player");
    }

    public static string WEAPONSBASE_FILE
    {
        get => CreatePath(gameFolder, "weaponsdata");
    }

    public static string LEVELRUNMODE_FILE
    {
        get => CreatePath(LEVELS_FOLDER, "levelrunmode");
    }
    


    /// <summary>
    /// functions
    /// </summary>

    private static string LEVELS_FOLDER
    {
        get => Path.Combine(gameFolder, "levels");
    }

    private static string CreatePath(string path, string fileName)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        return Path.Combine(path, $"{fileName}.{ext}");
    }
}