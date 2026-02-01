using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatManager: MonoBehaviour
{
    public static ChatManager Instance;

    [SerializeField]
    private ChatMessage[] chatMessages;

    [SerializeField] private int maxMessagesOnScreen = 8;

    private int messageIndex = 0;
    
    [SerializeField] private ChatMessageLibrary chatMessageLibrary;
    
    
    [SerializeField] private TextMeshProUGUI message1Text;
    [SerializeField] private TextMeshProUGUI message2Text;
    [SerializeField] private TextMeshProUGUI message3Text;
    [SerializeField] private TextMeshProUGUI message4Text;
    [SerializeField] private TextMeshProUGUI message5Text;
    [SerializeField] private TextMeshProUGUI message6Text;
    [SerializeField] private TextMeshProUGUI message7Text;
    [SerializeField] private TextMeshProUGUI message8Text;


    
    // array 8
    // startindex 0 ==> messages 0, 1, 2, 3, 4, 5, 6, 7
    // new message
    // startindex 1 ==> messages 1, 2, 3, 4, 5, 6, 7, 0
    // new message
    // startindex 2 ==> messages 2, 3, 4, 5, 6, 7, 0, 1
    // startindex 5 ==> messages 5, 6, 7, 0, 1, 2, 3, 4
    // 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }
    
    public void LoadNewMessage()
    {
        // new HugeNumber(50, 1);
        HugeNumber total =  GameManager.Instance.CurrentTotal; 

        ChatMessage randomChatMessage = chatMessageLibrary.GetRandom();

        if (total.IsBiggerThan(randomChatMessage.threshold))
        {
            chatMessages[messageIndex] = randomChatMessage;
            UpdateMessageIndex();
        } // else what happens? different message or no message?
        
        // TODO: donation
    }
    
    
    private void UpdateMessageIndex()
    {
        messageIndex = messageIndex++ % maxMessagesOnScreen;
    }
}