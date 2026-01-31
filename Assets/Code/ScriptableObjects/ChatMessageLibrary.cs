using UnityEngine;

[CreateAssetMenu(
    fileName = "ChatMessageLibrary",
    menuName = "Scriptable Object/Chat Message Library"
)]
public class ChatMessageLibrary : ScriptableObject
{
    public ChatMessage[] messages;

    public ChatMessage GetRandom()
    {
        return messages[Random.Range(0, messages.Length)];
    }
}