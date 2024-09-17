using UnityEngine;
using UnityEngine.AI;

public class FlyState :IFlyEnemyState
{
    private FlyEnemy enemy;
    private Vector3 basePosition;
    private const float kBaseDistance = 20f;

    public FlyState(FlyEnemy enemy)
    {
        this.enemy = enemy;
        basePosition = StageManager.Instance.GetPlayerBasePosition();
        basePosition.y = enemy.transform.position.y;
    }

    public void EnterState()
    {
        // Rigidbodyの物理演算を無効化
        enemy.rb.isKinematic = true;
        enemy.rb.useGravity = false;

        // NavMeshAgentを有効化
        enemy.nav.enabled = true;
        enemy.nav.areaMask = 1 << enemy.RoadArea;
        enemy.nav.SetDestination(basePosition);
    }

    public void UpdateState()
    {
        // プレイヤーのベースとの距離を計算
        float distanceToBase = Vector3.Distance(enemy.transform.position, basePosition);

        // 一定距離以内に入ったらNearBaseStateに遷移
        if (distanceToBase <= kBaseDistance)
        {
            enemy.TransitionToState(new NearBaseState(enemy));
        }
    }

    public void ExitState()
    {
        enemy.nav.enabled = false;
    }
}
