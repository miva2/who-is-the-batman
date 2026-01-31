using System.Collections;
using UnityEngine;

public class HeaterItem : BaseItem
{
    [Header("Heater")]
    [SerializeField] protected Transform maskTransform;
    [SerializeField] protected Transform maskEndPosition;
    [SerializeField] protected Transform armTransform;
    [SerializeField] protected SpriteRenderer armRenderer;
    [SerializeField] protected Sprite[] armSprites;
    [SerializeField] protected Transform armEndPosition;
    
    protected override IEnumerator DoRemoveMask()
    {
        Vector3 maskStartPosition = maskTransform.position;
        Vector3 armStartPosition = armTransform.position;
        float duration = animDuration / 3;
        
        //go up until filled
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            
            maskTransform.position = Vector3.Lerp(maskStartPosition, maskEndPosition.position, timer / duration);
            
            yield return null;
        }

        //show arm grab mask
        timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            
            armTransform.position = Vector3.Lerp(armStartPosition, armEndPosition.position, timer / duration);
            
            yield return null;
        }

        //remove
        Remove();
        armRenderer.sprite = armSprites[1];
        
        timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            
            armTransform.position = Vector3.Lerp(armEndPosition.position, armStartPosition, timer / duration);
            
            yield return null;
        }

        //reset
        maskTransform.position = maskStartPosition;
        armTransform.position = armStartPosition;
        armRenderer.sprite = armSprites[0];
        
        removeCoroutine = null;
    }
}
