using System.Collections;
using UnityEngine;

public class HandItem : BaseItem
{
    [Header("Hand")]
    [SerializeField]
    private Sprite[] handSprites;
    [SerializeField]
    private Transform handTransform;
    [SerializeField]
    private Transform endTransform;
    
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = handTransform.GetComponent<SpriteRenderer>();
    }

    protected override IEnumerator DoRemoveMask()
    {
        //hand go to mask
        float timer = 0;
        float halfDuration = animDuration / 2;
        
        Vector3 startPos = handTransform.position;
        
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            
            float delta = timer / halfDuration;
            handTransform.position = Vector3.Lerp(startPos, endTransform.position, delta);

            yield return null;
        }
        
        //remove
        Remove();
        spriteRenderer.sprite = handSprites[1];
        
        //hand go back
        timer = 0;
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            
            float delta = timer / halfDuration;
            handTransform.position = Vector3.Lerp(endTransform.position, startPos, delta);

            yield return null;
        }
        
        spriteRenderer.sprite = handSprites[0];
        handTransform.position = startPos;
        
        removeCoroutine = null;
    }
}
