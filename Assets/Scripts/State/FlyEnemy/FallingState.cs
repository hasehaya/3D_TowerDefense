using UnityEngine;

public class FallingState :IFlyEnemyState
{
    private FlyEnemy enemy;

    public FallingState(FlyEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        enemy.ShootDownPosition = enemy.transform.position;
        enemy.rb.isKinematic = false;
        enemy.rb.useGravity = true;
        enemy.rb.velocity = Vector3.zero;

        FlyEnemy.OnEnemyShootDown.Invoke(enemy);

        enemy.anim.SetBool("isShootDown", true);
    }

    public void UpdateState()
    {
        if (enemy.IsGrounded())
        {
            enemy.TransitionToState(new GroundState(enemy));
        }
    }

    public void ExitState()
    {
        enemy.rb.useGravity = false;
        enemy.rb.isKinematic = true;
        enemy.rb.velocity = Vector3.zero;
        enemy.anim.SetBool("isShootDown", false);
    }
}
