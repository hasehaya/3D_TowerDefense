using UnityEngine;

public class Canon :MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform muzzleTransform;
    [SerializeField] float ct;
    float coolTimeCounter;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (ct <= coolTimeCounter)
            {
                var bullet = Instantiate(bulletPrefab, muzzleTransform.position, Quaternion.identity);
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
