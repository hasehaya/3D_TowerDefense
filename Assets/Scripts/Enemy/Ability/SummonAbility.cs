using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

public class SummonAbility :IAbility
{
    Enemy.EnemyType summonedEnemyType;
    int summonedCount;
    GameObject gameObject;
    float distance = 1.0f;
    public float counter { get; set; }
    public float coolTime { get; set; }
    public SummonAbility(Enemy.EnemyType summonedEnemyType, int summonedCount, float coolTime, GameObject gameObject)
    {
        this.summonedEnemyType = summonedEnemyType;
        this.summonedCount = summonedCount;
        this.coolTime = coolTime;
        this.gameObject = gameObject;
    }
    public void Excute()
    {
        for (int i = 0; i < summonedCount; i++)
        {
            // 回転角度を計算
            float angle = 2 * Mathf.PI * i / summonedCount;
            // 前方向ベクトルを取得
            Vector3 forward = gameObject.transform.forward;
            // 回転行列を使って方向ベクトルを計算
            Vector3 direction = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
            // directionを前方向に基づいて回転
            direction = Quaternion.LookRotation(forward) * direction;
            // ポジションを計算
            Vector3 summonedPos = gameObject.transform.position + direction * distance + Vector3.up;
            EnemyManager.Instance.SpawnEnemy(summonedEnemyType, summonedPos);
        }
    }
}
