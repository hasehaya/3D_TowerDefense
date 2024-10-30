using UnityEngine;

public class FlyMultiplyEnemy :FlyEnemy
{
    [SerializeField] EnemyType multiplyEnemyType;
    [SerializeField] int count;
    const float kDistance = 1.0f;
    protected override void OnDestroy()
    {
        base.OnDestroy();
        Multiply();
    }

    void Multiply()
    {
        for (int i = 0; i < count; i++)
        {
            // 回転角度を計算
            float angle = 2 * Mathf.PI * i / count;
            // 前方向ベクトルを取得
            Vector3 forward = gameObject.transform.forward;
            // 回転行列を使って方向ベクトルを計算
            Vector3 direction = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
            // directionを前方向に基づいて回転
            direction = Quaternion.LookRotation(forward) * direction;
            // ポジションを計算
            Vector3 summonedPos = gameObject.transform.position + direction * kDistance + Vector3.up;
            EnemyManager.Instance.SpawnEnemy(multiplyEnemyType, summonedPos);
        }
    }
}