using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BB8HeadScript : MonoBehaviour {

    public Transform attachedPoint;
    public Vector3 offset;
	
	// Update is called once per frame
	void Update () {
        transform.position = attachedPoint.position;
        transform.position -= offset;
	}
}
