using Asset.Outline;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Facility :MonoBehaviour
{
    // 合成された際に呼ばれるイベント
    public delegate void FacilitySynthesized(Facility facility);
    public static event FacilitySynthesized OnFaicilitySynthesized;
    // 設置された際に呼ばれるイベント
    public delegate void FacilityInstalled(Facility facility);
    public static event FacilityInstalled OnFacilityInstalled;
    public enum Type
    {
        Canon = 0,
        Magic = 1,
        Tower = 2,
        Mine = 3,
        Freeze = 4,
        Shield = 5,
    }

    public enum Category
    {
        Attack = 0,
        Weather = 1,
        Mine = 2,
    }

    public enum InstallType
    {
        Side = 0,
        Road = 1,
    }

    public Type type;
    public FacilityParameter FacilityParameter { get; private set; }
    public int Level { get; private set; }


    public bool isInstalled = false;
    public bool canInstall = false;
    public bool isSelected = false;


    //メッシュ
    Dictionary<MeshRenderer, Color> mrAndColors = new Dictionary<MeshRenderer, Color>();
    //設置用のコライダー
    FacilityInstallCollider faciltyInstallCol;
    //輪郭
    protected Outline outline;
    //設置時の判定用に取得
    List<Collider> childrenCols = new List<Collider>();
    Crystal attachedCrystal;
    protected List<NoticeManager.NoticeType> noticeTypes = new List<NoticeManager.NoticeType>();


    protected virtual void Start()
    {
        var mrs = GetComponentsInChildren<MeshRenderer>();
        foreach (var mr in mrs)
        {
            mrAndColors.Add(mr, mr.material.color);
        }
        faciltyInstallCol = GetComponentInChildren<FacilityInstallCollider>();

        outline = GetComponentInChildren<Outline>();
        outline.enabled = false;

        childrenCols = GetComponentsInChildren<Collider>().ToList();
        faciltyInstallCol.SetChildrenCols(childrenCols);

        AddNoticeTypes();
    }

    protected virtual void Update()
    {
        InstallingFacility();
    }

    /// <summary>
    /// FacilityのGameObjectをParameterを付けて返す関数
    /// </summary>
    /// <returns>GameObject</returns>
    public static GameObject GenerateFacility(Type type)
    {
        var parameter = FacilityManager.Instance.GetFacilityParameter(type);
        var facilityObj = Instantiate(parameter.prefab);
        var facility = facilityObj.GetComponent<Facility>();
        facility.FacilityParameter = parameter;
        return facilityObj;
    }

    void InstallingFacility()
    {
        if (isInstalled)
            return;
        var groundPos = Reticle.Instance.GetTansform();
        if (groundPos != default)
        {
            transform.position = groundPos;
            NoticeManager.Instance.ShowFuncNotice(NoticeManager.NoticeType.Install, InstallFacility);
        }
    }

    public void InstallFacility()
    {
        if (!canInstall)
            return;
        if (isInstalled)
            return;
        isInstalled = true;
        foreach (var mr in mrAndColors)
        {
            mr.Key.material.color = mr.Value;
        }
        faciltyInstallCol.InstallFacility();
        NoticeManager.Instance.HideNotice(NoticeManager.NoticeType.PurchaseCancel);
        NoticeManager.Instance.ShowNotice(NoticeManager.NoticeType.OpenFacilityPurchase);
        OnFacilityInstalled?.Invoke(this);
    }

    /// <summary>
    /// それぞれの施設のNoticeを追加する関数
    /// </summary>
    protected virtual void AddNoticeTypes()
    {
        noticeTypes.Add(NoticeManager.NoticeType.Install);
        noticeTypes.Add(NoticeManager.NoticeType.Synthesize);
    }

    public void HideNotice()
    {
        foreach (var type in noticeTypes)
        {
            NoticeManager.Instance.HideNotice(type);
        }
    }

    public virtual void Synthesize(Crystal crystal)
    {
        CrystalBox.Instance.SynthesizeCrystal(crystal);
        OnFaicilitySynthesized?.Invoke(this);
    }
    public void ChangeColorRed()
    {
        foreach (var mr in mrAndColors)
        {
            mr.Key.material.color = new Color(1.0f, mr.Value.g / 3, mr.Value.b / 3, 0.9f);
        }
    }

    public void ChangeColorGreen()
    {
        foreach (var mr in mrAndColors)
        {
            mr.Key.material.color = new Color(mr.Value.r / 3, 1.0f, mr.Value.b / 3, 0.9f);
        }
    }

    /// <summary>
    /// 対象として選択、非選択されたときの関数
    /// </summary>
    /// <param name="isSelected"></param>
    public virtual void HandleSelection(bool isSelected)
    {
        if (!isSelected)
        {
            HideNotice();
        }
        outline.enabled = isSelected;
    }

    public GameObject GetPrefab()
    {
        return FacilityParameter.prefab;
    }

    public InstallType GetInstallType()
    {
        return FacilityParameter.installType;
    }
}

[System.Serializable]
public class FacilityParameter
{
    public Facility.Type type;
    public Facility.Category category;
    public Facility.InstallType installType;
    public GameObject prefab;
    public string name;
    public Sprite icon;
    public int price;
    public bool canAttachCrystal;

    public FacilityParameter()
    {
        type = Facility.Type.Canon;
        category = Facility.Category.Attack;
        installType = Facility.InstallType.Side;
        prefab = null;
        name = "";
        icon = null;
        price = 0;
        canAttachCrystal = true;
    }
}
