using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    public ShootingTrigger shootingTrigger;
    public BoxCollider shootingZone;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == shootingZone.name)
        {
            shootingTrigger.StartTracking();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == shootingZone.name)
        {
            shootingTrigger.StopTracking();
        }
    }
}
