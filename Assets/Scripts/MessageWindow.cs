using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MessageWindow :MonoBehaviour
{
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

    void Start()
    {
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
        messageWindow.SetActive(false);
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
}
