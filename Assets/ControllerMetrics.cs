using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMetrics : MonoBehaviour
{
    // current metrics (public for accessibility in other classes)
    //  position coordinates are world, not local
    public Vector3 currentPosition;
    //public Quaternion currentRotation;
    public Vector3 currentVelocity;
    //public Vector3 currentAngularVelocity;

    // last usage metrics (for velocity calculations)
    private Vector3 lastPosition;
    //private Quaternion lastRotation;

    // transform data of the controller
    private Transform controllerTransform;

    private float currentTime;
    private float lastTime;
    private int debugCounter = 0;

    public bool debugEnabled;

    // Start is called before the first frame update
    void Start()
    {
        controllerTransform = GetComponent<Transform>();
        currentPosition = lastPosition = controllerTransform.position;
        lastTime = Time.realtimeSinceStartup;
    }

    /// <summary>
    /// See <see cref="MonoBehaviour"/>.
    /// </summary>
    protected virtual void OnEnable()
    {
        Application.onBeforeRender += OnBeforeRender;
    }

    /// <summary>
    /// See <see cref="MonoBehaviour"/>.
    /// </summary>
    protected virtual void OnDisable()
    {
        Application.onBeforeRender -= OnBeforeRender;
    }

    // Update is called once per frame
    void Update()
    {
        if (debugEnabled)
        {
            Debug.Log("Update");
        }

        UpdateMetrics();
    }

    // Controller position updates before render, so get this data
    void OnBeforeRender()
    {
        if (debugEnabled)
        {
            Debug.Log("OnPreRender");
        }

        UpdateMetrics();
    }

    // UpdateMetrics gets current metrics and calculates velocities accordingly
    void UpdateMetrics()
    {
        // get current metrics
        currentPosition = controllerTransform.position;
        currentTime = Time.realtimeSinceStartup;

        // calculate velocity
        var timeDelta = currentTime - lastTime;
        if (timeDelta != 0)
        {

            currentVelocity = (currentPosition - lastPosition) / (timeDelta);

            // if debug enabled, output data to debug
            if (debugEnabled)
            {
                // every 1 position changes, print debug
                if (debugCounter == 1)
                {
                    Debug.Log(OutputDebug());
                    debugCounter = 0;
                }
                debugCounter++;
            }

            // reset last datapoints
            lastPosition = currentPosition;
            lastTime = currentTime;
        }
    }

    string OutputDebug()
    {
        string output = string.Format($"Current Time: {currentTime}: \n" +
            $"Last Position Change Time: {lastTime}; \n" +
            $"Current Position: {currentPosition}; \n" +
            $"Last Position: {lastPosition}; \n" +
            $"Current Velocity: {currentVelocity}; \n" +
            $"Time Delta: {currentTime - lastTime}");

        return output;
    }
}
