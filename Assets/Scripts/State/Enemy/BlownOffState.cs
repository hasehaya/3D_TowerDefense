using UnityEngine;

public class BlownOffState :IEnemyState
{
    private Enemy enemy;
    private Vector3 blowedDirection;
    private float blowedTimeCounter = 0;
    private const float BLOWED_DURATION = 2;

    public BlownOffState(Enemy enemy, Vector3 blowedDirection)
    {
        this.enemy = enemy;
        this.blowedDirection = blowedDirection;
    }

    public void EnterState()
    {
        enemy.nav.enabled = false;
        enemy.rb.isKinematic = false;
        enemy.rb.useGravity = true;
        enemy.rb.velocity = Vector3.zero;
        enemy.rb.AddForce(blowedDirection, ForceMode.Impulse);
    }

    public void UpdateState()
    {
        blowedTimeCounter += Time.deltaTime;
        if (blowedTimeCounter < BLOWED_DURATION)
        {
            return;
        }
        if (enemy.IsGrounded())
        {
            enemy.TransitionToState(new ReturningToRoadState(enemy));
        }
    }

    public void ExitState()
    {
        enemy.rb.isKinematic = true;
        enemy.rb.useGravity = false;
        enemy.rb.velocity = Vector3.zero;
    }
}
