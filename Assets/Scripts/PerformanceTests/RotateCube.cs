using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    [SerializeField] float rotationSpeed;

    void Update()
    {
        transform.Rotate(new Vector3(1f, 1f, 1f) * rotationSpeed * Time.deltaTime);
    }
}
