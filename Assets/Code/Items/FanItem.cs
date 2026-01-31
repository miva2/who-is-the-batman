using System.Collections;
using UnityEngine;

public class FanItem : BaseItem
{
    [Header("Fan")]
    [SerializeField] private Transform fan;
    [SerializeField] private float rotationSpeedMin;
    [SerializeField] private float rotationSpeedMax;
    
    private float rotationSpeed;
    
    protected override IEnumerator DoRemoveMask()
    {
        Remove();
        removeCoroutine = null;
        yield break;
    }

    public override void Upgrade()
    {
        base.Upgrade();
        
        rotationSpeed = Mathf.Lerp(rotationSpeedMin, rotationSpeedMax, (float)currentUpgrade / upgradesAmount);
    }

    protected override void Update()
    {
        base.Update();
        
        fan.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
    }
}
