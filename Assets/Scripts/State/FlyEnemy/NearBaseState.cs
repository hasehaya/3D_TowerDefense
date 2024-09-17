using UnityEngine;

public class NearBaseState :IFlyEnemyState
{
    private FlyEnemy enemy;
    private Vector3 basePosition;

    public NearBaseState(FlyEnemy enemy)
    {
        this.enemy = enemy;
        basePosition = StageManager.Instance.GetPlayerBasePosition();
    }

    public void EnterState()
    {
        // 初期化処理（必要なら追加）
    }

    public void UpdateState()
    {
        Vector3 direction = (basePosition - enemy.transform.position).normalized;
        enemy.transform.position += direction * enemy.Speed * Time.deltaTime;
    }

    public void ExitState()
    {
        // 終了処理（必要なら追加）
    }
}
