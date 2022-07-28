using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShootingTrigger : MonoBehaviour
{
    public BoxCollider shootingZone;
    public Rigidbody ballRb;

    private bool trackingActive;
    private List<Vector3> positions;
    private List<Vector3> velocities;

    private ControllerMetrics metrics;

    private Vector3 shootVelocity;
    private bool shoot;

    // Start is called before the first frame update
    void Start()
    {
        trackingActive = false;
        positions = new List<Vector3>();
        velocities = new List<Vector3>();

        metrics = GetComponent<ControllerMetrics>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<XRDirectInteractor>().allowSelect = true;
        if (trackingActive)
        {
            positions.Add(metrics.currentPosition);
            velocities.Add(metrics.currentVelocity);
            if (metrics.currentAngVelocity.x > 400)
            {
                Shoot();
            }
        }

        if (shoot)
        {
            ballRb.AddForce(shootVelocity, ForceMode.VelocityChange);
            shootVelocity *= 0;

            shoot = false;
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

    protected void Shoot()
    {
        shootVelocity = (velocities[velocities.Count-3] + velocities[velocities.Count - 2] + velocities[velocities.Count - 1]) / 3;
        Debug.Log($"Ball released at: {shootVelocity}");
        GetComponent<XRDirectInteractor>().allowSelect = false;
        shoot = true;
        StopTracking();
    }
}
