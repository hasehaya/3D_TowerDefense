using System;

using UnityEngine;

public class FlyEnemy :Enemy
{
    public static Action<FlyEnemy> OnEnemyShootDown;

    // 状態管理
    private IFlyEnemyState currentState;

    // 撃墜関係の変数
    public bool IsFlying => currentState is FlyState || currentState is NearBaseState;
    [SerializeField] private float defaultShootDownHp = 100f;
    private float shootDownHp;
    [SerializeField] private float shootDownTime = 5f;
    public float ShootDownTime => shootDownTime;
    private Vector3 shootDownPosition;
    public Vector3 ShootDownPosition { get => shootDownPosition; set => shootDownPosition = value; }

    protected override void Start()
    {
        base.Start();
        shootDownHp = defaultShootDownHp;
    }

    protected override void StartState()
    {
        TransitionToState(new FlyState(this));
    }

    protected override void UpdateState()
    {
        currentState.UpdateState();
    }

    public void TransitionToState(IFlyEnemyState newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    public void TakeDamageFromShootDown(float damage, float shootDownDamage)
    {
        TakeDamage(damage);
        if (!IsFlying)
        {
            return;
        }
        shootDownHp -= shootDownDamage;
        if (shootDownHp <= 0)
        {
            TransitionToState(new FallingState(this));
        }
    }

    public void ResetShootDownHp()
    {
        shootDownHp = defaultShootDownHp;
    }
}
