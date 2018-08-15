using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwingScript : MonoBehaviour {

    public List<Vector3> pointsInRope;

    public bool ropeActived;

    LineRenderer lineRender;

    GameObject playerBase;

    GameObject ropeAttachPoint;

    float nonAttachechRopeLength;

    public float maxLength = 5;

    float ropeUpdateDistance = 0.1f;

    public Sprite[] hitMarkers;

    public Image targetReticule;

    private void Start()
    {
        lineRender = GetComponent<LineRenderer>();
        playerBase = GameObject.FindGameObjectWithTag("PlayerBase");

        //set up the rope attach point
        ropeAttachPoint = new GameObject();
        ropeAttachPoint.transform.name = "Rope Attach Point";
        ropeAttachPoint.AddComponent<Rigidbody>();
        ropeAttachPoint.GetComponent<Rigidbody>().isKinematic = true;
    }


    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            ToggleRope();
        }

        if (ropeActived)
        {
            if (targetReticule != null)//Update target Reticule
                targetReticule.sprite = hitMarkers[2];

            CheckPoints();
            DrawRope();
            CalculateRopeLength();
            MoveRopeAttachPoint();
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

                //Add a configurable joint
                ConfigurableJoint joint = playerBase.AddComponent<ConfigurableJoint>();
                //Set up the configurable joint
                joint.xMotion = ConfigurableJointMotion.Limited;
                joint.yMotion = ConfigurableJointMotion.Limited;
                joint.zMotion = ConfigurableJointMotion.Limited;

                SoftJointLimit limits = new SoftJointLimit();
                limits.limit = maxLength;
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
        limits.limit = maxLength - nonAttachechRopeLength;
        joint.linearLimit = limits;
    }
}
