using System.Collections;

public class FanItem : BaseItem
{
    protected override IEnumerator DoRemoveMask()
    {
        Remove();
        removeCoroutine = null;
        yield break;
    }
}
