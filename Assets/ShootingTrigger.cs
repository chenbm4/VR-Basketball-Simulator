using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShootingTrigger : MonoBehaviour
{
    public BoxCollider shootingZone;
    public Rigidbody ballRb;
    public Transform headsetLoc;

    private bool trackingActive;
    private Vector3 startingPos;

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
            //ballRb.AddForce(new Vector3(0, 2, 0), ForceMode.VelocityChange);
            shootVelocity *= 0;
            StopTracking();
        }

        Grapher.Log(releaseStarted ? 100:0, "Release started");

        // measure distance from starting point
        float distFromStart = Vector3.Distance(startingPos, metrics.currentPosition);
        float distFromHeadset = Vector3.Distance(headsetLoc.position, metrics.currentPosition);

        if (trackingActive)
        {
            //positions.Add(metrics.currentPosition);
            //velocities.Add(metrics.currentVelocity);
            //accelerationMags.Add(metrics.currentMagAcceleration);

            if (!releaseStarted)
            {
                if (metrics.currentMagAcceleration > 200.0f) 
                {
                    releaseStarted = true;
                    Debug.Log("Release started.");
                }
                else
                {
                    startingPos = metrics.currentPosition;
                }
            }
            else if (metrics.currentVelSqrMagnitude > 15.0f) // if release has started and ball still moving
            {
                
                // take in release data
                positions.Add(metrics.currentPosition);
                velocities.Add(metrics.currentVelocity);
                accelerationMags.Add(metrics.currentMagAcceleration);
            } 
            else // if ball no longer moving
            {
                if (distFromStart > 0.2f && distFromHeadset > 0.5f) // if ball has moved far enough and isn't too close to the headset
                {
                    float shootingMagnitude = 0.0f;

                    // code for max velocity as shooting magnitude
                    //foreach (Vector3 velocity in velocities)
                    //{
                    //    var velocityMag = velocity.magnitude;

                    //    if (velocityMag > shootingMagnitude)
                    //    {
                    //        shootingMagnitude = velocityMag;
                    //    }
                    //}

                    // 90th percentile magnitude
                    int count = velocities.Count();
                    var orderedVelocities = velocities.OrderBy(v => v.magnitude);
                    shootingMagnitude = orderedVelocities.ElementAt(count-1).magnitude + 
                        orderedVelocities.ElementAt((count-2)).magnitude +
                        orderedVelocities.ElementAt((count - 3)).magnitude;
                    shootingMagnitude /= 3;

                    //List<Vector3> preFlickVelocities = new List<Vector3>();

                    // find highest position index
                    //int index = 0;
                    //int largestYIndex = 0;
                    //float largestY = 0.0f;

                    //foreach (var position in positions)
                    //{
                    //    if (position.y > largestY)
                    //    {
                    //        largestY = position.y;
                    //        largestYIndex = index;
                    //    }
                    //    index++;
                    //}

                    //for (int i=0; i<largestYIndex; i++)
                    //{
                    //    preFlickVelocities.Add(velocities[i]);
                    //}

                    var directionVector = new Vector3(
                        velocities.Average(x => x.x),
                        velocities.Average(x => x.y),
                        velocities.Average(x => x.z));
                    shootVelocity = directionVector.normalized * (shootingMagnitude * 1.3f);

                    GetComponent<XRDirectInteractor>().allowSelect = false;
                    shoot = true;
                    //shootVelocity = (velocities[maxVelocityIndex - 1] + velocities[maxVelocityIndex]) / 2;
                    //Debug.Log($"Ball released at: {shootVelocity}, index = {maxVelocityIndex}");
                    Debug.Log($"Ball released at: {shootVelocity}");
                } 
                else // ball hasn't moved far enough or too close to headset, so reset data
                {
                    startingPos = metrics.currentPosition;
                    //releaseStarted = false;
                    positions.Clear();
                    velocities.Clear();
                    accelerationMags.Clear();

                    Debug.Log($"Ball not far enough from starting point or ball too close to headset");
                }
            }
            
        }
    }

    public void StartTracking()
    {
        if (!trackingActive)
        {
            Debug.Log("Start Tracking");
            trackingActive = true;
            //GetComponent<ControllerMetrics>().StartTracking(ref positions, ref velocities);
            GetComponent<ControllerMetrics>().debugEnabled = true;
        }
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
