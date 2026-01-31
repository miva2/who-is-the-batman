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
    [SerializeField] protected int basePrice; //need big number here
    [SerializeField] protected AnimationCurve priceMultiplier; 
    [SerializeField] protected int upgradesAmount;
    
    [Header("Upgrades")]
    [SerializeField] protected float maxAnimDuration = 2;
    [SerializeField] protected float minAnimDuration = 0.1f;
    [SerializeField] protected int minMaskRemoveAmount = 1; //need big number here
    [SerializeField] protected int maxMaskRemoveAmount = 10; //need big number here
    [SerializeField] protected float maxAutoCooldown = 1;
    [SerializeField] protected float minAutoCooldown = 1;

    protected int currentUpgrade = 0;
    protected float timer = 0;
    protected float cooldown;
    protected int maskRemoveAmount;
    protected float animDuration;
    protected Coroutine removeCoroutine;

    public int GetPrice()
    {
        float step = (float)currentUpgrade / upgradesAmount;
        return Mathf.CeilToInt(basePrice * priceMultiplier.Evaluate(step));
    }

    public void Upgrade()
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
            float step = (float)currentUpgrade / upgradesAmount;
            cooldown = Mathf.Lerp(maxAutoCooldown, minAutoCooldown, step);
            animDuration = Mathf.Lerp(maxAnimDuration, minAnimDuration, step);
            maskRemoveAmount = Mathf.CeilToInt(Mathf.Lerp(minMaskRemoveAmount, maxMaskRemoveAmount, step));
        }

        currentUpgrade++;
    }

    private void Update()
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

    protected void Remove()
    {
        Vector2 dir = new Vector2(Random.Range(minDir.x,  maxDir.x), Random.Range(minDir.y, maxDir.y));
        float force = Random.Range(minForce, maxForce);

        MaskPool.Instance.Remove(dir.normalized, force);
        
        //gain maskRemoveAmount 
    }
}
