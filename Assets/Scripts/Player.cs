using UnityEngine;

public class Player :MonoBehaviour
{
    private static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }
    [SerializeField] Transform cameraTransform;
    Animator animator;
    Quaternion targetRotation = Quaternion.identity;

    void Start()
    {
        animator = GetComponent<Animator>();
        targetRotation = transform.rotation;
    }

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var horizontalRotation = Quaternion.AngleAxis(cameraTransform.eulerAngles.y, Vector3.up);
        var velocity = horizontalRotation * new Vector3(-vertical, 0, horizontal).normalized;


        var speed = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
        var rotationSpeed = 600 * Time.deltaTime;

        if (velocity.magnitude > 0.5f)
        {
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
        }

        animator.SetFloat("Speed", velocity.magnitude * speed, 0.1f, Time.deltaTime);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
    }
}
