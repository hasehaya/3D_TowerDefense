using System.Collections.Generic;

using UnityEngine;

public class AreaDamage :MonoBehaviour
{
    // Parameters
    public float damage;
    public float range;
    public Form form;
    public int durationCount;
    public float durationTime;

    private EnemyDetector enemyDetector;
    private float timer = -0.1f;

    void Initialize(Vector3 pos, Form form, float damage, float range, int durationCount, float durationTime)
    {
        timer = -0.1f;
        transform.position = pos;
        this.form = form;
        this.damage = damage;
        this.range = range;
        this.durationCount = durationCount;
        this.durationTime = durationTime;
        enemyDetector = gameObject.GetComponent<EnemyDetector>();
        enemyDetector.Initialize(form, range);
    }

    public static void Create(Vector3 pos, Form form, float damage, float range, int durationCount, float durationTime)
    {
        var areaDamagePrefab = Resources.Load<GameObject>("Prefabs/AreaDamage");
        var areaDamageObj = Instantiate(areaDamagePrefab, Vector3.zero, Quaternion.identity);
        var areaDamage = areaDamageObj.GetComponent<AreaDamage>();
        areaDamage.Initialize(pos, form, damage, range, durationCount, durationTime);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= durationTime)
        {
            timer = 0;
            durationCount--;
            AddDamage();
            if (durationCount <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void AddDamage()
    {
        foreach (var enemy in enemyDetector.GetEnemies())
        {
            enemy.TakeDamage(damage);
        }
    }
}
