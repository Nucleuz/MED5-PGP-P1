using UnityEngine;
using System.Collections;

public class Utilities : MonoBehaviour {


	//Creates a ray for the helmet light. Calculates reflection as well.
	public static Vector3 RayChecker(Ray ray, RaycastHit hit, LineRenderer linRend, int ind){
		//LineRenderer lR = Camera.main.GetComponent<LineRenderer>();

        if (Physics.Raycast(ray, out hit)) {
            // Declaring objectHit to be the object that the ray hits
            Transform objectHit = hit.transform;

            // Checks if we hit an object with the "Button" tag.
            if (objectHit.tag == "Button") {
                // Activates the isActivated boolean in the script. (I've just used a test script, so this needs to be corrected when we have the right objects with the right scripts)
                objectHit.GetComponent<TestButtonScript>().isActivated = true;

                // Draws the ray (nice to have as a visual representation)
                Debug.DrawRay(ray.origin, ray.direction * 10, Color.cyan);
            } else if(objectHit.tag == "Mirror"){
                objectHit.GetComponent<Mirror>().ReflectRay(ray.origin, hit, linRend, ind+1);
            }

        }
        return hit.point;
	}
}
