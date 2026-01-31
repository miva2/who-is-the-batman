using UnityEngine;

[CreateAssetMenu(
    fileName = "MaskLibrary",
    menuName = "Scriptable Object/Mask Library"
)]
public class MaskLibrary : ScriptableObject
{
    [System.Serializable]
    public struct MaskEntry
    {
        public Sprite Sprite;
        public string fxKey;
    }
    
    public MaskEntry[] masks;

    public MaskEntry GetRandom()
    {
        return masks[Random.Range(0, masks.Length)];
    }
}
