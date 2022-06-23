using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SecondScript : MonoBehaviour
{
    public GameObject clock;
    int knownSecond;

    // Start is called before the first frame update
    void Start()
    {
        knownSecond = DateTime.Now.Second;
        clock.transform.Rotate((float)(knownSecond * 6), 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        int currentSecond = DateTime.Now.Second;
        if (currentSecond != knownSecond)
        {
            clock.transform.Rotate(6.0f, 0.0f, 0.0f);
            knownSecond = currentSecond;
        }
    }
}
