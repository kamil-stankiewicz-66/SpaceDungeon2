using UnityEngine;

public class WindowChapters : MonoBehaviour, IWindow
{
    [SerializeField] SOChapters SO_chapters;
    [SerializeField] GameObject chapterButtonPrefab;
    [SerializeField] Transform container;

    public void Refresh() { }

    private void Start()
    {
        for (int i = 0; i < SO_chapters.Get.Length; i++)
        {
            SOChapter item = SO_chapters.Get[i];
            CreateButton(i);
        }
    }

    void CreateButton(int index)
    {
        if (index > SO_chapters.Size)
        {
            return;
        }

        GameObject button = Instantiate(chapterButtonPrefab, parent: container);

        if (button.TryGetComponent<ChapterButton>(out var core))
        {
            core.Set(index);
        }
    }
}
