using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CutscenePanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI profileNameTMP, messageTMP;
    [SerializeField] Image profilePicHolder;

    private void OnEnable()
    {
        if (profileNameTMP == null || messageTMP == null || profilePicHolder == null)
        {
            Debug.LogWarning("CHARACTER_DIALOGUE_STATEMENT :: something is null, idk");
        }
    }

    public void Set(string message, string profileName = null, Sprite profilePic = null)
    {
        //message

        messageTMP.text = message;


        //name

        if (profileName == null)
        {
            profileName = "???";
        }
        else
        {
            profileNameTMP.text = profileName;
        }


        //sprite

        if (profilePic == null)
        {
            profilePicHolder.enabled = false;
        }
        else
        {
            profilePicHolder.enabled = true;
            profilePicHolder.sprite = profilePic;

            Vector3 ratio = new Vector3(-1.0f * profilePic.textureRect.width / profilePic.textureRect.height, 1.0f);
            profilePicHolder.transform.localScale = ratio;
        }
    }
}
