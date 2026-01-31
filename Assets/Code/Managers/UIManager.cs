using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [Header("Top bar")] [SerializeField] private TextMeshProUGUI rateText;
    [SerializeField] private TextMeshProUGUI totalText;
    [SerializeField] private TextMeshProUGUI moneyText;

    [Header("Shop")]
    [SerializeField] private GameObject shop;
    [SerializeField] private ShopItem[] shopItems;
    [SerializeField] private ShopItem[] buffItems;
    
    [Header("Item description")]
    [SerializeField] private GameObject itemPanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private TextMeshProUGUI lvlText;
    [SerializeField] private TextMeshProUGUI coolDownText;
    
    [Header("Buff description")]
    [SerializeField] private GameObject buffPanel;
    [SerializeField] private TextMeshProUGUI titleText2;
    [SerializeField] private TextMeshProUGUI descriptionText2;
    [SerializeField] private TextMeshProUGUI statKeyText;
    [SerializeField] private TextMeshProUGUI statText;
    
    [Header("Buttons")]
    [SerializeField] private GameObject notif;
    [SerializeField] private Button openShop;
    [SerializeField] private Button closeShop;
    
    private ShopItem currentOverIndex = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        openShop.onClick.AddListener(() =>
        {
            OpenShop(true);
        });
        
        closeShop.onClick.AddListener(() =>
        {
            OpenShop(false);
        });
        
        OpenShop(false);

        for(int i = 0; i < shopItems.Length; i++)
        {
            int index = i;
            shopItems[i].item.onClick.AddListener(() =>
            {
                GameManager.Instance.Upgrade(index);
            });
        }
        
        
        // TODO do buffs
    }

    public void UpdateTopBar(HugeNumber total, HugeNumber money, HugeNumber rate)
    {
        totalText.text = total.FormatNumber() + " masks";
        moneyText.text = money.FormatNumber();
        rateText.text = rate.FormatNumber() + " masks/s";
    }

    public void UpdateShop()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            shopItems[i].Fill(GameManager.Instance.GetItem(i));
        }

        if (currentOverIndex != null)
            FillDescription(currentOverIndex);
    }

    public void FillDescription(ShopItem uiItem)
    {
        currentOverIndex = uiItem;
        int index = Array.IndexOf(shopItems, uiItem);
        
        Debug.Log(index);

        if (index != -1)
        {
            itemPanel.SetActive(true);
            buffPanel.SetActive(false);
            
            titleText.text = uiItem.title;
            descriptionText.text = uiItem.description;
            
            BaseItem item = GameManager.Instance.GetItem(index);
            
            amountText.text = item.GetNextLevelAmount().FormatNumber(); 
            lvlText.text = item.GetNextLevel().ToString();
            coolDownText.text = item.GetNextLevelCoolDown().ToString("0.00") + "s";
            return;
        }
        
        itemPanel.SetActive(false);
        buffPanel.SetActive(true);
        
        
        //find corresponding baseItem
        //title
        //level
        //stat
        //...
        
    }

    public void ShowNotif(bool canBuy)
    {
        notif.SetActive(canBuy);
    }

    private void OpenShop(bool isOpen)
    {
        shop.SetActive(isOpen);
        UpdateShop();
    }
}
