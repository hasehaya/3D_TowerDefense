using UnityEngine;

public class Bullet :MonoBehaviour
{
    protected Enemy enemy;
    Rigidbody rb;
    MeshRenderer mr;
    protected float damage;
    float speed;
    private bool isPaused = false;

    const int MAX_TURN_ANGLE = 50;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
    }

    public void Initialize(FacilityAttack facilityAttack, Enemy enemy)
    {
        damage = facilityAttack.GetAttackPower();
        speed = facilityAttack.GetAttackSpeed();
        mr.material = facilityAttack.GetMaterial();
        this.enemy = enemy;
        if (enemy == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 direction = (enemy.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        if (!other.gameObject.CompareTag("Enemy"))
        {
            return;
        }
        var hitEnemy = other.gameObject.GetComponent<Enemy>();
        if (hitEnemy != enemy)
            return;
        Attack();
        Destroy(gameObject);
    }

    protected virtual void Attack()
    {
        enemy.TakeDamage(damage);
    }

    void Update()
    {
        if (isPaused)
            return;
        if (enemy == null)
            return;
        Vector3 direction = (enemy.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, MAX_TURN_ANGLE * Time.deltaTime);

        rb.velocity = transform.forward * speed;
    }

    // 一時停止時に保存する変数
    private Vector3 savedRbVelocity;
    private bool wasRbKinematic;

    // IPauseableの実装
    public void Pause()
    {
        if (isPaused)
            return;
        isPaused = true;
        // Rigidbodyの状態を保存
        savedRbVelocity = rb.velocity;
        wasRbKinematic = rb.isKinematic;

        // Rigidbodyをキネマティックに設定し、物理演算を停止
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
    }

    public void Resume()
    {
        if (!isPaused)
            return;
        isPaused = false;
        // Rigidbodyの状態を復元
        rb.isKinematic = wasRbKinematic;
        rb.velocity = savedRbVelocity;
    }
}
