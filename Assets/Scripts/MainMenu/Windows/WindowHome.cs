using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WindowHome : MonoBehaviour, IWindow
{
    //so
    [SerializeField] SOPlayerData SO_playerData;
    [SerializeField] SOParameters SO_parameters;
    [SerializeField] SOChaptersBase SO_chaptersBase;
    [SerializeField] SOGameStartupPackage SO_gameStartupPackage;

    //stats bar
    [SerializeField] TextMeshProUGUI text_maxHealth;
    [SerializeField] TextMeshProUGUI text_damageAdd;
    [SerializeField] TextMeshProUGUI text_coins;

    //level bar
    [SerializeField] TextMeshProUGUI text_level;
    [SerializeField] TextMeshProUGUI text_expPoints;
    [SerializeField] Slider slider_expPoints;

    //continue bar
    [SerializeField] GameObject continueBar;
    [SerializeField] TextMeshProUGUI text_lvlNum;

    //link
    WindowLevels windowLevels;

    //ram
    string maxHealthOriginal;
    string damageOriginal;
    string coinsOriginal;
    string levelOriginal;

    private void Awake()
    {
        windowLevels = FindAnyObjectByType<WindowLevels>(FindObjectsInactive.Include);

        maxHealthOriginal = text_maxHealth.text;
        damageOriginal = text_damageAdd.text;
        coinsOriginal = text_coins.text;
        levelOriginal = text_level.text;
    }

    public void Refresh()
    {
        //stats bar
        text_maxHealth.text = $"{maxHealthOriginal} {SO_playerData.Health}";
        text_damageAdd.text = $"{damageOriginal}{SO_playerData.Damage}";
        text_coins.text = $"{coinsOriginal} {SO_playerData.Coins}";

        //level bar
        text_level.text = $"{levelOriginal} {SO_playerData.ExpLevel}";
        slider_expPoints.maxValue = SO_parameters.PLAYER_EXP2PROMOTION;
        slider_expPoints.value = SO_playerData.Exp - ((SO_playerData.ExpLevel -1) * SO_parameters.PLAYER_EXP2PROMOTION);
        text_expPoints.text = SO_playerData.Exp.ToString();

        //continue bar
        (int, int) _levelPtr = SO_chaptersBase.GetCurrentOrLastLevelPtr();
        text_lvlNum.text = $"Lvl: {_levelPtr.Item1 + 1}.{_levelPtr.Item2 + 1}";
        continueBar.SetActive(true);
    }

    public void ContinueButton()
    {
        (int, int) _levelPtr = SO_chaptersBase.GetCurrentOrLastLevelPtr();
        SO_gameStartupPackage.Chapter = _levelPtr.Item1;
        SO_gameStartupPackage.Level = _levelPtr.Item2;
        windowLevels.ButtonPlay();
    }
}
