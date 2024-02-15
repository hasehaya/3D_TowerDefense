using UnityEngine;

public class Bullet :MonoBehaviour
{
    Enemy enemy;
    Rigidbody rb;
    [SerializeField] int damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetEnemy(Enemy enemy)
    {
        this.enemy = enemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        rb.velocity = (enemy.transform.position - transform.position).normalized * 10;
    }
}
