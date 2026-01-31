using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BaseItem handRight;
    [SerializeField] private BaseItem handLeft;

    private int total = 0;
    private int current = 0;
    
    public void ClickMask()
    {
        if (!handRight.RemoveMask())
        {
            handLeft.RemoveMask();
        }
    }
}
