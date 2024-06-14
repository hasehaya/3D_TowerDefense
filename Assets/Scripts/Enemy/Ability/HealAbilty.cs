using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbilty : IAbility
{
    int healValue;
    float healRange;
    GameObject gameObject;
    public float counter { get; set; }
    public float coolTime { get; set; }
    public HealAbilty(int healValue, float healRange, float coolTime, GameObject gameObject)
    {
        this.healValue = healValue;
        this.healRange = healRange;
        this.gameObject = gameObject;
        this.coolTime = coolTime;
        
    }
    public void Excute()
    {
        List<Enemy> enemyList = EnemyManager.Instance.getEnemyList();
        foreach(Enemy enemy in enemyList)
        {
            // 対象となるEnemyとの距離を調べ、近くだったら何らかの処理をする
            float dist = Vector3.Distance(enemy.transform.position, this.gameObject.transform.position);
            if (dist < this.healRange)
            {
                enemy.TakeDamage(-healValue);
            }
        }

    }


    

}
