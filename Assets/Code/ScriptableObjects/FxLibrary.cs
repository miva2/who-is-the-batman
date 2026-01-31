using UnityEngine;

[CreateAssetMenu(
    fileName = "FxLibrary",
    menuName = "Scriptable Object/Fx Library"
)]
public class FxLibrary : ScriptableObject
{
    [System.Serializable]
    public struct FxEntry
    {
        public string key;
        public AudioClip sound;
    }
    
    public FxEntry[] fx;
}
