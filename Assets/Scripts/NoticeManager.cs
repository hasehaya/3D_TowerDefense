using System.Collections.Generic;
using System.Linq;

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
    // 自動で削除しないNotice（明示的に削除する必要があるNotice）
    NoticeType[] notNeedAutoDeleteNotices = null;
    // イベントの設定
    UnityEvent<object> synthesizeEvent = new UnityEvent<object>();
    UnityEvent<object> purchase = new UnityEvent<object>();
    UnityEvent climbEvent = new UnityEvent();
    UnityEvent descendEvent = new UnityEvent();
    UnityEvent installEvent = new UnityEvent();
    UnityEvent cancelInstallEvent = new UnityEvent();
    UnityEvent warpEvent = new UnityEvent();
    UnityEvent purchaseCancel = new UnityEvent();
    // Typeから呼び出せるよう紐づけ
    Dictionary<NoticeType, UnityEvent> noticeEvents = new Dictionary<NoticeType, UnityEvent>();
    Dictionary<NoticeType, UnityEvent<object>> noticeArgEvents = new Dictionary<NoticeType, UnityEvent<object>>();
    Dictionary<NoticeType, object> noticeArgments = new Dictionary<NoticeType, object>();
    // キーの設定
    Dictionary<NoticeType, KeyCode> noticeKey = new Dictionary<NoticeType, KeyCode>();

    private void Awake()
    {
        // 引数ありのイベントを登録
        noticeArgEvents.Add(NoticeType.Synthesize, synthesizeEvent);
        noticeArgEvents.Add(NoticeType.Purchase, purchase);
        // 引数なしのイベントを登録
        noticeEvents.Add(NoticeType.Climb, climbEvent);
        noticeEvents.Add(NoticeType.Descend, descendEvent);
        noticeEvents.Add(NoticeType.Install, installEvent);
        noticeEvents.Add(NoticeType.CancelInstall, cancelInstallEvent);
        noticeEvents.Add(NoticeType.Warp, warpEvent);
        noticeEvents.Add(NoticeType.PurchaseCancel, purchaseCancel);
        // 実行の際自動で削除しないNoticeを登録（明示的に削除する必要があるNoticeをここに記入）
        notNeedAutoDeleteNotices = new NoticeType[]
        {

        };
        // キーの登録
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
        ExcuteNotice();
    }

    void ExcuteNotice()
    {
        if (currentNotices.Count == 0)
            return;
        // foreach中に要素を削除するとエラーになるため一時的なリストを作成
        var tempNotices = new List<NoticeType>(currentNotices);
        var tempEvents = new Dictionary<NoticeType, UnityEvent>(noticeEvents);
        var tempArgEvents = new Dictionary<NoticeType, UnityEvent<object>>(noticeArgEvents);
        var tempArgments = new Dictionary<NoticeType, object>(noticeArgments);
        foreach (var noticeType in tempNotices)
        {
            if (!Input.GetKeyDown(noticeKey[noticeType]))
            {
                continue;
            }
            // 引数なしのイベントを実行
            if (tempEvents.ContainsKey(noticeType))
            {
                tempEvents[noticeType]?.Invoke();
                // 通知を自動で削除
                if (!notNeedAutoDeleteNotices.Contains(noticeType))
                {
                    HideNotice(noticeType);
                }
            }
            // 引数ありのイベントを実行
            else if (tempArgEvents.ContainsKey(noticeType))
            {
                // 引数の配列に引数がない場合は実行しない
                if (!tempArgEvents.ContainsKey(noticeType))
                {
                    continue;
                }
                tempArgEvents[noticeType]?.Invoke(tempArgments[noticeType]);
                // 通知を自動で削除
                if (!notNeedAutoDeleteNotices.Contains(noticeType))
                {
                    HideNotice(noticeType);
                }
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
    /// Noticeを表示するための関数
    /// </summary>
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

    // イベントを登録するための関数
    void SetEvent(NoticeType noticeType, UnityAction action)
    {
        noticeEvents[noticeType].RemoveAllListeners();
        noticeEvents[noticeType].AddListener(action);
    }

    /// <summary>
    /// 引数ありのNoticeを表示するための関数
    /// </summary>
    public void ShowArgNotice<T>(NoticeType noticeType, UnityAction<T> action, T arg)
    {
        if (currentNotices.Contains(noticeType))
        {
            return;
        }
        currentNotices.Add(noticeType);
        var notice = Instantiate(noticePrefab, noticeParent.transform);
        var text = notice.GetComponentInChildren<Text>();
        text.text = GetNoticeText(noticeType) + ":" + noticeKey[noticeType];
        SetArgEvent(noticeType, action, arg);
    }

    // 引数ありのイベントを登録するための関数
    void SetArgEvent<T>(NoticeType noticeType, UnityAction<T> action, T arg)
    {
        noticeArgments[noticeType] = arg;
        noticeArgEvents[noticeType].RemoveAllListeners();
        noticeArgEvents[noticeType].AddListener(new UnityAction<object>(obj => action((T)obj)));
    }

    public void HideNotice(NoticeType noticeType)
    {
        if (!currentNotices.Contains(noticeType))
        {
            return;
        }
        if (noticeArgments.ContainsKey(noticeType))
        {
            noticeArgments.Remove(noticeType);
        }
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
