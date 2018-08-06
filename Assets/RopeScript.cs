using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour {

    Rigidbody rb;

    public int dist = 1;

    public GameObject ropePart;

    private void Start()
    {
        gameObject.AddComponent<Rigidbody>();
        rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        for (int i = 0; i < dist; i++)
        {
            Instantiate(ropePart, Vector3.Lerp(transform.position, GameObject.FindGameObjectWithTag("PlayerBase").transform.position,  (float)i / (float)dist), Quaternion.identity, transform);
        }

        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform t = transform.GetChild(i);
            t.gameObject.AddComponent<HingeJoint>();

            t.gameObject.GetComponent<Rigidbody>().mass = 0.3f;
            t.gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            HingeJoint hinge = t.gameObject.GetComponent<HingeJoint>();

            hinge.connectedBody = i == 0 ? rb :
                transform.GetChild(i - 1).GetComponent<Rigidbody>();

            hinge.useSpring = true;
            if (i == childCount - 1)
            {
                t.tag = "RopeEnd";
            }
        }
    }
}
