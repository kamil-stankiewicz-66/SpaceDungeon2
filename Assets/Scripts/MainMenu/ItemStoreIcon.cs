using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class ItemStoreIcon : MonoBehaviour
{
    [SerializeField] GameObject lockedHolder;
    [SerializeField] GameObject unLockedHolder;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI text_weaponName;

    Button button;
    ColorBlock basic_colorBlock;
    ColorBlock clicked_colorBlock;
    bool isClicked;

    UnityAction<short> unityAction;
    short id;

    /// <summary>
    /// public
    /// </summary>

    internal void Set(Sprite _icon,
                          string _name,
                          bool _visible,
                          short _id,
                          UnityAction<short> _unityAction)
    {
        icon.sprite = _icon;
        text_weaponName.text = _name;
        id = _id;

        unLockedHolder.SetActive(_visible);
        lockedHolder.SetActive(!_visible);

        unityAction = _unityAction;

        //colors
        basic_colorBlock = button.colors;
        clicked_colorBlock = new ColorBlock();
        clicked_colorBlock.normalColor = basic_colorBlock.pressedColor;
        clicked_colorBlock.highlightedColor = basic_colorBlock.pressedColor;
        clicked_colorBlock.pressedColor = basic_colorBlock.normalColor;
        clicked_colorBlock.selectedColor = basic_colorBlock.pressedColor;
        clicked_colorBlock.disabledColor = basic_colorBlock.pressedColor;
        clicked_colorBlock.colorMultiplier = basic_colorBlock.colorMultiplier;
        clicked_colorBlock.fadeDuration = basic_colorBlock.fadeDuration;
    }

    internal void ChangeState(bool _isClicked)
    {
        if (_isClicked == isClicked)
            return;

        button.colors = _isClicked
            ? clicked_colorBlock
            : basic_colorBlock;

        isClicked = _isClicked;
        print("ItemStoreIcon: change state to: isClicked == " + _isClicked);
    }

    public void OnClickButton()
    {
        unityAction?.Invoke(id);
    }


    /// <summary>
    /// unuty caller
    /// </summary>

    private void OnEnable()
    {
        button = gameObject.GetComponent<Button>();
    }

}
