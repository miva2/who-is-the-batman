using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //non-automatic
    [SerializeField] private BaseItem handRight;
    [SerializeField] private BaseItem handLeft;

    [SerializeField] private BaseItem[] items;

    private int total = 0; 
    private int money = 0;

    private int maskValue = 1;

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
        handRight.Upgrade();
    }

    public void ClickMask()
    {
        if (!handRight.RemoveMask())
        {
            if (handLeft.gameObject.activeInHierarchy)
                handLeft.RemoveMask();
        }
    }

    public void Upgrade(int index)
    {
        BaseItem item = items[index];

        money -= item.GetPrice();
        item.Upgrade();
    }

    public void Gain(int amount)
    {
        total += amount;
        money += amount * maskValue;
    }
}
