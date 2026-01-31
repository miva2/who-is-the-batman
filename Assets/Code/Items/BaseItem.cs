using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseItem : MonoBehaviour
{
    //add base price, multiplier, etc
    
    [SerializeField] protected float animDuration = 2;
    [SerializeField] protected int maskRemoveAmount = 1;
    [SerializeField] protected bool automatic = true;
    [SerializeField] protected float cooldown = 1;
    
    [SerializeField] protected Vector2 minDir;
    [SerializeField] protected Vector2 maxDir;
    [SerializeField] protected float minForce;
    [SerializeField] protected float maxForce;

    protected float timer = 0;
    protected Coroutine removeCoroutine;

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
