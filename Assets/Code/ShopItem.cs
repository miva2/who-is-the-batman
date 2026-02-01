using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    public Button item;
    public Image icon;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI priceText;
    public GameObject buyImageGood;
    public GameObject buyImageBad;

    [Header("Item Info")] 
    public string title;
    [TextArea]
    public string description;
    public Sprite IconSprite;

    public void Fill(BaseItem baseItem)
    {
        icon.sprite = IconSprite;
        levelText.text = baseItem.Level.ToString("00");

        if (baseItem.Level == baseItem.MaxLevel)
        {
            item.interactable = false;
            item.GetComponent<Image>().color = Color.gray;
            buyImageGood.SetActive(false);
            buyImageBad.SetActive(true);
            priceText.text = "Max";
            return;
        }
            
        priceText.text = baseItem.GetPrice().FormatNumber();

        if (baseItem.GetPrice().IsBiggerThan(GameManager.Instance.CurrentMoney))
        {
            buyImageGood.SetActive(false);
            buyImageBad.SetActive(true);
        }
        else
        {
            buyImageGood.SetActive(true);
            buyImageBad.SetActive(false);
        }
            
        icon.color = baseItem.Level == 0 ? Color.gray : Color.white;
    }

    public void Fill(int level, HugeNumber price)
    {
        icon.sprite = IconSprite;
        
        if(level > -1)
            levelText.text = level.ToString("00");

        if (level == 99)
        {
            item.interactable = false;
            item.GetComponent<Image>().color = Color.gray;
            buyImageGood.SetActive(false);
            buyImageBad.SetActive(true);
            priceText.text = "Max";
            return;
        }
            
        priceText.text = price.FormatNumber();

        if (price.IsBiggerThan(GameManager.Instance.CurrentMoney)) 
        {
            buyImageGood.SetActive(false);
            buyImageBad.SetActive(true);
        }
        else
        {
            buyImageGood.SetActive(true);
            buyImageBad.SetActive(false);
        }
            
        icon.color = level == 0 ? Color.gray : Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.FillDescription(this);
        transform.localScale = Vector3.one * 1.05f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }
}
