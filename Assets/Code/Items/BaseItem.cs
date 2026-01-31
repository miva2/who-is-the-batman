using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseItem : MonoBehaviour
{
    [SerializeField] protected bool automatic = true;
    
    [Header("Mask")]
    [SerializeField] protected Vector2 minDir;
    [SerializeField] protected Vector2 maxDir;
    [SerializeField] protected float minForce;
    [SerializeField] protected float maxForce;
    
    [Header("Shop")]
    [SerializeField] protected HugeNumber basePrice; //need big number here
    [SerializeField] protected float priceMultiplier = 1.15f; 
    [SerializeField] protected int upgradesAmount;
    [SerializeField] protected Sprite icon;
    
    [Header("Upgrades")]
    [SerializeField] protected float maxAnimDuration = 2;
    [SerializeField] protected float minAnimDuration = 0.1f;
    [SerializeField] protected HugeNumber minMaskRemoveAmount; //need big number here
    [SerializeField] protected HugeNumber maxMaskRemoveAmount; //need big number here
    [SerializeField] protected float maxAutoCooldown = 1;
    [SerializeField] protected float minAutoCooldown = 1;
    
    [Header("Debug")]
    [SerializeField] 
    protected int currentUpgrade = 0;
    [SerializeField] 
    protected float cooldown;
    [SerializeField] 
    protected HugeNumber maskRemoveAmount;
    [SerializeField] 
    protected float animDuration;
    
    protected float timer = 0;
    protected Coroutine removeCoroutine;
    
    public Sprite Icon => icon;
    public int Level => currentUpgrade;

    public int MaxLevel => upgradesAmount;

    public HugeNumber GetPrice()
    {
        return basePrice.Mult(Mathf.Pow(priceMultiplier, currentUpgrade));
    }

    public HugeNumber GetRate()
    {
        return maskRemoveAmount.Mult(1f/cooldown);
    }

    public virtual void Upgrade()
    {
        if (!gameObject.activeInHierarchy) //initial purchase
        {
            gameObject.SetActive(true);
            cooldown = maxAutoCooldown;
            animDuration = maxAnimDuration;
            maskRemoveAmount = minMaskRemoveAmount;
        }
        else
        {
            float delta = (float)currentUpgrade / upgradesAmount;
            cooldown = Mathf.Lerp(maxAutoCooldown, minAutoCooldown, delta);
            animDuration = Mathf.Lerp(maxAnimDuration, minAnimDuration, delta);
            maskRemoveAmount = HugeNumber.Lerp(minMaskRemoveAmount, maxMaskRemoveAmount, delta);
        }
        
        currentUpgrade++;

        if (currentUpgrade >= upgradesAmount)
            currentUpgrade = upgradesAmount;
    }

    protected virtual void Update()
    {
        if(!automatic) return;
        
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            RemoveMask();
            timer = cooldown;
        }
    }
    
    public bool RemoveMask()
    {
        if (removeCoroutine != null)
        {
            return false;
        }
        
        removeCoroutine = StartCoroutine(DoRemoveMask());
        return true;
    }
    
    //override this and add custom animation
    protected virtual IEnumerator DoRemoveMask()
    {
        yield break;
    }

    protected void Remove(bool vacuum = false)
    {
        Vector2 dir = new Vector2(Random.Range(minDir.x,  maxDir.x), Random.Range(minDir.y, maxDir.y));
        float force = Random.Range(minForce, maxForce) + (((float)currentUpgrade / upgradesAmount) * 100);

        if(vacuum)
            MaskPool.Instance.SpecialRemove();
        else
            MaskPool.Instance.Remove(dir.normalized, force);
        
        GameManager.Instance.Gain(maskRemoveAmount);
    }

    public HugeNumber GetNextLevelAmount()
    {
        if (currentUpgrade == upgradesAmount)
        {
            return maskRemoveAmount;
        }

        float delta = (float)(currentUpgrade + 1) / upgradesAmount;
        return HugeNumber.Lerp(minMaskRemoveAmount, maxMaskRemoveAmount, delta);
    }
    
    public int GetNextLevel()
    {
        if (currentUpgrade == upgradesAmount)
        {
            return currentUpgrade;
        }
        
        return currentUpgrade + 1;
    }
    
    public float GetNextLevelCoolDown()
    {
        if (currentUpgrade == upgradesAmount)
        {
            return cooldown;
        }
        
        float delta = (float)(currentUpgrade + 1) / upgradesAmount;
        return Mathf.Lerp(maxAutoCooldown, minAutoCooldown, delta);
    }
}
