using System;
using UnityEditor;
using UnityEngine;

public class SOResetEditor : EditorWindow
{
    SOPlayerData playerData;
    SOChaptersBase chapters;
    SOWeaponsBase weaponsBase;
    bool autoReset;

    [MenuItem("CustomEditors/ScriptObj Reset")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<SOResetEditor>("SOReset");
    }

    private Vector2 scrollPosition;
    private void OnGUI()
    {
        //scrollbar start
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        //so
        playerData = (SOPlayerData)EditorGUILayout.ObjectField("PlayerData", playerData, typeof(SOPlayerData), false);
        chapters = (SOChaptersBase)EditorGUILayout.ObjectField("Chapters", chapters, typeof(SOChaptersBase), false);
        weaponsBase = (SOWeaponsBase)EditorGUILayout.ObjectField("WeaponsBase", weaponsBase, typeof(SOWeaponsBase), false);
        autoReset = EditorGUILayout.Toggle("AutoReset", autoReset);

        //scroolbar end
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Reset"))
        {
            Reset();
        }
    }

    private void Reset()
    {
        try
        {
            playerData.SetDefault();
            weaponsBase.SetDefault();
            chapters.SetDefault();

            Debug.Log("EDITOR: SO_ResetEditor: Reset completed");
        }
        catch (Exception e)
        {
            Debug.LogWarning($"SO_ResetEditor: Null exception: {e.Message}");
        }

    }


    //auto reset

    private void OnEnable() => EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

    private void OnDisable() => EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (!autoReset)
        {
            return;
        }

        if (state == PlayModeStateChange.EnteredEditMode)
        {
            Reset();
        }
    }
}
