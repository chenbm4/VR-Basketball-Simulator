using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShootingManager : MonoBehaviour
{
    public ShootingTrigger shootingTrigger;
    public BoxCollider shootingZone;
    public XRGrabInteractable basketballGrabInteractable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (basketballGrabInteractable.isSelected)
        {
            shootingTrigger.StartTracking();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.name == shootingZone.name && basketballGrabInteractable.isSelected)
        //{
        //    shootingTrigger.StartTracking();
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == shootingZone.name && basketballGrabInteractable.isSelected)
        {
            shootingTrigger.StopTracking();
        }
    }
}
