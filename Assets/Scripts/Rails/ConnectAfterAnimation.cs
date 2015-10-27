﻿using UnityEngine;
using System.Collections;

public class ConnectAfterAnimation : MonoBehaviour {

    public RailConnection[] railConnections;
    private Animator anim;
    float timeStamp;
    public bool isStarted;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        isStarted = false;
        
    }
	
	// Update is called once per frame
	void Update () {

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Open") && !isStarted)
        {
            timeStamp = Time.time + anim.GetCurrentAnimatorStateInfo(0).length;
            isStarted = true;
        }

        if (Time.time > timeStamp && isStarted)
        {
            for(int i = 0;i<railConnections.Length;i++)
                railConnections[i].connectToPrev = true;
        }
    }
}
