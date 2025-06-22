using System.Linq;
using UnityEngine;
using io = System.IO;

public static class PATH
{
    //main
    static string gameFolder = Application.persistentDataPath;
    const string ext = "save";


    public const string PLAYERDATA_FILE = "playerdata";
    public const string WEAPONSBASE_FILE = "weaponsdata";
    public const string LEVELRUNMODE_FILE = "levelrunmode";

    public static string LEVELS_FOLDER = "levels";
    public static string LEVELS_PLAYERDATA_FILE = "player";
    public static string LEVELS_STORYACTIVITY_FOLDER = "story";
    public static string LEVELS_ENEMIES_FOLDER = "enemies";
    public static string LEVELS_CHESTS_FOLDER = "chests";
    public static string LEVELS_META_FILE = "meta";


    //func

    public static string GetDirectory(string file)
    {
        return CreatePath(gameFolder, file);
    }

    public static string GetDirectory(string[] path)
    {
        string directory = gameFolder;
        for (int i = 0; i < path.Length-1; i++)
        {
            directory = io.Path.Combine(directory, path[i]);
        }

        return CreatePath(directory, path.Last());
    }


    //helpers

    static string CreatePath(string path, string fileName)
    {
        if (!io.Directory.Exists(path))
            io.Directory.CreateDirectory(path);

        return io.Path.Combine(path, $"{fileName}.{ext}");
    }
}