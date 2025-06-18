using TMPro;
using UnityEngine;

public class ChapterButton : MonoBehaviour
{
    [SerializeField] SOChapters SO_chapters;
    [SerializeField] SOGameStartupPackage SO_gameStartupPackage;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI subTitle;
    [SerializeField] string windowNameDisplayOnClick;

    WindowManager manager;

    int chapterID = -1;

    public void Set(int chapterID)
    {
        if (chapterID >= SO_chapters.Size)
        {
            return;
        }

        this.chapterID = chapterID;

        title.text = $"Chapter {(chapterID+1).ToRomanNum()}";
        subTitle.text = SO_chapters.Get(chapterID).Title;
    }

    public void Click()
    {
        if (chapterID < 0)
        {
            return;
        }


        SO_gameStartupPackage.Chapter = chapterID;
        manager.Display(windowNameDisplayOnClick);
    }

    [System.Obsolete]
    private void Awake()
    {
        manager = FindObjectOfType<WindowManager>();
    }
}
