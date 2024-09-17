using UnityEngine;

public class GroundState :IFlyEnemyState
{
    private FlyEnemy enemy;
    private float shootDownCounter = 0;

    public GroundState(FlyEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        shootDownCounter = 1;
    }

    public void UpdateState()
    {
        shootDownCounter += Time.deltaTime;
        if (shootDownCounter > enemy.ShootDownTime)
        {
            enemy.TransitionToState(new FloatingState(enemy));
        }
    }

    public void ExitState()
    {
        // 終了処理（必要なら追加）
    }
}
