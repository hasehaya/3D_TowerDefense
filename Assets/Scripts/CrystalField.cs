using UnityEngine;

public class CrystalField :MonoBehaviour
{
    public Crystal crystal;
    [SerializeField] GameObject spriteObj;
    [SerializeField] SpriteRenderer sr;

    private float vibrationStrength = 0.3f; //U•
    private float vibrationOffset = 1f; //’†S
    private float vibrationSpeed = 3f; //üŠú

    private void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            vibrationOffset += hit.point.y;
        }
    }

    private void Update()
    {
        spriteObj.transform.LookAt(Camera.main.transform.position);
        //y = ’†S + U• * sin(2ƒÎt/üŠú)
        transform.position = new Vector3(
            transform.position.x,
            vibrationOffset + vibrationStrength * Mathf.Sin(2 * Mathf.PI * Time.time / vibrationSpeed),
            transform.position.z);
    }

    public void SetCrystal(Crystal crystal, Sprite sprite)
    {
        this.crystal = crystal;
        sr.sprite = sprite;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CrystalBox.Instance.AddCrystal(crystal);
            Destroy(gameObject);
        }
    }
}