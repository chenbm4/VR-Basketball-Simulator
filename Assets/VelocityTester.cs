using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTester : MonoBehaviour
{
    private int count;
    public BallVelocity bv;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        count++;

        if (count == 100)
        {
            Debug.Log($"World Velocity: {bv.GetVelocity()}");
            Debug.Log($"Velocity to Rim: {bv.GetVelocityToRim()}");
            count = 0;
        }
    }
}
