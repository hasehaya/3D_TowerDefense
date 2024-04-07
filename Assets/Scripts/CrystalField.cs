using UnityEngine;

public class CrystalField :MonoBehaviour
{
    public Crystal crystal;
    [SerializeField] GameObject spriteObj;
    [SerializeField] SpriteRenderer sr;

    private float vibrationStrength = 0.3f; //振れ幅
    private float vibrationOffset = 2f; //初期値
    private float vibrationFrequency = 3f; //周波数

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
        //y = 初期値 + 振れ幅 * sin(2π * 時間 / 周波数)
        transform.position = new Vector3(
            transform.position.x,
            vibrationOffset + vibrationStrength * Mathf.Sin(2 * Mathf.PI * Time.time / vibrationFrequency),
            transform.position.z);
    }

    public void SetCrystal(Crystal crystal)
    {
        this.crystal = crystal;
        sr.sprite = crystal.sprite;
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