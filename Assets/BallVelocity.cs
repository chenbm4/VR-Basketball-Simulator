using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallVelocity : MonoBehaviour
{
    public Rigidbody rb;
    public Transform rimTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }

    public Vector3 GetVelocityToRim()
    {
        return rb.GetRelativePointVelocity(rimTransform.position);
    }
}
