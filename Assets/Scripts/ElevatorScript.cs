using UnityEngine;
using System.Collections;

public class ElevatorScript : MonoBehaviour {
    public bool isActivated = false;

    public Transform end;
    public Transform[] nodes;
    public float speed;
    private float startTime;
 
	// Use this for initialization
	void Start () {
        startTime = Time.time;
        speed = 40;
	}
	
	// Update is called once per frame
	void Update () {

        // Just checks if the mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            isActivated = true;
            Debug.Log("Mouse pressed!");
        }

        if (isActivated)
        {
            //float step = speed * Time.deltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, end.position, step);
            transform.position = Vector3.Lerp(transform.position, end.transform.position, (Time.time - startTime) / speed);
        }
        
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(end.position, new Vector3(0.5f, 0.5f, 0.5f));
    }
}
