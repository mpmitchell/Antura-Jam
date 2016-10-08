using UnityEngine;
using System.Collections;

public class aceleration : MonoBehaviour
{
    public Vector3 forceVec;
    public Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        rb.AddForce(Input.gyro.userAcceleration.x * forceVec);
    }
}