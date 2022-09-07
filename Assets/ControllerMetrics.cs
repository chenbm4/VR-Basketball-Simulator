using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerMetrics : MonoBehaviour
{
    // current metrics (public for accessibility in other classes)
    //  position coordinates are world, not local
    public Vector3 currentPosition;
    public Quaternion currentRotation;
    public Vector3 currentVelocity;
    public Vector3 currentAngVelocity;
    public float currentVelSqrMagnitude;
    public float currentAngVelSqrMagnitude;
    //public Vector3 currentAngularVelocity;
    public Vector3 currentAcceleration;
    public float currentMagAcceleration;

    // last usage metrics (for velocity calculations)
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private Vector3 lastVelocity;
    private float lastVelocitySqrMagnitude;

    // transform data of the controller
    private Transform controllerTransform;

    private float currentTime;
    private float lastTime;

    private List<Vector3> positionsList;
    private List<Vector3> velocitiesList;

    public bool debugEnabled;
    private bool trackingActive;

    // Start is called before the first frame update
    void Start()
    {
        controllerTransform = GetComponent<Transform>();
        lastPosition = controllerTransform.position;
        lastRotation = controllerTransform.localRotation;
        lastVelocity = Vector3.zero;
        lastVelocitySqrMagnitude = 0.0f;

        lastTime = Time.realtimeSinceStartup;

        positionsList = null;
        velocitiesList = null;
        //accelerationsList = null;

        trackingActive = false;

        //Grapher.Init();
    }

    /// <summary>
    /// See <see cref="MonoBehaviour"/>.
    /// </summary>
    //protected virtual void OnEnable()
    //{
    //    Application.onBeforeRender += OnBeforeRender;
    //}

    ///// <summary>
    ///// See <see cref="MonoBehaviour"/>.
    ///// </summary>
    //protected virtual void OnDisable()
    //{
    //    Application.onBeforeRender -= OnBeforeRender;
    //}

    // Update is called once per frame
    void Update()
    {
        //if (debugEnabled)
        //{
        //    Debug.Log("Update");
        //}

        UpdateMetrics();
    }

    // Controller position updates before render, so get this data
    //void OnBeforeRender()
    //{
    //    if (debugEnabled)
    //    {
    //        Debug.Log("OnPreRender");
    //    }

    //    UpdateMetrics();
    //}

    // UpdateMetrics gets current metrics and calculates velocities accordingly
    void UpdateMetrics()
    {
        currentTime = Time.realtimeSinceStartup;
        // get current metrics
        currentPosition = controllerTransform.position;
        currentRotation = controllerTransform.localRotation;
        if (trackingActive)
        {
            positionsList.Add(currentPosition);
        }

        // calculate velocities and acceleration
        var timeDelta = currentTime - lastTime;
        if (timeDelta != 0)
        {
            currentVelocity = (currentPosition - lastPosition) / (timeDelta);
            Quaternion rotationDelta = currentRotation * Quaternion.Inverse(lastRotation);
            var eulerRotation = new Vector3(Mathf.DeltaAngle(0, rotationDelta.eulerAngles.x), Mathf.DeltaAngle(0, rotationDelta.eulerAngles.y), Mathf.DeltaAngle(0, rotationDelta.eulerAngles.z));
            currentAngVelocity = eulerRotation / (timeDelta);
            currentVelSqrMagnitude = currentVelocity.sqrMagnitude;
            currentAngVelSqrMagnitude = currentAngVelocity.sqrMagnitude;
            currentAcceleration = (currentVelocity - lastVelocity) / (timeDelta);
            currentMagAcceleration = (currentVelSqrMagnitude - lastVelocitySqrMagnitude) / (timeDelta);
            if (trackingActive)
            {
                velocitiesList.Add(currentVelocity);
                //accelerationsList.Add(currentAcceleration);
            }

            Grapher.Log(currentPosition.x, "Position X Graph");
            Grapher.Log(currentPosition.y, "Position Y Graph");
            Grapher.Log(currentPosition.z, "Position Z Graph");

            Grapher.Log(currentVelocity.x, "Velocity X Graph");
            Grapher.Log(currentVelocity.y, "Velocity Y Graph");
            Grapher.Log(currentVelocity.z, "Velocity Z Graph");

            // if debug enabled, output data to debug
            if (debugEnabled)
            {
                Debug.Log(OutputDebug());
            }

            // reset last datapoints
            lastPosition = currentPosition;
            lastRotation = currentRotation;
            lastTime = currentTime;
            lastVelocity = currentVelocity;
            lastVelocitySqrMagnitude = currentVelSqrMagnitude;
        }
    }

    // adds datapoints to given lists when tracking started
    public void StartTracking(ref List<Vector3> positions, ref List<Vector3> velocities)
    {
        trackingActive = true;
        positionsList = positions;
        velocitiesList = velocities;
    }

    public void StopTracking()
    {
        trackingActive = false;
        //DebugGUI.RemoveGraph("positionXGraph");
    }

    //public void GraphData()
    //{
    //    DebugGUI.Graph("PositionXGraph", currentPosition.x);
    //}

    string OutputDebug()
    {
        string output = string.Format($"Current Time: {currentTime}: \n" +
            $"Last Position Change Time: {lastTime}; \n" +
            $"Current Position: {currentPosition}; \n" +
            $"Last Position: {lastPosition}; \n" +
            $"Current Rotation: {currentRotation}; \n" +
            $"Current Velocity: {currentVelocity}; \n" +
            $"Last Velocity: {lastVelocity}; \n" +
            $"Current Velocity Magnitude: {currentVelSqrMagnitude}; \n" +
            $"Current Angular Velocity: {currentAngVelocity}; \n" +
            $"Current Angular Velocity Magnitude: {currentAngVelSqrMagnitude}; \n" +
            $"Current Acceleration: {currentAcceleration}; \n" +
            $"Current Magnitude Acceleration: {currentMagAcceleration}; \n" +
            $"Time Delta: {currentTime - lastTime}");

        return output;
    }
}
