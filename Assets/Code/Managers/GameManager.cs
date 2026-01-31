using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //non-automatic
    [SerializeField] private BaseItem handRight;
    [SerializeField] private BaseItem handLeft;

    [SerializeField] private BaseItem[] items;

    private int total = 0;
    [SerializeField]
    private int current = 10000;

    private void Start()
    {
        handRight.Upgrade();
    }

    public void ClickMask()
    {
        if (!handRight.RemoveMask())
        {
            if(handLeft.gameObject.activeInHierarchy)
                handLeft.RemoveMask();
        }
    }

    public void Upgrade(int index)
    {
        BaseItem item = items[index];
        
        current -= item.GetPrice();
        item.Upgrade();
    }
}
