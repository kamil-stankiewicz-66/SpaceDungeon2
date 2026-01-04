using System.Collections;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] GameObject cutscenePanelPrefab;
    InputManager inputManager;


    //controller
    bool isRunning;


    private void Awake()
    {
        if (cutscenePanelPrefab == null) { Debug.LogWarning("CHAR_DIAL_CONTROLLER :: dial panel is null"); }


        inputManager = FindAnyObjectByType<InputManager>();

        if (inputManager == null) { Debug.LogWarning("CHAR_DIAL_CONTROLLER :: inputManager is null"); }
    }


    public void Call(SOCutsceneSeq dialogSeq)
    {
        if (!isRunning)
        {
            StartCoroutine(CallProcess(dialogSeq));
        }
    }


    IEnumerator CallProcess(SOCutsceneSeq dialogSeq)
    {
        isRunning = true;
        GameMarks.SetAll(false);


        var cutscenePanelInstance = Instantiate(cutscenePanelPrefab);
        CutscenePanel cutscenePanel = cutscenePanelInstance?.GetComponent<CutscenePanel>();

        if (cutscenePanel == null) { Debug.LogWarning("CHAR_DIAL_CONTROLLER :: charDialStatement is null"); }


        //show dial
        bool skip = false;
        int index = 0;
        while (true)
        {
            if (index == dialogSeq.Get().Length)
            {
                break;
            }


            var entry = dialogSeq.Get()[index];
            cutscenePanel.Set(entry.message, entry.profile.profileName, entry.profile.profilePic);


            //wait for ENTER
            if (inputManager.SkipTrigger && !skip)
            {
                index++;
                skip = true;
            }
            else if (!inputManager.SkipTrigger)
            {
                skip = false;
            }


            yield return null;
        }


        //dispose
        Destroy(cutscenePanelInstance);
        GameMarks.SetAll(true);
        isRunning = false;
    }
}
