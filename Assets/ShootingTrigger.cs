using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShootingTrigger : MonoBehaviour
{
    public BoxCollider shootingZone;

    private bool trackingActive;
    private List<Vector3> positions;
    private List<Vector3> velocities;

    // Start is called before the first frame update
    void Start()
    {
        trackingActive = false;
        positions = new List<Vector3>();
        velocities = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (trackingActive)
        {
            velocities.Add(GetComponent<ControllerMetrics>().currentPosition);
            velocities.Add(GetComponent<ControllerMetrics>().currentVelocity);
        }
    }

    public void StartTracking()
    {
        Debug.Log("Start Tracking");
        trackingActive = true;
        GetComponent<ControllerMetrics>().StartTracking(ref positions, ref velocities);
        GetComponent<ControllerMetrics>().debugEnabled = true;
    }

    public void StopTracking()
    {
        //foreach (Vector3 position in positions)
        //{
        //    Debug.Log(position);
        //}

        //foreach (Vector3 velocity in velocities)
        //{
        //    Debug.Log(velocity);
        //}

        positions.Clear();
        velocities.Clear();
        trackingActive = false;
        GetComponent<ControllerMetrics>().StopTracking();
        GetComponent<ControllerMetrics>().debugEnabled = false;
        Debug.Log("Stop Tracking");
    }
}
