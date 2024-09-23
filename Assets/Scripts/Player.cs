using System.Collections; // コルーチンを使用するために必要

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
    Rigidbody rb;
    Animator animator;
    Quaternion targetRotation = Quaternion.identity;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        targetRotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Jump());
        }
    }

    IEnumerator Jump()
    {
        animator.SetBool("isJump", true);
        yield return new WaitForSeconds(2);
        animator.SetBool("isJump", false);
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var horizontalRotation = Quaternion.AngleAxis(cameraTransform.eulerAngles.y, Vector3.up);
        var velocity = horizontalRotation * new Vector3(horizontal, 0, vertical).normalized;

        var speed = Input.GetKey(KeyCode.LeftShift) ? 1 : 0.5f;
        var rotationSpeed = 600 * Time.deltaTime;

        if (velocity.magnitude > 0.5f)
        {
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
        }

        animator.SetFloat("Speed", velocity.magnitude * speed, 0.1f, Time.deltaTime);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
    }

    public void WarpTo(Vector3 position)
    {
        transform.position = position;
    }
}
