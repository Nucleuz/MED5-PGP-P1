using UnityEngine;
using System.Collections;

public class ConnectAfterAnimation : MonoBehaviour {

    public RailConnection[] railConnections;
    private Animator anim;
    float timeStamp;
    public bool isStarted;
    public float connectAfter;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        isStarted = false;
        
    }
	
	// Update is called once per frame
	void Update () {

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Open") && !isStarted)
        {
            timeStamp = Time.time + connectAfter;
            isStarted = true;
        }

        if (Time.time > timeStamp && isStarted)
        {
            for(int i = 0;i<railConnections.Length;i++){
                railConnections[i].connectToNext = true;
            }
            for(int i = 0;i<railConnections.Length;i++){
                railConnections[i].connectToPrev = true;
            }
        }
    }
}
