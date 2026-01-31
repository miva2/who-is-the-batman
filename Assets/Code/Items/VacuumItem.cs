using System.Collections;
using UnityEngine;

public class VacuumItem : BaseItem
{
    protected override IEnumerator DoRemoveMask()
    {
        Remove(true);
        removeCoroutine = null;
        yield break;
    }
}
