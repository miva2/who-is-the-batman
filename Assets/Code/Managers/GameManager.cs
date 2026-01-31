using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //non-automatic
    [SerializeField] private BaseItem handRight;
    [SerializeField] private BaseItem handLeft;

    [SerializeField] private BaseItem[] items;

    private HugeNumber total; 
    private HugeNumber money;
    private HugeNumber rate;

    private int maskValue;
    
    public HugeNumber CurrentMoney => money;

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
        UIManager.Instance.ShowNotif(false);
    }

    public void ClickMask()
    {
        if (!handLeft.RemoveMask())
        {
            if (handRight.gameObject.activeInHierarchy)
                handRight.RemoveMask();
        }
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
    }

    public void Gain(HugeNumber amount)
    {
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
}
