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
        
        // BUFFS
        buffItems[0].item.onClick.AddListener(() =>
        {
            GameManager.Instance.IncreaseMaskValue();
        });
        
        buffItems[1].item.onClick.AddListener(() =>
        {
            GameManager.Instance.UnlockDonations();
        });
        
        buffItems[2].item.onClick.AddListener(() =>
        {
            GameManager.Instance.IncreaseMaxCombo();
        });
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
        
        buffItems[0].Fill(GameManager.Instance.MaskValueLevel, GameManager.Instance.GetMaskValuePrice());
        buffItems[1].Fill(GameManager.Instance.UnlockedDonations ? 99 : 0, GameManager.Instance.GetDonationBasePrice());
        buffItems[2].Fill(GameManager.Instance.MaxComboLevel, GameManager.Instance.GetMaxComboPrice());

        if (currentOverIndex != null)
            FillDescription(currentOverIndex);
    }

    public void FillDescription(ShopItem uiItem)
    {
        currentOverIndex = uiItem;
        int index = Array.IndexOf(shopItems, uiItem);

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
        
        index = Array.IndexOf(buffItems, uiItem);
        
        titleText2.text = uiItem.title;
        descriptionText2.text = uiItem.description;
        
        switch (index)
        {
            case 0:
                statKeyText.text = "Resell value";
                statText.text = GameManager.Instance.GetNextMaskValue().ToString(); 
                break;
            case 1:
                statKeyText.text = "Stream donation";
                statText.text = "open!";
                break;
            case 2:
                statKeyText.text = "Max combo";
                statText.text = GameManager.Instance.GetNextMaxCombo().ToString("0.0"); 
                break;
        }
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
