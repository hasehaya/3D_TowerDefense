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
        Install = 4,
        CancelInstall = 5,
        Warp = 6,
        Purchase = 7,
        PurchaseCancel = 8,
    }
    // 現在表示中のNotice
    List<NoticeType> currentNotices = new List<NoticeType>();
    // イベントの設定
    UnityEvent synthesizeEvent = new UnityEvent();
    UnityEvent climbEvent = new UnityEvent();
    UnityEvent descendEvent = new UnityEvent();
    UnityEvent installEvent = new UnityEvent();
    UnityEvent cancelInstallEvent = new UnityEvent();
    UnityEvent warpEvent = new UnityEvent();
    UnityEvent purchase = new UnityEvent();
    UnityEvent purchaseCancel = new UnityEvent();

    // Typeから呼び出せるよう紐づけ
    Dictionary<NoticeType, UnityEvent> noticeEvents = new Dictionary<NoticeType, UnityEvent>();
    // キーの設定
    Dictionary<NoticeType, KeyCode> noticeKey = new Dictionary<NoticeType, KeyCode>();

    private void Awake()
    {
        noticeEvents.Add(NoticeType.Synthesize, synthesizeEvent);
        noticeEvents.Add(NoticeType.Climb, climbEvent);
        noticeEvents.Add(NoticeType.Descend, descendEvent);
        noticeEvents.Add(NoticeType.Install, installEvent);
        noticeEvents.Add(NoticeType.CancelInstall, cancelInstallEvent);
        noticeEvents.Add(NoticeType.Warp, warpEvent);
        noticeEvents.Add(NoticeType.Purchase, purchase);
        noticeEvents.Add(NoticeType.PurchaseCancel, purchaseCancel);
        noticeKey.Add(NoticeType.Synthesize, KeyCode.Z);
        noticeKey.Add(NoticeType.Climb, KeyCode.Q);
        noticeKey.Add(NoticeType.Descend, KeyCode.Tab);
        noticeKey.Add(NoticeType.Install, KeyCode.E);
        noticeKey.Add(NoticeType.CancelInstall, KeyCode.R);
        noticeKey.Add(NoticeType.Warp, KeyCode.F);
        noticeKey.Add(NoticeType.Purchase, KeyCode.V);
        noticeKey.Add(NoticeType.PurchaseCancel, KeyCode.X);
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
            case NoticeType.Install:
            return "設置";
            case NoticeType.CancelInstall:
            return "キャンセル";
            case NoticeType.Warp:
            return "ワープ";
            case NoticeType.Purchase:
            return "購入";
            case NoticeType.PurchaseCancel:
            return "購入キャンセル";
            default:
            return "";
        }
    }

    /// <summary>
    /// イベントを登録するための関数
    /// </summary>
    /// <param name="noticeType"></param>
    /// <param name="action"></param>
    void SetEvent(NoticeType noticeType, UnityAction action)
    {
        noticeEvents[noticeType].RemoveAllListeners();
        noticeEvents[noticeType].AddListener(action);
    }

    /// <summary>
    /// 一つのNoticeを表示するための関数
    /// </summary>
    /// <param name="noticeType"></param>
    public void ShowNotice(NoticeType noticeType, UnityAction action)
    {
        //すでに表示中のNoticeは表示しない、毎回明示的に消す処理が必要
        if (currentNotices.Contains(noticeType))
        {
            return;
        }
        currentNotices.Add(noticeType);
        // Noticeの生成
        var notice = Instantiate(noticePrefab, noticeParent.transform);
        var text = notice.GetComponentInChildren<Text>();
        text.text = GetNoticeText(noticeType) + ":" + noticeKey[noticeType];
        // イベントの登録
        SetEvent(noticeType, action);
    }

    public void HideNotice(NoticeType noticeType)
    {
        if (!currentNotices.Contains(noticeType))
            return;
        currentNotices.Remove(noticeType);
        var text = GetNoticeText(noticeType) + ":" + noticeKey[noticeType];
        foreach (Transform child in noticeParent.transform)
        {
            if (child.GetComponentInChildren<Text>().text.Contains(text))
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void HideAllNotice()
    {
        currentNotices.Clear();
        foreach (Transform child in noticeParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
