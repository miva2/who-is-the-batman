using System.Collections;
using UnityEngine;

public class TrumpetItem : BaseItem
{
    [Header("Trumpet")]
    [SerializeField] private int burst;
    [SerializeField] private AudioClip clip;
    [SerializeField] private Transform gameCamera;
    protected override IEnumerator DoRemoveMask()
    {
        //use 3 s for now, but clip length after
        StartCoroutine(DoShakeTrumpet(3));
        
        float duration = (float)3 / burst;
        //play sound

        for (int i = 0; i < burst; i++)
        {
            Remove(); //TODO should divide the amountRemove by burst
            yield return new WaitForSeconds(duration);
        }
        
        removeCoroutine = null;
    }

    private IEnumerator DoShakeTrumpet(float duration)
    {
        Vector3 startPos = transform.position;
        Vector3 cameraPos = gameCamera.position;

        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;

            transform.position = startPos + Random.insideUnitSphere * 0.1f;
            gameCamera.position = cameraPos + Random.insideUnitSphere * 0.05f;
            
            yield return null;
        }

        transform.position = startPos;
        gameCamera.position = cameraPos;
    }
}
