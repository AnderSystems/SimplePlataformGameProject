using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRb : MonoBehaviour
{
    public Rigidbody rb;
    [Min(0)]
    public float ImpactForce;

    private void OnDrawGizmosSelected()
    {
        if (!rb)
        {
            TryGetComponent<Rigidbody>(out rb);
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>())
        {
            if (collision.relativeVelocity.magnitude >= ImpactForce)
            {
                rb.constraints = RigidbodyConstraints.None;
                rb.detectCollisions = true;
                rb.isKinematic = false;
            }
        }
    }
}
