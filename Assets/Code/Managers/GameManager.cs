using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //non-automatic
    [SerializeField] private BaseItem handRight;
    [SerializeField] private BaseItem handLeft;

    [SerializeField] private BaseItem[] items;
    
    [SerializeField] private HugeNumber finisherPrice;

    private HugeNumber total;
    public HugeNumber CurrentTotal => total;
    private HugeNumber money;
    public HugeNumber CurrentMoney => money;
    private HugeNumber rate;
    public HugeNumber CurrentRate => rate;
    
    public HugeNumber FinisherPrice => finisherPrice;

    private int maskValue;
    private int maskValueLevel = 0;
    
    private float maxCombo = 1.1f;
    private int maxComboLevel = 0;
    
    private float currentCombo = 1;
    private float currentComboAdd = 0.01f;
    
    private bool unlockedDonations = false;

    [Header("Mask value")] 
    [SerializeField] private HugeNumber maskValueBasePrice;
    [SerializeField] private float maskValueMultiplier = 1.2f;
    
    [Header("Max Combo")] 
    [SerializeField] private HugeNumber maxComboBasePrice;
    [SerializeField] private float maxComboAdd = 0.1f;
    
    [Header("Chat donation")] 
    [SerializeField] private HugeNumber donationBasePrice;
    
    
    public bool UnlockedDonations => unlockedDonations;
    public int MaxComboLevel => maxComboLevel;
    public int MaskValueLevel => maskValueLevel;

    private float lastClickTime = 0;
    private float comboWaitDuration = 1;

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
        total = new HugeNumber(0,1);
        money = new HugeNumber(0,1);
        rate = new HugeNumber(0,1);

        maskValue = 1;
        
        money = handLeft.GetPrice();
        Upgrade(0);
        
        UIManager.Instance.UpdateTopBar(total, money, rate);
        UIManager.Instance.UpdateMaskValue(maskValue);
        UIManager.Instance.UpdateCombo(currentCombo);
        UIManager.Instance.ShowNotif(false);
    }

    private void Update()
    {
        lastClickTime -= Time.deltaTime;

        if (lastClickTime < 0)
        {
            lastClickTime = 0;
            currentCombo -= currentComboAdd * Time.deltaTime * 5;
            currentCombo = Mathf.Clamp(currentCombo, 1, maxCombo);
            UIManager.Instance.UpdateCombo(currentCombo);
        }
    }

    public void ClickMask()
    {
        lastClickTime = comboWaitDuration;
        
        currentCombo += currentComboAdd;
        currentCombo = Mathf.Clamp(currentCombo, 1, maxCombo);
        
        if (!handLeft.RemoveMask())
        {
            if (handRight.gameObject.activeInHierarchy)
                handRight.RemoveMask();
        }
        
        UIManager.Instance.UpdateCombo(currentCombo);
    }

    public void Upgrade(int index)
    {
        BaseItem item = items[index];

        if (money.IsSmallerThan(item.GetPrice()))
        {
            Debug.Log("Not enough money");
            return;
        }

        money = money.Subtract(item.GetPrice());
        item.Upgrade();

        UpdateRate();
        
        UIManager.Instance.UpdateTopBar(total, money, rate);
        UIManager.Instance.UpdateShop();
        UIManager.Instance.ShowNotif(CanBuyUpgrade());
    }

    public void Gain(HugeNumber amount)
    {
        amount = amount.Mult(currentCombo); //multiply by combo
        
        total = total.Add(amount);
        money = money.Add(amount.Mult(maskValue));
        UIManager.Instance.UpdateTopBar(total, money, rate);

        bool canBuy = CanBuyUpgrade();
        UIManager.Instance.ShowNotif(canBuy);
    }

    public BaseItem GetItem(int index)
    {
        return items[index];
    }

    private void UpdateRate()
    {
        rate = new HugeNumber(0,1);

        for(int i = 2; i < items.Length; i++)
            rate = rate.Add(items[i].GetRate());
    }

    private bool CanBuyUpgrade()
    {
        foreach (BaseItem it in items)
        {
            if(it.GetPrice().IsSmallerThan(money))
                return true;
        }
        
        return false;
    }

    public void IncreaseMaskValue()
    {
        Debug.Log(money);
        Debug.Log(GetMaskValuePrice());
        if (money.IsBiggerThan(GetMaskValuePrice()))
        {
            Debug.Log(money.FormatNumber());
            Debug.Log(GetMaskValuePrice().FormatNumber());
            
            money = money.Subtract(GetMaskValuePrice());
            maskValueLevel++;
            maskValue = Mathf.CeilToInt(maskValue * maskValueMultiplier);
            
            UIManager.Instance.UpdateTopBar(total, money, rate);
            UIManager.Instance.UpdateShop();
            UIManager.Instance.UpdateMaskValue(maskValue);
            UIManager.Instance.ShowNotif(CanBuyUpgrade());
        }
    }

    public HugeNumber GetMaskValuePrice()
    {
        return maskValueBasePrice.Mult(Mathf.Pow(1.15f, maskValueLevel));
    }

    public int GetNextMaskValue()
    {
        return Mathf.CeilToInt(maskValue * maskValueMultiplier);
    }
    
    public void IncreaseMaxCombo()
    {
        if (money.IsBiggerThan(GetMaxComboPrice()))
        {
            money = money.Subtract(GetMaxComboPrice());
            maxComboLevel++;
            maxCombo += maxComboAdd;
            UIManager.Instance.UpdateTopBar(total, money, rate);
            UIManager.Instance.UpdateShop();
            UIManager.Instance.ShowNotif(CanBuyUpgrade());
        }
    }

    public HugeNumber GetMaxComboPrice()
    {
        return maxComboBasePrice.Mult(Mathf.Pow(1.15f, maxComboLevel));
    }
    
    public float GetNextMaxCombo()
    {
        return maxCombo + maxComboAdd;
    }

    public void UnlockDonations()
    {
        if (money.IsBiggerThan(donationBasePrice))
        {
            unlockedDonations = true;
            UIManager.Instance.UpdateTopBar(total, money, rate);
            UIManager.Instance.UpdateShop();
            UIManager.Instance.ShowNotif(CanBuyUpgrade());
        }
    }

    public HugeNumber GetDonationBasePrice()
    {
        return donationBasePrice;
    }
}
