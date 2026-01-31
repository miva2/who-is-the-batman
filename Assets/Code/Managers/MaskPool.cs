using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

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
        lastMask.SetMask(batmask);
        
        InvokeRepeating("Remove", 1f, 0.2f);
    }

    public void Remove()
    {
        MaskItem lastMask = pool[pool.Count - 1];
        
        lastMask.Remove(new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1, 1f)), UnityEngine.Random.Range(150, 200f));
        pool.RemoveAt(pool.Count - 1);
        pool.Insert(0, lastMask);
        
        lastMask = pool[pool.Count - 1];
        
        if(!lastMask.Frozen)
            lastMask.Reset();

        MaskLibrary.MaskEntry entry = maskLibrary.GetRandom();
        lastMask.SetMask(entry.Sprite); //choose random
        SoundManager.Instance.PlayFX(entry.fxKey != "" ? entry.fxKey : "default mask");
    }
}
