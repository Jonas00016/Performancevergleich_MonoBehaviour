using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const float MOVEMENTSPEED = 2000f;
    private const float MAX_LIFETIME = 5f;

    private Rigidbody rb;
    private float lifetime = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Age();
    }

    void FixedUpdate()
    {
        MoveForward();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.tag.Equals("Enemy")) return;

        DestroyEnemy(collision.gameObject);
    }

    private void DestroyEnemy(GameObject enemy)
    {
        Destroy(enemy);
        Destroy(gameObject);
    }

    private void MoveForward()
    {
        rb.velocity = transform.forward * MOVEMENTSPEED * Time.deltaTime;
    }

    private void Age()
    {
        lifetime += Time.deltaTime;

        if (lifetime < MAX_LIFETIME) return;

        Destroy(this.gameObject);
    }
}
