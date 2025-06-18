using unitySceneManager = UnityEngine.SceneManagement.SceneManager;
using System.Collections.Generic;

public static class SceneManager
{
    public static Scene currentScene = Scene.MainMenu;

    private static Dictionary<Scene, ushort> sceneMappings = new Dictionary<Scene, ushort>
    {
        { Scene.MainMenu, 0},
        { Scene.Game, 1}
    };


    public static void ChangeScene(Scene scene)
    {
        UnityEngine.Debug.Log("SceneManager: " + currentScene + " => " + scene);

        if (scene == currentScene)
        {
            return;
        }

        if (sceneMappings.TryGetValue(scene, out ushort _sceneNum))
        {
            unitySceneManager.LoadScene(_sceneNum);
            currentScene = scene;
        }
    }

}

public enum Scene
{
    MainMenu,
    Game
}