using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem.XR;

public class VelocityCalculator : MonoBehaviour
{
    private XRController rightHand;
    public Transform rim;
    //private XRNode rightXRNode = XRNode.RightHand;
    //private List<InputDevice> controllers = new List<InputDevice>();

    // Start is called before the first frame update
    void Start()
    {
        rightHand = XRController.rightHand;
    }

    //void GetDevice()
    //{
    //    rightHand = XRController.rightHand;
    //}

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 GetControllerPosition()
    {
        Vector3 positionValue;
        //if (rightHand.TryGetFeatureValue(CommonUsages.devicePosition, out positionValue))
        //{
        //    return positionValue;
        //}


        positionValue = rightHand.devicePosition.ReadValue();

        //Debug.Log("Failed to get controller position value.");
        return positionValue;
    }
}

    //public Vector3 GetControllerVelocity()
    //{
    //    //if (!rightHand.isValid)
    //    //{
    //    //    GetDevice();
    //    //}

    //    Vector3 velocityValue;
    //    //if (rightHand.TryGetFeatureValue(CommonUsages.deviceVelocity, out velocityValue))
    //    //{
    //    //    return velocityValue;
    //    //}

    //    velocityValue = Unity.XR.Oculus.Input.OculusTouchController.rightHand.deviceVelocity;

    //    Debug.Log("Failed to get controller velocity value.");
    //    return velocityValue;
    //}

//    public Vector3 GetControllerAcceleration()
//    {
//        if (!rightHand.isValid)
//        {
//            GetDevice();
//        }

//        Vector3 accelerationValue;
//        if (rightHand.TryGetFeatureValue(CommonUsages.deviceVelocity, out accelerationValue))
//        {
//            return accelerationValue;
//        }

//        Debug.Log("Failed to get controller velocity value.");
//        return accelerationValue;
//    }
//}
