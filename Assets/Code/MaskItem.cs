using System.Collections;
using UnityEngine;
using UnityEngine.LowLevelPhysics2D;

public class MaskItem : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D[] bonesRbs;
    
    [SerializeField]
    private float resetTime;
    
    [SerializeField]
    private Sprite emptySprite;
    
    private Rigidbody2D rb;
    
    private Vector3[] bonesLocalPositions;
    private Quaternion[] bonesLocalRotations;
    
    private SpriteRenderer sr;
    private MaterialPropertyBlock block;

    private Vector3 startScale;

    private bool isFrozen;
    public bool Frozen => isFrozen;

    public void Initialize()
    {
        startScale = transform.localScale;
        
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        block = new MaterialPropertyBlock();
        
        bonesLocalPositions = new Vector3[bonesRbs.Length];
        bonesLocalRotations = new Quaternion[bonesRbs.Length];
        
        for (int i = 0; i < bonesRbs.Length; i++)
        {
            bonesLocalPositions[i] = bonesRbs[i].transform.localPosition;
            bonesLocalRotations[i] = bonesRbs[i].transform.localRotation;
        }
        
        Freeze(true);
    }

    public void SetMask(Sprite sprite)
    {
        sr.GetPropertyBlock(block);
        block.SetTexture("_OverrideTex", sprite.texture);
        sr.SetPropertyBlock(block);
    }

    public void Freeze(bool freeze)
    {
        RigidbodyType2D type = freeze ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
        
        rb.bodyType = type;
        
        foreach (Rigidbody2D boneRb in bonesRbs)
            boneRb.bodyType = type;
        
        isFrozen = freeze;
        
        if(freeze)
            SetMask(emptySprite);
    }

    public void Remove(Vector2 direction, float force)
    {
        Freeze(false);
        
        sr.sortingOrder = 1;

        Rigidbody2D bone = GetClosestBoneInDirection(direction);
                
        rb.AddForce(direction * force, ForceMode2D.Impulse);
        rb.AddTorque(180, ForceMode2D.Impulse);
        
        bone.AddForce(direction * force * 0.8f, ForceMode2D.Impulse);
        
        foreach (var b in bonesRbs)
        {
            float distance = Vector2.Distance(
                bone.worldCenterOfMass,
                b.worldCenterOfMass
            );
        
            float falloff = Mathf.Clamp01(1f - distance * 2f);
            b.AddForce(direction * force * falloff, ForceMode2D.Impulse);
        }

        StartCoroutine(DoWaitForReset());
    }

    public void Vacuum(Vector3 vacuum)
    {
        Freeze(false);
        
        sr.sortingOrder = 1;
        
        foreach (var b in bonesRbs)
        {
            b.AddForce(Random.insideUnitSphere * 30, ForceMode2D.Impulse);
        }
        
        StartCoroutine(DoVacuumWaitForReset(vacuum));
    }

    public void Reset()
    {
        StopAllCoroutines();

        sr.sortingOrder = 0;
        
        Freeze(true);
        
        transform.localScale = startScale;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // Reset rigidbody state
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Reset bones
        for (int i = 0; i < bonesRbs.Length; i++)
        {
            Rigidbody2D bone = bonesRbs[i];

            bone.linearVelocity = Vector2.zero;
            bone.angularVelocity = 0f;

            bone.transform.localPosition = bonesLocalPositions[i];
            bone.transform.localRotation = bonesLocalRotations[i];
        }
        
        gameObject.SetActive(false);
    }

    private IEnumerator DoWaitForReset()
    {
        yield return new WaitForSeconds(resetTime);
        Reset();
    }
    
    private IEnumerator DoVacuumWaitForReset(Vector3 vacuum)
    {
        float timer = 0;
        float vacuumDuration = 1;
        
        rb.bodyType = RigidbodyType2D.Kinematic;
        
        Vector3 startPos = transform.position;
        
        while (timer < resetTime)
        {
            timer += Time.deltaTime;
            
            float delta = timer / vacuumDuration;
            float delayedDelta = delta - 0.2f;

            if (delayedDelta > 0 && delayedDelta < 1)
            {
                transform.localScale = Vector3.Lerp(startScale, Vector3.zero, delayedDelta);
            }

            if (delta <= 1)
            {
                transform.position = Vector3.Lerp(startPos, vacuum, delta);
            }
            
            yield return null;
        }

        Reset();
    }
    
    private Rigidbody2D GetClosestBoneInDirection(Vector2 direction)
    {
        direction = direction.normalized;

        Rigidbody2D bestBone = null;
        float bestDot = -Mathf.Infinity;

        Vector2 rootPos = rb.worldCenterOfMass;

        foreach (var bone in bonesRbs)
        {
            Vector2 toBone = ((Vector2)bone.worldCenterOfMass - rootPos).normalized;
            float dot = Vector2.Dot(direction, toBone);

            if (dot > bestDot)
            {
                bestDot = dot;
                bestBone = bone;
            }
        }

        return bestBone;
    }
}
