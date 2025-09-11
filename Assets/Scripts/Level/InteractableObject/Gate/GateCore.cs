using UnityEngine;

public class GateCore : MonoBehaviour
{
    [SerializeField] GameObject closedHandler;
    [SerializeField] GameObject openedHandler;

    [SerializeField] SOItemData key;

    GameNotificationCaller notificationCaller;
    PlayerCore playerCore;


    public bool IsOpened { get; private set; }


    //message

    string MessageOpened()
    {
        if (key == null)
            return string.Empty;

        return new string($"Opened with {key.ItemName}.");
    }

    string MessageCantOpen()
    {
        if (key == null)
            return string.Empty;

        return new string($"You need a {key.ItemName} to open.");
    }    


    //init

    private void Awake()
    {
        playerCore = GameObject.FindAnyObjectByType<PlayerCore>();
        notificationCaller = GameObject.FindAnyObjectByType<GameNotificationCaller>();

        if (playerCore == null) Debug.LogWarning("GATE_CORE :: player core is null");
        if (notificationCaller == null) Debug.LogWarning("GATE_CORE :: notification caller is null");


        Close();
    }


    //actions

    public void Open()
    {
        if (IsOpened)
            return;

        if (key == null || playerCore.EquipmentSystem.IsItemInEquipment(key))
        {
            SetOpenState(true);
            notificationCaller.ShowLog(MessageOpened());
        }
        else
        {
            notificationCaller.ShowLog(MessageCantOpen());
        }
    }

    public void Close() => SetOpenState(false);


    public void SetOpenState(bool state)
    {
        IsOpened = state;
        closedHandler.SetActive(!state);
        openedHandler.SetActive(state);
    }

}
