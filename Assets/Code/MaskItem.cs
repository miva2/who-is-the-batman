using System.Collections;
using UnityEngine;

public class MaskItem : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D[] bonesRbs;

    [SerializeField] private Sprite sprite;
    [SerializeField] private float delay;
    
    private Rigidbody2D rb;
    private Collider2D[] boneColliders;
    
    private SpriteRenderer sr;
    private MaterialPropertyBlock block;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        block = new MaterialPropertyBlock();
        
        boneColliders = GetComponentsInChildren<Collider2D>();
        
        Freeze(true);

        StartCoroutine(DoAddForce());
        
        SetMask(sprite);
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
        
        foreach (Collider2D col in boneColliders)
            col.enabled = !freeze;
    }

    public void Remove(Vector2 direction, float force)
    {
        Freeze(false);

        Rigidbody2D bone = GetClosestBoneInDirection(direction);
                
        rb.AddForce(direction * force, ForceMode2D.Impulse);
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
    }

    IEnumerator DoAddForce()
    {
        yield return new WaitForSeconds(1f + delay);
        Remove(new Vector2(0.5f, 0.5f), 175);
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
