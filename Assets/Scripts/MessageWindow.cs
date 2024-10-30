using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MessageWindowManager :MonoBehaviour
{
    private static MessageWindowManager instance;
    public static MessageWindowManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MessageWindowManager>();
            }
            return instance;
        }
    }

    [SerializeField] GameObject messageWindow;
    [SerializeField] Text messageText;

    [SerializeField] float charInterval = 0.05f;
    private Coroutine writingCoroutine;
    private bool isWriting = false;
    private string fullMessage = string.Empty;

    private List<char> muteChars = new List<char>()
    {
        ' ', '　', ',', '、', '。', '「', '」', '!', '?', '.', ':', ';'
    };

    [SerializeField] MessageData[] messageList;

    void Start()
    {
        messageList = ScriptableObjectManager.Instance.GetMessageDataListEntity().lists;
        messageText.text = "";
        messageWindow.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        messageWindow.SetActive(true);
        if (isWriting)
        {
            StopCoroutine(writingCoroutine);
            isWriting = false;
        }

        fullMessage = message;
        messageText.text = "";
        writingCoroutine = StartCoroutine(TypeWriteMessage(fullMessage));
    }

    private IEnumerator TypeWriteMessage(string message)
    {
        isWriting = true;
        foreach (char c in message)
        {
            messageText.text += c;
            if (!muteChars.Contains(c))
            {
                SE.Play("TypeWriteMessage");
            }
            yield return new WaitForSeconds(charInterval);
        }
        isWriting = false;
    }

    public void SkipWriting()
    {
        if (isWriting)
        {
            StopCoroutine(writingCoroutine);
            messageText.text = fullMessage;
            isWriting = false;
            messageWindow.SetActive(false);
        }
    }

    public void ClearMessage()
    {
        if (isWriting)
        {
            StopCoroutine(writingCoroutine);
            isWriting = false;
        }
        messageText.text = "";
        messageWindow.SetActive(false);
    }

    // Function to display messages based on MessageId
    public void ShowMessagesWithId(int messageId)
    {
        StartCoroutine(ShowMessagesCoroutine(messageId));
    }

    private IEnumerator ShowMessagesCoroutine(int messageId)
    {
        // Find all messages with the given MessageId and sort them by InternalNo
        List<MessageData> targetMessages = new List<MessageData>();
        foreach (MessageData m in messageList)
        {
            if (m.messageId == messageId)
            {
                targetMessages.Add(m);
            }
        }
        targetMessages.Sort((a, b) => a.internalNo.CompareTo(b.internalNo));

        messageWindow.SetActive(true);

        // Display each message sequentially
        foreach (MessageData messageData in targetMessages)
        {
            yield return StartCoroutine(TypeWriteMessage(messageData.text));
            yield return new WaitForSeconds(1f); // Optional delay between messages
        }

        // Close the MessageWindow after displaying all messages
        messageWindow.SetActive(false);
    }
}

[System.Serializable]
public class MessageData
{
    public int messageId;
    public int internalNo;
    public string text;

    public MessageData()
    {
        messageId = 0;
        internalNo = 0;
        text = "";
    }
}