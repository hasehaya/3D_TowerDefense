using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NoticeManager :MonoBehaviour
{
    private static NoticeManager instance;
    public static NoticeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NoticeManager>();
            }
            return instance;
        }
    }
    // NoticeのPrefab
    [SerializeField] GameObject noticePrefab;
    // Noticeの親
    [SerializeField] GameObject noticeParent;
    // Noticeの種類
    public enum NoticeType
    {
        None = 0,
        Synthesize = 1,
        Climb = 2,
        Descend = 3,
        CancelInstall = 4,
    }
    // 現在表示中のNotice
    List<NoticeType> currentNotices = new List<NoticeType>();
    // イベントの設定
    [NonSerialized] public UnityEvent synthesizeEvent;
    [NonSerialized] public UnityEvent climbEvent;
    [NonSerialized] public UnityEvent descendEvent;
    [NonSerialized] public UnityEvent cancelInstallEvent;
    // Typeから呼び出せるよう紐づけ
    Dictionary<NoticeType, UnityEvent> noticeEvents = new Dictionary<NoticeType, UnityEvent>();
    // キーの設定
    Dictionary<NoticeType, string> noticeKey = new Dictionary<NoticeType, string>();

    private void Start()
    {
        noticeEvents.Add(NoticeType.Synthesize, synthesizeEvent);
        noticeEvents.Add(NoticeType.Climb, climbEvent);
        noticeEvents.Add(NoticeType.Descend, descendEvent);
        noticeEvents.Add(NoticeType.CancelInstall, cancelInstallEvent);
        noticeKey.Add(NoticeType.Synthesize, "Q");
        noticeKey.Add(NoticeType.Climb, "E");
        noticeKey.Add(NoticeType.Descend, "E");
        noticeKey.Add(NoticeType.CancelInstall, "1");
    }

    private void Update()
    {
        if (currentNotices.Count == 0)
            return;
        foreach (var noticeType in currentNotices)
        {
            if (Input.GetKeyDown(noticeKey[noticeType]))
            {
                // イベントがNullでなければ実行
                noticeEvents[noticeType]?.Invoke();
            }
        }
    }

    /// <summary>
    /// イベントを登録するための関数
    /// </summary>
    /// <param name="noticeType"></param>
    /// <param name="action"></param>
    public void SetEvent(NoticeType noticeType, UnityAction action)
    {
        noticeEvents[noticeType].RemoveAllListeners();
        noticeEvents[noticeType].AddListener(action);
    }

    string GetNoticeText(NoticeType noticeType)
    {
        switch (noticeType)
        {
            case NoticeType.Synthesize:
            return "水晶合成";
            case NoticeType.Climb:
            return "登る";
            case NoticeType.Descend:
            return "降りる";
            case NoticeType.CancelInstall:
            return "キャンセル";
            default:
            return "";
        }
    }

    /// <summary>
    /// 一つのNoticeを表示するための関数
    /// </summary>
    /// <param name="noticeType"></param>
    public void ShowNotice(NoticeType noticeType)
    {
        currentNotices.Clear();
        currentNotices.Add(noticeType);
        var notice = Instantiate(noticePrefab, noticeParent.transform);
        var text = notice.GetComponentInChildren<Text>();
        text.text = GetNoticeText(noticeType) + ":" + noticeKey[noticeType];
    }

    /// <summary>
    /// 複数のNoticeを表示するための関数
    /// </summary>
    /// <param name="noticeTypes"></param>
    public void ShowNotices(NoticeType[] noticeTypes = null)
    {
        currentNotices.Clear();
        foreach (var type in noticeTypes)
        {
            currentNotices.Add(type);
            var notice = Instantiate(noticePrefab, noticeParent.transform);
            var text = notice.GetComponentInChildren<Text>();
            text.text = GetNoticeText(type) + ":" + noticeKey[type];
        }
    }

    public void HideNotice()
    {
        currentNotices.Clear();
        foreach (Transform child in noticeParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
