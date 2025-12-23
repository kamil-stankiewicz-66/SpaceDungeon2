using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharDialPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI characterNameTMP, messageTMP;
    [SerializeField] Image profilePicHolder;

    private void OnEnable()
    {
        if (characterNameTMP == null || messageTMP == null || profilePicHolder == null)
        {
            Debug.LogWarning("CHARACTER_DIALOGUE_STATEMENT :: something is null, idk");
        }
    }

    public void Set(string message, string characterName = null, Sprite profilePic = null)
    {
        //message

        messageTMP.text = message;


        //name

        if (characterName == null)
        {
            characterName = "???";
        }
        else
        {
            characterNameTMP.text = characterName;
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
