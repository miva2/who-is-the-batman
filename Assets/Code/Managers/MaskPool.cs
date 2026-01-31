using System.Collections.Generic;
using UnityEngine;

public class MaskPool : MonoBehaviour
{
    public static MaskPool Instance;
    
    [SerializeField] private MaskLibrary maskLibrary;
    [SerializeField] private int poolSize;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Sprite batmask;
    
    private List<MaskItem> pool;

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
    }

    public void Remove(Vector2 direction, float force)
    {
        MaskItem lastMask = pool[pool.Count - 1];
        
        lastMask.Remove(direction, force);
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
