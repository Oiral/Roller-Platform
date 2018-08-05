using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour {

    public GameObject laserPoint;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire1"))
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
	}
}
