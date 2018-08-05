using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BB8MoveScript : MonoBehaviour {

    Rigidbody rb;
    public GameObject bb8Base;

    public float moveSpeed = 2;
    public float vertRotSpeed = 2;
    public float horiRotSpeed = 1;

    public float jumpForce = 1;

    private void Start()
    {
        rb = bb8Base.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    float verticalInput;
    float horizontalInput;

    float mouseX;
    float mouseY;

    // Update is called once per frame
    void Update () {
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Move();
        Look();
    }

    void Move()
    {
        //add force relative to direction of look
        verticalInput = verticalInput * moveSpeed;
        horizontalInput = horizontalInput * moveSpeed;

        Vector3 forwardPush = transform.forward * verticalInput;
        Vector3 sidewaysPush = transform.right * horizontalInput;

        rb.AddForce(forwardPush + sidewaysPush, ForceMode.Acceleration);
    }

    void Look()
    {
        //rotate head around body


        transform.Rotate(new Vector3(-mouseY * vertRotSpeed, 0));
        transform.Rotate(Vector3.up, mouseX * horiRotSpeed, Space.World);
        //transform.Rotate(transform.right, -mouseY);
    }

    void Jump()
    {
        //Test to see if there is a hit using a BoxCast
        //Calculate using the center of the GameObject's Collider(could also just use the GameObject's position), half the GameObject's size, the direction, the GameObject's rotation, and the maximum distance as variables.
        //Also fetch the hit data
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, new Vector3(0.2f, 0.2f, 0.2f), -Vector3.up, out hit, Quaternion.identity, 0.5f))
        {
            //Output the name of the Collider your Box hit
            Debug.Log("Hit : " + hit.collider.name);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
        
    }
}
