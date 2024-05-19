using UnityEngine;

public class Bullet :MonoBehaviour
{
    Enemy enemy;
    Rigidbody rb;
    MeshRenderer mr;
    float damage;
    float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
    }

    public void Initialize(FacilityAttack facilityAttack, Enemy enemy)
    {
        damage = facilityAttack.AttackPower;
        speed = facilityAttack.AttackSpeed;
        mr.material = facilityAttack.Material;
        this.enemy = enemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
        {
            return;
        }
        enemy = other.gameObject.GetComponent<Enemy>();
        enemy.TakeDamage(damage);
        Destroy(gameObject);
    }

    void Update()
    {
        if (enemy == null)
            return;
        rb.velocity = (enemy.transform.position - transform.position).normalized * speed;
    }
}
