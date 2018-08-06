using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour {

    public GameObject laserPoint;
    public GameObject ropePrefab;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire2"))
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position + transform.up, transform.forward, out hit))
            {
                laserPoint.SetActive(true);
                laserPoint.transform.position = hit.point;
                laserPoint.transform.position += hit.normal*0.01f;
                laserPoint.transform.rotation = Quaternion.LookRotation(hit.normal);
                //Debug.Log(hit.normal);
            }
            else
                laserPoint.SetActive(false);
        }
        else
            laserPoint.SetActive(false);


        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position + transform.up, transform.forward, out hit))
            {
                GameObject rope = Instantiate(ropePrefab, hit.point, Quaternion.LookRotation(transform.position - hit.point));
                rope.GetComponent<RopeScript>().dist = (int)((float)hit.distance * (float)5);
                Debug.Log(hit.distance);
            }
        }
        else if (Input.GetButton("Fire1"))
        {
            HingeJoint hinge = gameObject.GetComponent<BB8MoveScript>().bb8Base.AddComponent<HingeJoint>();
            hinge.connectedBody = GameObject.FindGameObjectWithTag("RopeEnd").GetComponent<Rigidbody>();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            Destroy(gameObject.GetComponent<BB8MoveScript>().bb8Base.GetComponent<HingeJoint>());
            Destroy(GameObject.FindGameObjectWithTag("RopeEnd").transform.parent.gameObject);
        }
    }
}
