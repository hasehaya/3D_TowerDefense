using System;

using UnityEngine;

public class Damageable :MonoBehaviour
{
    // ダメージを受けるオブジェクトが破壊されたときに呼ばれるイベント
    public static Action<Damageable> OnDestroyDamageableObject;

    HPBar hpBar;
    public float CurrentHp { get; private set; }
    public float MaxHp { get; private set; }
    GameObject hpBarInstance;
    public void Initialize(float maxHp)
    {
        CurrentHp = maxHp;
        MaxHp = maxHp;
        GenerateHpBar();
    }

    void GenerateHpBar()
    {
        GameObject hpBarPrefab = Resources.Load<GameObject>("Prefabs/HPBar");
        if (hpBarPrefab == null)
        {
            Debug.LogError($"HPバーのプレハブが見つかりません");
            return;
        }
        hpBarInstance = Instantiate(hpBarPrefab, transform);
        Vector3 position = hpBarInstance.transform.position;
        float height = GetComponent<Collider>().bounds.size.y;
        position.y += height + 1;
        hpBarInstance.transform.position = position;
        hpBar = hpBarInstance.GetComponentInChildren<HPBar>();
        hpBar.SetMaxHp(MaxHp);
    }

    public void TakeDamage(float damage)
    {
        if (CurrentHp <= 0)
        {
            return;
        }

        CurrentHp -= damage;
        if (CurrentHp > MaxHp)
        {
            CurrentHp = MaxHp;
        }
        hpBar.SetHp(CurrentHp);

        ShowDamageCount(damage);

        if (CurrentHp <= 0)
        {
            OnDestroyDamageableObject?.Invoke(this);
        }
    }

    void ShowDamageCount(float damage)
    {
        DamageText damageText = ObjectPool<DamageText>.Instance.GetObject();
        damageText.transform.position = transform.position + Vector3.up * 1f;
        damageText.SetDamage(damage);
    }

    public void SetHpBarPosition(Vector3 pos)
    {
        pos.y += 1;
        hpBarInstance.transform.position = pos;
    }

    public void HideHpBar()
    {
        hpBarInstance.SetActive(false);
    }
}
