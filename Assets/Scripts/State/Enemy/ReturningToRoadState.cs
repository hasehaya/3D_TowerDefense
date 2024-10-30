// ReturningToRoadState.cs
using UnityEngine;
using UnityEngine.AI;

public class ReturningToRoadState :IEnemyState
{
    private Enemy enemy;

    // OffsetDistance を const として宣言
    private const float OFFSET_DISTANCE = 5f;
    private const int MAX_ATTEMPTS = 5; // 最大試行回数
    private const float MIN_OFFSET_DISTANCE = 0.01f; // 最小オフセット距離
    private const float ARRIVAL_THRESHOLD = 0.01f; // 目的地到達判定の閾値

    public ReturningToRoadState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        // 現在位置が Road エリアに含まれているか確認
        NavMeshHit currentHit;
        if (NavMesh.SamplePosition(enemy.transform.position, out currentHit, 0f, 1 << enemy.RoadArea))
        {
            // Road エリアにいるので WalkingOnRoadState に遷移
            enemy.TransitionToState(new WalkingOnRoadState(enemy));
            Debug.LogWarning($"{enemy.name} is already on Road area. Transitioning to WalkingOnRoadState.");
            return;
        }

        // NavMeshAgent を有効化し、Rigidbody を Kinematic に設定
        enemy.nav.enabled = true;
        enemy.rb.isKinematic = true;

        // 「Road」と「Ground」エリアを歩けるようにする
        enemy.nav.areaMask = (1 << enemy.RoadArea) | (1 << enemy.GroundArea);

        // 現在位置から最も近い Road エリアのポイントを取得
        NavMeshHit closestRoadHit;
        bool foundClosestRoad = NavMesh.SamplePosition(enemy.transform.position, out closestRoadHit, 100f, 1 << enemy.RoadArea);

        if (!foundClosestRoad)
        {
            // Road エリアが見つからない場合、プレイヤーのベースへ設定
            Vector3 basePosition = StageManager.Instance.GetPlayerBasePosition();
            enemy.nav.SetDestination(basePosition);
            Debug.LogWarning($"{enemy.name}: No Road area found near current position. Setting destination to base.");
            return;
        }

        Vector3 closestRoadPosition = closestRoadHit.position;
        Vector3 directionToRoad = (closestRoadPosition - enemy.transform.position).normalized;

        // オフセット距離の初期化
        float currentOffset = OFFSET_DISTANCE;
        int attempt = 0;
        bool destinationSet = false;

        while (attempt < MAX_ATTEMPTS && currentOffset >= MIN_OFFSET_DISTANCE)
        {
            // 敵の位置から currentOffset だけ進んだサンプル位置を計算
            Vector3 offsetPosition = closestRoadPosition + directionToRoad * currentOffset;

            // サンプル位置周辺で「Road」エリアの最も近いポイントを取得
            NavMeshHit hit;
            if (NavMesh.SamplePosition(offsetPosition, out hit, 20f, 1 << enemy.RoadArea))
            {
                // サンプル位置が「Road」エリアに含まれているかを確認
                if ((hit.mask & (1 << enemy.RoadArea)) != 0)
                {
                    enemy.nav.SetDestination(offsetPosition);
                    destinationSet = true;
                    Debug.LogWarning($"{enemy.name}: Destination set to Road area at {hit.position}");
                    break; // 有効な目的地が見つかったのでループを抜ける
                }
                else
                {
                    Debug.LogWarning($"{enemy.name}: Sampled position is not on Road area. Reducing offset.");
                }
            }
            else
            {
                Debug.LogWarning($"{enemy.name}: No Road area found near offset position. Reducing offset.");
            }

            // オフセット距離を短くする (例: 80% に減少)
            currentOffset *= 0.8f;
            attempt++;
            Debug.LogWarning($"{enemy.name}: Reducing offset distance to {currentOffset}");
        }

        if (!destinationSet)
        {
            // 最大試行回数までに有効な位置が見つからなかった場合はプレイヤーのベースへ設定
            Vector3 basePosition = StageManager.Instance.GetPlayerBasePosition();
            enemy.nav.SetDestination(basePosition);
            Debug.LogWarning($"{enemy.name}: Unable to find Road area within offset attempts. Setting destination to base.");
        }
    }

    public void UpdateState()
    {
        // エージェントが目的地に到達したか確認
        if (!enemy.nav.pathPending)
        {
            if (enemy.nav.remainingDistance <= enemy.nav.stoppingDistance + ARRIVAL_THRESHOLD)
            {
                if (!enemy.nav.hasPath || enemy.nav.velocity.sqrMagnitude == 0f)
                {
                    // 目的地に到達したので状態を遷移
                    enemy.TransitionToState(new WalkingOnRoadState(enemy));
                    Debug.LogWarning($"{enemy.name}: Arrived at destination. Transitioning to WalkingOnRoadState.");
                }
            }
        }
    }

    public void ExitState()
    {
        // 「Road」のみを歩くように設定
        enemy.nav.areaMask = 1 << enemy.RoadArea;
    }
}
