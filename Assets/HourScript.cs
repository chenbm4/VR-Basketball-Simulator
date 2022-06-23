using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HourScript : MonoBehaviour
{
    public GameObject clock;
    int knownSecond;

    // Start is called before the first frame update
    void Start()
    {
        int currentHour = DateTime.Now.Hour;
        int currentMinute = DateTime.Now.Minute;
        knownSecond = DateTime.Now.Second;
        clock.transform.Rotate((float)((currentHour * 30) + (currentMinute * 0.5) + (knownSecond * (1 / 120))), 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        int currentSecond = DateTime.Now.Second;
        if (currentSecond != knownSecond)
        {
            clock.transform.Rotate((float)(1 / 120), 0.0f, 0.0f);
            knownSecond = currentSecond;
        }
    }
}
