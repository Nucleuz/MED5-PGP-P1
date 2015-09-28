using UnityEngine;
using System.Collections;

public class Mirror : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ReflectRay(Vector3 originPos, RaycastHit hit){
		Vector3 incVec = hit.point - originPos;
		Vector3 reflectVec = Vector3.Reflect(incVec, hit.normal);
		Debug.DrawLine(originPos, hit.point, Color.green);
		Debug.DrawRay(hit.point, reflectVec, Color.blue);

		Ray ray = new Ray(hit.point, reflectVec);

		RaycastHit mirHit;

		// Casts a ray against all colliders
        if (Physics.Raycast(ray, out mirHit)) {
            // Declaring objectHit to be the object that the ray hits
            Transform objectHit = mirHit.transform;

            // Checks if we hit an object with the "Button" tag.
            if (objectHit.tag == "Button") {
                // Activates the isActivated boolean in the script. (I've just used a test script, so this needs to be corrected when we have the right objects with the right scripts)
                objectHit.GetComponent<TestButtonScript>().isActivated = true;

                // Draws the ray (nice to have as a visual representation)
                Debug.DrawRay(ray.origin, ray.direction * 10, Color.cyan);
            } else if(objectHit.tag == "Mirror"){
            	objectHit.GetComponent<Mirror>().ReflectRay(hit.point, mirHit);
            }
        }
	}

}
