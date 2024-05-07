using UnityEngine;

public class Muzzle :MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float ct;

    FacilityAttack canon;
    float coolTimeCounter;


    private void Start()
    {
        canon = GetComponentInParent<FacilityAttack>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!canon.isInstalled)
            return;
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (ct <= coolTimeCounter)
            {
                var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.GetComponent<Bullet>().SetEnemy(other.gameObject.GetComponent<Enemy>());
                coolTimeCounter = 0;
            }
            else
            {
                coolTimeCounter += Time.deltaTime;
            }
        }
    }
}
