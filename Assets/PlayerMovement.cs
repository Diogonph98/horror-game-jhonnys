using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    private Vector3 rot;
    public float speed;
    public float cameraSensX;
    public float cameraSensY;
    [SerializeField] private GameObject cameraAttached;
    [SerializeField] private GameObject cameraPivot;
    [SerializeField] private Rigidbody rb;

    private Vector3 movementDiretion;

    public float jumpForce = 20f;
    public float jumpIterations = 5;

    public bool isJumping = false;
    public bool isGrounded = true;
    public bool isSpriting = false;

    public Vector3 gravityLoaded;
    public Vector3 gravityToApply;

    [Header("Earth Parameters:")]
    public Vector3 earthGravity;
    public float earthJumpForce;
    public float earthSpeed;
    public float earthJumpIterations;



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
        if(GameObject.Find("Camera"))
            GameObject.Find("Camera").SetActive(false);
        gravityToApply = Vector3.zero;
        gravityLoaded = earthGravity;
        cameraAttached = transform.GetChild(0).transform.GetChild(0).gameObject;
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

        isGrounded = CheckIfIsGrounded();
        if (!isGrounded)
        {
            gravityToApply += gravityLoaded;
            rb.AddForce(gravityToApply * Time.fixedDeltaTime);
        }
        else
        {
            gravityToApply = gravityLoaded;
            isJumping = false;
        }
           
    }

    private IEnumerator applyJump()
    {
        //if (isJumping) yield break;
        isJumping = true;
        var t = 0.1f;
        var up = new Vector3(0, 1, 0);

        while (t > 0)
        {
            t -= Time.deltaTime;
            rb.AddForce(up * jumpForce * 10 * Time.fixedDeltaTime);
            yield return null;
        }
        
    }

    private bool CheckIfIsGrounded()
    {
        bool grounded = false;
        int layerMask = (1 << 3);
        if(Physics.CheckSphere(transform.position,1.05f,layerMask))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        return grounded;
    }

   

    private void Rotate()
    {
        float InputMouseX = Input.GetAxis("Mouse X");
        float InputMouseY = Input.GetAxis("Mouse Y");

        rot += new Vector3(-InputMouseY * cameraSensY, InputMouseX * cameraSensX, 0);
        rot.x = Mathf.Clamp(rot.x, -30f, 45f);
        cameraPivot.transform.eulerAngles = rot;
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
            rb.velocity = movementDiretion * speed *10* Time.fixedDeltaTime;
        else
            rb.velocity = movementDiretion * speed*2 * 10 * Time.fixedDeltaTime;
    }
}
