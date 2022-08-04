using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float MOVEMENTSPEED = 200f;

    [SerializeField] EnemySpawnSystem enemySpawnSystem;
    [SerializeField] GameObject projectilePrefab;

    private Rigidbody rb;
    private float perSecond = 50f;
    private float nextTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }

    void Update()
    {
        HandleRotation();
        HandleShootProjectile();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.tag.Equals("Enemy")) return;

        HandleGameOver();
    }

    private void HandleGameOver()
    {
        enemySpawnSystem.enabled = false;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }

        foreach (GameObject projectile in GameObject.FindGameObjectsWithTag("Projectile"))
        {
            Destroy(projectile);
        }

        Destroy(gameObject);
    }

    private void HandleRotation()
    {
        transform.rotation *= Quaternion.Euler(0f, Input.GetAxis("Mouse X"), 0f);
    }

    private void HandleMovement()
    {
        Vector3 verticalInput = Vector3.forward * (Input.GetKey("w") ? 1f : (Input.GetKey("s") ? -1f : 0f));
        Vector3 horizontalInput = Vector3.right * (Input.GetKey("d") ? 1f : (Input.GetKey("a") ? -1f : 0f));

        rb.velocity = transform.rotation * Vector3.Normalize(verticalInput + horizontalInput) * MOVEMENTSPEED * Time.deltaTime;
    }

    private void HandleShootProjectile()
    {
        if (!Input.GetKey("space") || Time.time < nextTime) return;
        nextTime = Time.time + 1f / perSecond;

        Instantiate(projectilePrefab, transform.position, transform.rotation);
    }
}
