using UnityEngine;

public class Bullet :MonoBehaviour
{
    protected Enemy enemy;
    Rigidbody rb;
    MeshRenderer mr;
    protected float damage;
    float speed;

    const int maxTurnAngle = 50;

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
        if (enemy == null)
            return;
        Vector3 direction = (enemy.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxTurnAngle * Time.deltaTime);

        rb.velocity = transform.forward * speed;
    }
}
