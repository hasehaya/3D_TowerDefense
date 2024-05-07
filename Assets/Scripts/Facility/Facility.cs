using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Facility :MonoBehaviour
{
    public bool isInstalled = true;
    public bool isTouchingOtherObj = true;
    public bool isInRange = false;
    public bool isSelected = false;

    //クリスタルアタッチ用
    public enum Category
    {
        Attack,
        Weather,
    }
    public Category category;

    [SerializeField] MeshRenderer mr;
    //元の色を保持
    Color originColor;
    //設置用のコライダー
    FacilityInstallCollider faciltyInstallCol;
    //輪郭
    protected Outline outline;
    //設置時の判定用に取得
    List<Collider> childrenCols = new List<Collider>();
    Crystal attachedCrystal;
    protected List<NoticeManager.NoticeType> noticeTypes = new List<NoticeManager.NoticeType>();

    protected void Start()
    {
        originColor = mr.material.color;
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

    void InstallingFacility()
    {
        if (isInstalled)
            return;
        var groundPos = Reticle.Instance.GetTansform();
        if (groundPos != default)
        {
            transform.position = groundPos;
            NoticeManager.Instance.ShowNotice(NoticeManager.NoticeType.Install, InstallFacility);
        }
    }

    public void InstallFacility()
    {
        if (isTouchingOtherObj)
            return;
        if (isInstalled)
            return;
        isInstalled = true;
        mr.material.color = originColor;
        faciltyInstallCol.InstallFacility();
        NoticeManager.Instance.HideNotice(NoticeManager.NoticeType.PurchaseCancel);
        NoticeManager.Instance.ShowNotice(NoticeManager.NoticeType.Purchase, FacilityManager.Instance.CreateFacility);
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
    }
    public void ChangeColorRed()
    {
        mr.material.color = new Color(1.0f, originColor.g / 3, originColor.b / 3, 0.9f);
    }

    public void ChangeColorGreen()
    {
        mr.material.color = new Color(originColor.r / 3, 1.0f, originColor.b / 3, 0.9f);
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
}
