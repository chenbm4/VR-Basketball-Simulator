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
    private List<float> accelerationMags;

    private ControllerMetrics metrics;

    private Vector3 shootVelocity;
    private bool shoot;
    private bool releaseStarted;

    // Start is called before the first frame update
    void Start()
    {
        trackingActive = false;
        positions = new List<Vector3>();
        velocities = new List<Vector3>();
        accelerationMags = new List<float>();
        shoot = false;
        releaseStarted = false;

        metrics = GetComponent<ControllerMetrics>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shoot)
        {
            ballRb.AddForce(shootVelocity, ForceMode.VelocityChange);
            shootVelocity *= 0;
            StopTracking();
        }
        
        if (trackingActive)
        {
            //positions.Add(metrics.currentPosition);
            //velocities.Add(metrics.currentVelocity);
            //accelerationMags.Add(metrics.currentMagAcceleration);
            
            if (!releaseStarted && metrics.currentMagAcceleration > 500.0f)
            {
                releaseStarted = true;
                Debug.Log("Release started.");
            }

            if (releaseStarted)
            {
                positions.Add(metrics.currentPosition);
                velocities.Add(metrics.currentVelocity);
                accelerationMags.Add(metrics.currentMagAcceleration);

                if (metrics.currentVelSqrMagnitude < 5.0f)
                {
                    int maxVelocityIndex = 0;
                    int indexCounter = 0;
                    int shotAcceleratingCounter = 0;

                    foreach (float acceleration in accelerationMags)
                    {
                        if (acceleration > 0.0f)
                        {
                            shotAcceleratingCounter++;
                        }
                        else if (shotAcceleratingCounter >= 2)
                        {
                            maxVelocityIndex = indexCounter;
                            break;
                        }
                        else
                        {
                            shotAcceleratingCounter = 0;
                        }

                        indexCounter++;
                    }

                    shootVelocity = (/*velocities[maxAccelIndex-2] + */velocities[maxVelocityIndex - 1] /*+ velocities[maxVelocityIndex]*/);
                    Debug.Log($"Ball released at: {shootVelocity}, index = {maxVelocityIndex}");
                    GetComponent<XRDirectInteractor>().allowSelect = false;
                    shoot = true;
                }
            }
        }
    }

    public void StartTracking()
    {
        Debug.Log("Start Tracking");
        trackingActive = true;
        //GetComponent<ControllerMetrics>().StartTracking(ref positions, ref velocities);
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
        shoot = false;
        releaseStarted = false;

        positions.Clear();
        velocities.Clear();
        accelerationMags.Clear();
        trackingActive = false;
        GetComponent<ControllerMetrics>().StopTracking();
        GetComponent<ControllerMetrics>().debugEnabled = false;
        GetComponent<XRDirectInteractor>().allowSelect = true;
        Debug.Log("Stop Tracking");
    }
}
