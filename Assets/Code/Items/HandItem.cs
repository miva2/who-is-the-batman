using System.Collections;
using UnityEngine;

public class HandItem : BaseItem
{
    [SerializeField]
    private Sprite[] handSprites;
    
    private Vector3 startPos;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = transform.position;
    }

    protected override IEnumerator DoRemoveMask()
    {
        //hand go to mask
        float timer = 0;
        float halfDuration = animDuration / 2;
        
        Vector3 maskPos = MaskPool.Instance.transform.position;
        
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            
            float delta = timer / halfDuration;
            transform.position = Vector3.Lerp(startPos, maskPos, delta);

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
            transform.position = Vector3.Lerp(maskPos, startPos, delta);

            yield return null;
        }
        
        spriteRenderer.sprite = handSprites[0];
        
        removeCoroutine = null;
    }
}
