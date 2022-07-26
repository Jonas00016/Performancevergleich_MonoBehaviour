using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float ROTATIONSPEED = 0.66f;
    private const float MOVEMENTSPEED = 500f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }

    void Update()
    {
        HandleRotation();
        HandleMovement();
    }

    private void HandleRotation()
    {
        float rotationOffset = ROTATIONSPEED * Input.GetAxis("Mouse X");

        transform.rotation *= Quaternion.Euler(0f, rotationOffset, 0f);
    }

    private void HandleMovement()
    {
        Vector3 verticalInput = Vector3.forward * (Input.GetKey("w") ? 1f : (Input.GetKey("s") ? -1f : 0f));
        Vector3 horizontalInput = Vector3.right * (Input.GetKey("d") ? 1f : (Input.GetKey("a") ? -1f : 0f));

        rb.velocity = transform.rotation * Vector3.Normalize(verticalInput + horizontalInput) * MOVEMENTSPEED * Time.deltaTime;
    }
}
