using UnityEditor;
using UnityEngine;

public class CharacterDialogueTester : EditorWindow
{
    [SerializeField] CutsceneController cdc;
    [SerializeField] SOCutsceneSeq dialSeq;

    [MenuItem("CustomEditors/CharacterDialogueTester")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<CharacterDialogueTester>("DialTester");
    }

    private void OnGUI()
    {
        cdc = EditorGUILayout.ObjectField("Controller:", cdc, typeof(CutsceneController), true) as CutsceneController;
        dialSeq = EditorGUILayout.ObjectField("DialogSeq:", dialSeq, typeof(SOCutsceneSeq), true) as SOCutsceneSeq;

        //button style
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fixedHeight = 30;

        //button
        if (GUILayout.Button("Test", buttonStyle))
        {
            if (cdc == null || dialSeq == null)
            {
                Debug.Log("EDITOR :: TEST :: CHAR_DIAL_TESTER :: test error");
            }
            else
            {
                cdc.Call(dialSeq);
                Debug.Log("EDITOR :: TEST :: CHAR_DIAL_TESTER :: test started");
            }

        }
    }
}
