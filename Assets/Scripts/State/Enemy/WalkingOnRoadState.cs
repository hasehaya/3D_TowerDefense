public class WalkingOnRoadState :IEnemyState
{
    private Enemy enemy;

    public WalkingOnRoadState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        enemy.nav.areaMask = 1 << enemy.roadArea; // 「Road」のみを歩く
        enemy.nav.SetDestination(StageManager.Instance.GetPlayerBasePosition());
    }

    public void UpdateState()
    {
        // 通常時の処理（必要なら追加）
    }

    public void ExitState()
    {
        // 状態から抜ける際の処理（必要なら追加）
    }
}
