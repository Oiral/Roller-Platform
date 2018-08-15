using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour {

    public List<Rigidbody> rbInTrigger;

    private void OnTriggerEnter(Collider other)
    {
        rbInTrigger.Add(other.attachedRigidbody);
    }
    private void OnTriggerExit(Collider other)
    {
        if (rbInTrigger.Contains(other.attachedRigidbody))
        {
            rbInTrigger.Remove(other.attachedRigidbody);
        }
    }

    private void FixedUpdate()
    {
        foreach (Rigidbody rb in rbInTrigger)
        {
            rb.AddForce(transform.up * 18, ForceMode.Acceleration);
        }
    }
}
