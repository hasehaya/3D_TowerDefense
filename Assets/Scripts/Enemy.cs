using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent nav;
    [SerializeField] GameObject target;
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        nav.destination = target.transform.position;
    }
}