using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;
using UnityEngine.InputSystem;

// for charts
//using System.Windows.Input;

public class DropBall : MonoBehaviour
{
    public BoxCollider shootingZone;
    public GameObject basketball;
    public Rigidbody basketballRb;
    public XRGrabInteractable basketballGrabInteractable;
    public XRBaseInteractor hand;
    public float velocityMultiplier;
    public Transform controllerTransform;

    private bool startTracking;
    private Vector3 shootVelocity;
    private bool shoot;
    private Vector3 lastPosition;

    private List<Vector3> positions;
    private List<Vector3> velocities;
    //private UnityEngine.InputSystem.XR.XRController controller;

    // Start is called before the first frame update
    void Start()
    {
        startTracking = false;
        shoot = false;

        positions = new List<Vector3>();
        velocities = new List<Vector3>();

        lastPosition = Vector3.zero;

        //controller = UnityEngine.InputSystem.XR.XRController.rightHand;
    }

    // Update is called once per frame
    void Update()
    {

        if (shoot)
        {
            //basketballRb.position = ballPosition;
            //basketballRb.velocity = shootVelocity;
            basketballRb.AddForce(shootVelocity, ForceMode.VelocityChange);
            shootVelocity *= 0;

            positions.Clear();
            velocities.Clear();
            shoot = false;
        }

        hand.allowSelect = true;
        //Debug.Log($"Ball Acceleration (Y-Axis): {velocityCalculator.acceleration.y}");
    }

    private void FixedUpdate()
    {
        if (startTracking)
        {
            //Vector3 controllerVelocity = velocityCalculator.GetControllerVelocity();
            Vector3 controllerPosition = controllerTransform.position;

            //transform.TransformPoint(controllerPosition);
            //Vector3 controllerPosition = velocityCalculator.GetControllerPosition();
            //if (positions.Count == 0)
            //{
            //    positions.Add(controllerPosition);
            //}
            //Vector3 velocity;
            //if (positions.Count > 0 && positions.Last() != controllerPosition)
            //{
            //    velocity = (controllerPosition - positions.Last()) / (Time.deltaTime);
            //    velocities.Add(velocity);

            if (positions.Count > 0 && lastPosition.y > controllerPosition.y)
            {
                Debug.Log($"Did not add position: {controllerPosition}");
                ShootBall();
                Debug.Log("Stopped Tracking");
                startTracking = false;
                lastPosition = Vector3.zero;
            }
            else
            {
                //Debug.Log($"Position: {controllerPosition}");
                positions.Add(controllerPosition);
                lastPosition = controllerPosition;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (
            !startTracking &&
            basketballGrabInteractable.isSelected &&
            other.gameObject.name == "Main Camera"
            )
        {
            Debug.Log("Tracking Started");
            startTracking = true;
        }

        //if (startTracking)
        //{
        //    Debug.Log($"Ball Acceleration (Y-Axis): {velocityCalculator.acceleration.y}");
        //    if (velocityCalculator.acceleration.y < 0.0f)
        //    {
        //        startTracking = false;
        //        hand.allowSelect = false;
        //        Debug.Log("Ball released");
        //    }
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (startTracking &&
            basketballGrabInteractable.isSelected &&
            other.gameObject.name == "Main Camera")
        {
            Debug.Log("Tracking Stopped: Left Shooting Zone");
            startTracking = false;
        }
    }

    private void ShootBall()
    {
        var maxMagnitude = 0.0f;
        Vector3 lastPosition = Vector3.zero;
        bool first = true;
        int count = 1;
        foreach (var position in positions)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                if (lastPosition == position)
                {
                    count++;
                }
                else
                {
                    var velocity = (position - lastPosition) / (count * Time.fixedDeltaTime);
                    count = 1;
                    velocities.Add(velocity);
                
                    if (velocity.sqrMagnitude > maxMagnitude)
                    {
                        maxMagnitude = velocity.sqrMagnitude;
                        shootVelocity = velocity;
                    }
                }
            }

            lastPosition = position;
        }

        //shootVelocity.z = velocities.Average();
        Debug.Log($"Shoot released at: {shootVelocity}");
        hand.allowSelect = false;
        shoot = true;
    }
}
