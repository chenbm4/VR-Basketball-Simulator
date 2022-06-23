using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MinuteScript : MonoBehaviour
{
    public GameObject clock;
    int knownSecond;

    // Start is called before the first frame update
    void Start()
    {
        int currentMinute = DateTime.Now.Minute;
        knownSecond = DateTime.Now.Second;
        clock.transform.Rotate((float)((currentMinute * 6) + (knownSecond * 0.1)), 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        int currentSecond = DateTime.Now.Second;
        if (currentSecond != knownSecond)
        {
            clock.transform.Rotate(0.1f, 0.0f, 0.0f);
            knownSecond = currentSecond;
        }
    }
}
