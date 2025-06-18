using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DialogWindowCaller : MonoBehaviour
{
    [SerializeField] GameObject dialogWindow;
    [SerializeField] TextMeshProUGUI text_contents;

    private UnityEvent e_accept;
    private UnityEvent e_reject;

    public void Show(string _contents, UnityEvent e_accept, UnityEvent e_reject = null)
    {
        text_contents.text = _contents;

        if (!dialogWindow.activeSelf)
            dialogWindow.SetActive(true);

        this.e_accept = e_accept;
        this.e_reject = e_reject;
    }

    public void ButtonAccept()
    {
        e_accept?.Invoke();
        Hide();
    }

    public void ButtonReject()
    {
        e_reject?.Invoke();
        Hide();
    }

    private void Hide()
    {
        if (dialogWindow.activeSelf)
            dialogWindow.SetActive(false);

        e_accept = null;
        e_reject = null;
    }
}
