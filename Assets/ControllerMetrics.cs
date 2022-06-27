using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ControllerMetrics : MonoBehaviour
{

    private int count;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        count++;

        if (count == 100)
        {
            Debug.Log($"World Velocity: {GetPosition()}");
            count = 0;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
