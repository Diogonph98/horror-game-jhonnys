using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    private Vector3 rot;
    public float speed;
    public float cameraSens;
    [SerializeField] private GameObject cameraAttached;
    [SerializeField] private Rigidbody rb;

    private Vector3 movementDiretion;

    public float jumpForce = 20f;
    public int jumpIterations = 5;

    public bool isJumping = false;
    public bool isGrounded = true;
    public bool isSpriting = false;

    public Vector3 gravityApplied;
    public Vector3 earthGravity;



    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        if (!IsOwner) return;

        GameObject.Find("Camera").SetActive(false);

        
    }

    // Start is called before the first frame update
    void Start()
    {
        earthGravity = Physics.gravity * 10;
        gravityApplied = earthGravity;
        cameraAttached = transform.GetChild(0).gameObject;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        Move();
        Rotate();

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            StartCoroutine(applyJump());
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && !isSpriting && isGrounded)
        {
            isSpriting = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift) && isSpriting)
        {
            isSpriting = false;
        }

        ApplyGravity();
        if(!isGrounded)
        {
            rb.AddForce(gravityApplied);
        }
    }

    private IEnumerator applyJump()
    {
        isJumping = true;
        var t = jumpIterations;
        var up = new Vector3(0, 1, 0);
        while (t > 0)
        {
            t -= 1;
            rb.AddForce(up * jumpForce * 10);
            yield return null;
        }
        isJumping = false;
    }

    private void ApplyGravity()
    {
        int layerMask = (1 << 3);
        if(Physics.CheckSphere(transform.position,1.05f,layerMask))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

   

    private void Rotate()
    {
        float InputMouseX = Input.GetAxis("Mouse X");

        rot += new Vector3(0, InputMouseX, 0) * cameraSens;
        transform.eulerAngles = rot;
        var forward = (transform.position - cameraAttached.transform.position).normalized;
        forward.y = 0;
        transform.forward = forward;
    }

    private void Move()
    {
        movementDiretion = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            movementDiretion += transform.forward;
        }
        if(Input.GetKey(KeyCode.S))
        {
            movementDiretion -= transform.forward;
        }
        if(Input.GetKey(KeyCode.D))
        {
            movementDiretion += transform.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementDiretion -= transform.right;
        }

        //movementDiretion.y = Physics.gravity.y;

        if(!isSpriting)
            rb.velocity = movementDiretion * speed *100* Time.deltaTime;
        else
            rb.velocity = movementDiretion * speed*2 * 100 * Time.deltaTime;
    }
}
