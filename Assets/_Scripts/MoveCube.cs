using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{

    public float speed;
    public Rigidbody body;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var hor = Input.GetAxisRaw("Horizontal");  
        var ver = Input.GetAxisRaw("Vertical");
        Vector3 force = new Vector3(hor, 0, ver) * speed * 100 * Time.deltaTime;
        body.AddForce(force);


    }
}
