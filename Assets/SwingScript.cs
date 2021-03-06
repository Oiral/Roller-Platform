﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwingScript : MonoBehaviour {

    public List<Vector3> pointsInRope = new List<Vector3>();

    public bool ropeActived;

    LineRenderer lineRender;

    GameObject playerBase;

    GameObject ropeAttachPoint;

    float nonAttachechRopeLength;

    public float maxLength = 2;

    float ropeLength;

    float ropeUpdateDistance = 0.1f;

    public float retractSpeed = 0f;

    public Sprite[] hitMarkers;

    Image targetReticule;

    UnlockScript unlocks;

    private void Start()
    {
        lineRender = GetComponent<LineRenderer>();
        playerBase = GameObject.FindGameObjectWithTag("PlayerBase");
        unlocks = GetComponentInParent<UnlockScript>();
        targetReticule = GameObject.FindGameObjectWithTag("CrossHair").GetComponent<Image>();

        //set up the rope attach point
        ropeAttachPoint = new GameObject();
        ropeAttachPoint.transform.name = "Rope Attach Point";
        ropeAttachPoint.AddComponent<Rigidbody>();
        ropeAttachPoint.GetComponent<Rigidbody>().isKinematic = true;
    }


    // Update is called once per frame
    void Update () {
        //check if the player has unlocked the grapple
        if (Input.GetButtonDown("Fire1") && unlocks.grapple)
        {
            
            ToggleRope();
        }

        if (ropeActived)
        {
            //Debug.Log(ropeLength);
            //Debug.Log(nonAttachechRopeLength);

            if (targetReticule != null)//Update target Reticule
                targetReticule.sprite = hitMarkers[2];

            CheckPoints();
            DrawRope();
            CalculateRopeLength();
            MoveRopeAttachPoint();

            //if you can reel in
            if (unlocks.reel)
            {
                //when you first press the wheel in button
                if (Input.GetButtonDown("Fire2"))
                {
                    //set the rope length to the current length

                    ropeLength = Vector3.Distance(transform.position, pointsInRope[pointsInRope.Count - 1]);
                    ropeLength += nonAttachechRopeLength;
                }
                else if (Input.GetButton("Fire2"))
                {
                    //make the rope smaller
                    ropeLength -= retractSpeed * Time.deltaTime;
                }
            }


        }
        else
        {
            CheckRange();
        }
	}

    void CheckRange()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up, transform.forward, out hit))
        {
            if (hit.distance < maxLength)
            {
                if (targetReticule != null)//Update target Reticule
                    targetReticule.sprite = hitMarkers[1];
            }
            else
            {
                targetReticule.sprite = hitMarkers[0];
            }

        }
        else
        {
            targetReticule.sprite = hitMarkers[0];
        }
    }

    void ToggleRope()
    {
        if (ropeActived)//If the rope is already active and then the player toggles it turn it off
        {
            ropeActived = false;
            pointsInRope.Clear();
            lineRender.enabled = false;
            //Check if the configurable joint is attatched to the player
            if (playerBase.GetComponent<ConfigurableJoint>())
            {
                //Remove the configurable joint
                Destroy(playerBase.GetComponent<ConfigurableJoint>());
            }
            
            
        }
        else
        {
            //check if the rope can be placed and the toggle it on

            RaycastHit hit;
            if (Physics.Raycast(transform.position + transform.up, transform.forward,out hit))
            {
                if (hit.distance > maxLength)
                {
                    return;
                }
                ropeActived = true;
                pointsInRope.Add(hit.point);
                lineRender.enabled = true;

                ropeLength = maxLength;

                //Add a configurable joint
                ConfigurableJoint joint = playerBase.AddComponent<ConfigurableJoint>();
                //Set up the configurable joint
                joint.xMotion = ConfigurableJointMotion.Limited;
                joint.yMotion = ConfigurableJointMotion.Limited;
                joint.zMotion = ConfigurableJointMotion.Limited;

                SoftJointLimit limits = new SoftJointLimit();
                limits.limit = ropeLength;
                limits.contactDistance = 2f;
                joint.linearLimit = limits;

                joint.connectedBody = ropeAttachPoint.GetComponent<Rigidbody>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = new Vector3(0, 0, 0);
            }

        }

    }

    void DrawRope()
    {
        lineRender.positionCount = pointsInRope.Count + 1;
        lineRender.SetPosition(pointsInRope.Count, transform.position);
        for (int i = 0; i < pointsInRope.Count; i++)
        {
            lineRender.SetPosition(i, pointsInRope[i]);
        }
    }

    void CheckPoints()
    {
        //Check the current point in the rope and see if we can see it
        RaycastHit lastPointHit;
        Vector3 lastPoint = pointsInRope[pointsInRope.Count - 1];
        Vector3 dir = lastPoint - transform.position;

        if (Physics.Raycast(transform.position,dir,out lastPointHit))
        {
            if (Vector3.Distance(lastPointHit.point, lastPoint) > ropeUpdateDistance)
            {
                pointsInRope.Add(lastPointHit.point);
            }
        }

        //check if we can see the previous node and if so remove the current node
        if (pointsInRope.Count > 1)
        {
            RaycastHit hit;
            Vector3 prevPoint = pointsInRope[pointsInRope.Count - 2];
            Vector3 dirToPrevPoint = prevPoint - transform.position;

            if (Physics.Raycast(transform.position,dirToPrevPoint,out hit)){
                Debug.DrawRay(transform.position, hit.point - transform.position,Color.red);
                if (Vector3.Distance(prevPoint,hit.point) < ropeUpdateDistance)
                {
                    pointsInRope.RemoveAt(pointsInRope.Count - 1);
                }
            }
        }

    }

    void CalculateRopeLength()
    {
        //reset rope length
        nonAttachechRopeLength = 0;
        //Calcuate distance between player and last point the the rope
        //ropeLength += Vector3.Distance(transform.position, pointsInRope[pointsInRope.Count - 1]);


        for (int i = 0; i < pointsInRope.Count - 1; i++)
        {
            nonAttachechRopeLength += Vector3.Distance(pointsInRope[i], pointsInRope[i + 1]);
        }
    }

    void MoveRopeAttachPoint()
    {
        ropeAttachPoint.transform.position = pointsInRope[pointsInRope.Count - 1];


        //Reset the joint limit to the current attach point
        ConfigurableJoint joint = playerBase.GetComponent<ConfigurableJoint>();

        SoftJointLimit limits = joint.linearLimit;
        limits.limit = ropeLength - nonAttachechRopeLength;
        joint.linearLimit = limits;
    }
}
