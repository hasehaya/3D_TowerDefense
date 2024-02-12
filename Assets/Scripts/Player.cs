using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var velocity = new Vector3(horizontal, 0, vertical).normalized;
        var speed = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;

        if (velocity.magnitude > 0.5f)
        {
            transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
        }

        animator.SetFloat("Speed", velocity.magnitude * speed , 0.1f , Time.deltaTime);
    }
}
