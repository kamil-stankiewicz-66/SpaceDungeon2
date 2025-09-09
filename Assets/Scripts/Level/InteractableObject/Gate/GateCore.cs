using UnityEngine;

public class GateCore : MonoBehaviour
{
    [SerializeField] GameObject closedHandler;
    [SerializeField] GameObject openedHandler;


    public bool IsOpened { get; private set; }


    public void Open() => SetOpenState(true);

    public void Close() => SetOpenState(false);


    public void SetOpenState(bool state)
    {
        IsOpened = state;
        closedHandler.SetActive(!state);
        openedHandler.SetActive(state);
    }


    private void Awake()
    {
        Close();
    }
}
