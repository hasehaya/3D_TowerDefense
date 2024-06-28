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
        OpenFacilityPurchase = 7,
        PurchaseCancel = 8,
        NextWave = 9,
        CutTree = 10,
    }
    // 現在表示中のNotice
    List<NoticeType> currentNotices = new List<NoticeType>();
    // 自動で削除しないNotice（明示的に削除する必要があるNotice）
    NoticeType[] notNeedAutoDeleteNotices = null;
    // 関数が決まっているNoticeのイベント
    NoticeType[] fixedNotices = null;
    // イベントの設定
    UnityEvent<object> synthesizeEvent = new UnityEvent<object>();
    UnityEvent climbEvent = new UnityEvent();
    UnityEvent descendEvent = new UnityEvent();
    UnityEvent installEvent = new UnityEvent();
    UnityEvent warpEvent = new UnityEvent();
    UnityEvent openFacilityPurchase = new UnityEvent();
    UnityEvent purchaseCancel = new UnityEvent();
    UnityEvent nextWaveEvent = new UnityEvent();
    UnityEvent cutTree = new UnityEvent();
    // Typeから呼び出せるよう紐づけ
    Dictionary<NoticeType, UnityEvent> noticeEvents = new Dictionary<NoticeType, UnityEvent>();
    Dictionary<NoticeType, UnityEvent<object>> noticeArgEvents = new Dictionary<NoticeType, UnityEvent<object>>();
    Dictionary<NoticeType, object> noticeArgments = new Dictionary<NoticeType, object>();
    // キーの設定
    Dictionary<NoticeType, KeyCode> noticeKey = new Dictionary<NoticeType, KeyCode>();
    // テキストの設定
    Dictionary<NoticeType, string> noticeText = new Dictionary<NoticeType, string>();
    // キー入力のフラグ
    private Dictionary<NoticeType, bool> noticeInputFlags = new Dictionary<NoticeType, bool>();

    private void Awake()
    {
        // 引数ありのイベントを登録
        noticeArgEvents.Add(NoticeType.Synthesize, synthesizeEvent);
        // 引数なしのイベントを登録
        noticeEvents.Add(NoticeType.Climb, climbEvent);
        noticeEvents.Add(NoticeType.Descend, descendEvent);
        noticeEvents.Add(NoticeType.Install, installEvent);
        noticeEvents.Add(NoticeType.Warp, warpEvent);
        noticeEvents.Add(NoticeType.OpenFacilityPurchase, openFacilityPurchase);
        noticeEvents.Add(NoticeType.PurchaseCancel, purchaseCancel);
        noticeEvents.Add(NoticeType.NextWave, nextWaveEvent);
        noticeEvents.Add(NoticeType.CutTree, cutTree);
        // 実行の際自動で削除しないNoticeを登録（明示的に削除する必要があるNoticeをここに記入）
        notNeedAutoDeleteNotices = new NoticeType[]
        {

        };
        // 関数が決まっているNoticeを登録
        fixedNotices = new NoticeType[]
        {
            NoticeType.OpenFacilityPurchase,
            NoticeType.NextWave,
        };
        // 関数が決まっている関数を登録
        openFacilityPurchase.AddListener(() => UIManager.Instance.facilityPurchasePresenter.OpenFacilityPurchase());
        nextWaveEvent.AddListener(() => WaveManager.Instance.NextWave());
        // キーの登録
        noticeKey.Add(NoticeType.Synthesize, KeyCode.Z);
        noticeKey.Add(NoticeType.Climb, KeyCode.Q);
        noticeKey.Add(NoticeType.Descend, KeyCode.Tab);
        noticeKey.Add(NoticeType.Install, KeyCode.E);
        noticeKey.Add(NoticeType.Warp, KeyCode.F);
        noticeKey.Add(NoticeType.OpenFacilityPurchase, KeyCode.V);
        noticeKey.Add(NoticeType.PurchaseCancel, KeyCode.X);
        noticeKey.Add(NoticeType.NextWave, KeyCode.H);
        noticeKey.Add(NoticeType.CutTree, KeyCode.G);
        // テキストの登録
        noticeText.Add(NoticeType.Synthesize, "水晶合成");
        noticeText.Add(NoticeType.Climb, "登る");
        noticeText.Add(NoticeType.Descend, "降りる");
        noticeText.Add(NoticeType.Install, "設置");
        noticeText.Add(NoticeType.Warp, "ワープ");
        noticeText.Add(NoticeType.OpenFacilityPurchase, "建物購入");
        noticeText.Add(NoticeType.PurchaseCancel, "購入キャンセル");
        noticeText.Add(NoticeType.NextWave, "次のWave");
        noticeText.Add(NoticeType.CutTree, "木を切る");
        // キー入力のフラグを初期化
        noticeInputFlags = noticeKey.ToDictionary(x => x.Key, x => false);
    }

    private void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        foreach (var noticeType in currentNotices)
        {
            if (Input.GetKeyDown(noticeKey[noticeType]))
            {
                noticeInputFlags[noticeType] = true;
            }
        }
    }

    private void FixedUpdate()
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
            if (!noticeInputFlags[noticeType])
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

    /// <summary>
    /// 関数固定のNoticeを表示するための関数
    /// </summary>
    public void ShowNotice(NoticeType noticeType)
    {
        if (currentNotices.Contains(noticeType))
        {
            return;
        }
        // 関数を必要とするNoticeは表示しない
        if (!fixedNotices.Contains(noticeType))
        {
            return;
        }
        currentNotices.Add(noticeType);
        var notice = Instantiate(noticePrefab, noticeParent.transform);
        var text = notice.GetComponentInChildren<Text>();
        text.text = noticeText[noticeType] + ":" + noticeKey[noticeType];
    }

    /// <summary>
    /// 関数指定のNoticeを表示するための関数
    /// </summary>
    public void ShowFuncNotice(NoticeType noticeType, UnityAction action)
    {
        //すでに表示中のNoticeは表示しない
        if (currentNotices.Contains(noticeType))
        {
            return;
        }
        // 関数が決まっているNoticeは表示しない
        if (fixedNotices.Contains(noticeType))
        {
            return;
        }
        currentNotices.Add(noticeType);
        // Noticeの生成
        var notice = Instantiate(noticePrefab, noticeParent.transform);
        var text = notice.GetComponentInChildren<Text>();
        text.text = noticeText[noticeType] + ":" + noticeKey[noticeType];
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
        // 関数が決まっているNoticeは表示しない
        if (fixedNotices.Contains(noticeType))
        {
            return;
        }
        currentNotices.Add(noticeType);
        var notice = Instantiate(noticePrefab, noticeParent.transform);
        var text = notice.GetComponentInChildren<Text>();
        text.text = noticeText[noticeType] + ":" + noticeKey[noticeType];
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
        noticeInputFlags[noticeType] = false;
        var text = noticeText[noticeType] + ":" + noticeKey[noticeType];
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
        var keys = noticeInputFlags.Keys.ToList();
        foreach (var key in keys)
        {
            noticeInputFlags[key] = false;
        }
        foreach (Transform child in noticeParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
