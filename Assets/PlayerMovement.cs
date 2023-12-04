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

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraAttached = transform.GetChild(0).gameObject;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        Move();
        Rotate();
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


        rb.velocity = movementDiretion * speed * Time.deltaTime;
    }
}
