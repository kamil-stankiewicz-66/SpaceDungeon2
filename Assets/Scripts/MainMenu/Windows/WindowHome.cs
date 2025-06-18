using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WindowHome : MonoBehaviour, IWindow
{
    //so
    [SerializeField] SOPlayerData SO_playerData;
    [SerializeField] SOParameters SO_parametars;

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

    //ram
    string maxHealthOriginal;
    string damageOriginal;
    string coinsOriginal;
    string levelOriginal;

    private void Awake()
    {
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
        slider_expPoints.maxValue = 100;
        slider_expPoints.value = 100 - Mathf.Abs(SO_playerData.Exp - SO_parametars.PLAYER_EXP2PROMOTION);
        text_expPoints.text = SO_playerData.Exp.ToString();

        //continue bar
        //levelsBase.currentLevelPointer = levelsBase.GetNextLevelPointer();
        //text_lvlNum.text = $"Lvl: {levelsBase.currentLevelPointer.chapterNum + 1}.{levelsBase.currentLevelPointer.levelNum + 1}";
        //continueBar.SetActive(true);
    }
}
