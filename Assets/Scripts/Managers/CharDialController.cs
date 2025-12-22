using System.Collections;
using UnityEngine;

public class CharDialController : MonoBehaviour
{
    [SerializeField] GameObject charDialPanelPrefab;
    InputManager inputManager;


    //controller
    bool isRunning;


    private void Awake()
    {
        if (charDialPanelPrefab == null) { Debug.LogWarning("CHAR_DIAL_CONTROLLER :: dial panel is null"); }


        inputManager = FindAnyObjectByType<InputManager>();

        if (inputManager == null) { Debug.LogWarning("CHAR_DIAL_CONTROLLER :: inputManager is null"); }
    }


    public void Call(SOCharDialSeq dialogSeq)
    {
        if (!isRunning)
        {
            StartCoroutine(CallProcess(dialogSeq));
        }
    }


    IEnumerator CallProcess(SOCharDialSeq dialogSeq)
    {
        isRunning = true;
        GameMarks.SetAll(false);


        //create window
        GameObject charDialPanel = Instantiate(charDialPanelPrefab);


        //get script
        CharDialPanel charDialStatement = charDialPanel.GetComponent<CharDialPanel>();

        if (charDialStatement == null) { Debug.LogWarning("CHAR_DIAL_CONTROLLER :: charDialStatement is null"); }


        //show dial
        int index = 0;
        while (true)
        {
            if (index == dialogSeq.Get().Length)
            {
                break;
            }


            var entry = dialogSeq.Get()[index];
            charDialStatement.Set(entry.message, entry.name, entry.profilePic);


            //wait for ENTER
            if (inputManager.SkipTrigger) 
            {
                index++;
            }

           
            yield return null;
        }


        //dispose
        Destroy(charDialPanel);
        GameMarks.SetAll(true);
        isRunning = false;
    }
}
