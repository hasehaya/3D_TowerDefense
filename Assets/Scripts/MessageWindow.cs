﻿using System.Collections;
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
                instance.Initialize();
            }
            return instance;
        }
    }

    [SerializeField] GameObject messageWindow; // メッセージウィンドウ
    [SerializeField] Text messageText; // メッセージテキスト

    [SerializeField] float charInterval = 0.037f; // 文字表示間隔
    private Coroutine writingCoroutine;
    private bool isWriting = false;
    private string fullMessage = string.Empty;

    private List<char> muteChars = new List<char>()
    {
        ' ', '　', ',', '、', '。', '「', '」', '!', '?', '.', ':', ';'
    };

    MessageData[] messageArray;
    private bool isWaitingForNextMessage = false;

    void Initialize()
    {
        messageArray = ScriptableObjectManager.Instance.GetMessageDataArray();
        messageText.text = "";
        messageWindow.SetActive(false);
    }

    void Update()
    {
        if (!messageWindow.activeSelf)
        {
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isWaitingForNextMessage)
            {
                // 全文表示後にクリックされたら次のメッセージへ
                isWaitingForNextMessage = false;
            }
        }
    }

    // メッセージを表示
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

    // タイプライター風に文字を表示
    private IEnumerator TypeWriteMessage(string message)
    {
        isWriting = true;
        foreach (char c in message)
        {
            messageText.text += c;
            if (!muteChars.Contains(c))
            {
                //SE.Play("TypeWriteMessage");
            }
            yield return new WaitForSeconds(charInterval);
        }
        isWriting = false;
    }

    // メッセージをクリア
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

    // 指定のStageIdとMessageIdのメッセージを表示
    public void ShowMessagesWithId(int stageId, int messageId)
    {
        StartCoroutine(ShowMessagesCoroutine(stageId, messageId));
    }

    // メッセージ表示のコルーチン
    private IEnumerator ShowMessagesCoroutine(int stageId, int messageId)
    {
        StageManager.Instance.Pause();
        // 指定StageIdとMessageIdのメッセージを取得して内部番号順にソート
        List<MessageData> targetMessages = new List<MessageData>();
        foreach (MessageData m in messageArray)
        {
            if (m.stageId == stageId && m.messageId == messageId)
            {
                targetMessages.Add(m);
            }
        }
        targetMessages.Sort((a, b) => a.internalNo.CompareTo(b.internalNo));

        messageWindow.SetActive(true);

        // 各メッセージを順に表示
        foreach (MessageData messageData in targetMessages)
        {
            // メッセージ間でテキストをクリア
            messageText.text = "";

            // メッセージを表示
            yield return StartCoroutine(TypeWriteMessage(messageData.text));

            // クリックを待つ
            yield return StartCoroutine(WaitForClick());
        }

        // 全メッセージ表示後にウィンドウを閉じる
        messageWindow.SetActive(false);
        StageManager.Instance.Resume();
    }

    // クリックを待つコルーチン
    private IEnumerator WaitForClick()
    {
        isWaitingForNextMessage = true;
        while (isWaitingForNextMessage)
        {
            yield return null;
        }
        // 次のメッセージへ
    }
}

[System.Serializable]
public class MessageData
{
    public int stageId;
    public int messageId;
    public int internalNo;
    public string text;

    public MessageData()
    {
        stageId = 0;
        messageId = 0;
        internalNo = 0;
        text = "";
    }
}
