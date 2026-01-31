using System;

[Serializable]
public struct ChatMessage
{
    public string username;
    public string message;
    public HugeNumber threshold;
    public HugeNumber donation;


    public ChatMessage(string username, string message, HugeNumber threshold, HugeNumber donation = null)
    {
        this.username = username;
        this.message = message;
        this.threshold = threshold;
        this.donation = donation;
    }
}