using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DropBall : MonoBehaviour
{
    public BoxCollider shootingZone;
    public GameObject basketball;
    public XRGrabInteractable basketballGrabInteractable;
    public XRBaseInteractor hand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hand.allowSelect = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (basketballGrabInteractable.isSelected && other.gameObject.name == "Main Camera")
        {
            //Debug.Log($"{OVRInput.GetLocalControllerAcceleration((OVRInput.Controller.RTouch))}");
            //Debug.Log("Drop Ball");
            //hand.allowSelect = false;
        }
    }
}
