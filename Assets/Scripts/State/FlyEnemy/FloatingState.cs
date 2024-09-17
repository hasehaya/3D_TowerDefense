using UnityEngine;

public class FloatingState :IFlyEnemyState
{
    private FlyEnemy enemy;

    public FloatingState(FlyEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        // 初期化処理（必要なら追加）
    }

    public void UpdateState()
    {
        Vector3 direction = (enemy.ShootDownPosition - enemy.transform.position).normalized;
        enemy.transform.position += direction * enemy.Speed * Time.deltaTime;
        if (Vector3.Distance(enemy.transform.position, enemy.ShootDownPosition) <= 0.1f)
        {
            enemy.ResetShootDownHp();
            enemy.TransitionToState(new FlyState(enemy));
        }
    }

    public void ExitState()
    {
        // 終了処理（必要なら追加）
    }
}
