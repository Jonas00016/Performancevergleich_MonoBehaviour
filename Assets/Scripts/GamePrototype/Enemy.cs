using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private const float MOVEMENTSPEED = 100f;

    private Rigidbody rb;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        LookAtPlayer();
    }
    void FixedUpdate()
    {
        MoveForward();
    }

    private void LookAtPlayer()
    {
        transform.LookAt(player);
    }

    private void MoveForward()
    {
        rb.velocity = transform.forward * MOVEMENTSPEED * Time.deltaTime;
    }
}
