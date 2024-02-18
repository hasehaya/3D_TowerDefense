using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Muzzle :MonoBehaviour
{
    [SerializeField] Canon canon;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float ct;
    float coolTimeCounter;

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
