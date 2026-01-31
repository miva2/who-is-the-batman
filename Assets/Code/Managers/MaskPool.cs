using System;
using System.Collections.Generic;
using UnityEngine;

public class MaskPool : MonoBehaviour
{
    [SerializeField] private MaskLibrary maskLibrary;
    [SerializeField] private int poolSize;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Sprite batmask;
    
    private List<MaskItem> pool;

    private void Start()
    {
        pool = new List<MaskItem>(poolSize);

        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = Instantiate(prefab, transform);
            MaskItem item = go.GetComponent<MaskItem>();

            item.Initialize();
            
            pool.Add(item);
        }
        
        MaskItem lastMask = pool[pool.Count - 1];
        lastMask.SetMask(batmask); //should be batman mask
        
        InvokeRepeating("Remove", 1f, 0.5f);
    }

    public void Remove()
    {
        MaskItem lastMask = pool[pool.Count - 1];
        
        lastMask.Remove(new Vector2(0.5f, 0.5f), 175f);
        pool.RemoveAt(pool.Count - 1);
        pool.Insert(0, lastMask);
        
        lastMask = pool[pool.Count - 1];
        
        if(!lastMask.Frozen)
            lastMask.Reset();
        
        lastMask.SetMask(maskLibrary.GetRandom().Sprite); //choose random
        //play sound
    }
}
