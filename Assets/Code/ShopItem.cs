using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour, IPointerEnterHandler
{
    [Header("UI")]
    public Button item;
    public Image icon;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI priceText;
    public Image buyImage;

    [Header("Item Info")] 
    public string title;
    [TextArea]
    public string description;

    public void Fill(BaseItem baseItem)
    {
        icon.sprite = baseItem.Icon;
        levelText.text = baseItem.Level.ToString("00");

        if (baseItem.Level == baseItem.MaxLevel)
        {
            item.interactable = false;
            item.GetComponent<Image>().color = Color.gray;
            buyImage.color = Color.gray;
            priceText.text = "Max";
            return;
        }
            
        priceText.text = baseItem.GetPrice().FormatNumber();

        if (baseItem.GetPrice().IsBiggerThan(GameManager.Instance.CurrentMoney))
        {
            buyImage.color = Color.red;
        }
        else
        {
            buyImage.color = Color.green;
        }
            
        icon.color = baseItem.Level == 0 ? Color.gray : Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.FillDescription(this);
    }
}
