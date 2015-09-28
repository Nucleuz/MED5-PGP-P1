using UnityEngine;
using System.Collections;

public class Utilities : MonoBehaviour {


	//Creates a ray for the helmet light + reflection rays
	public static void RayChecker(Ray ray, LineRenderer linRend, int i){

		RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            // Declaring objectHit to be the object that the ray hits
            Transform objectHit = hit.transform;

			// Updates the line renderer vertecies
			linRend.SetVertexCount(++i);
			linRend.SetPosition(i-1, hit.point);
			Debug.DrawLine(ray.origin, hit.point, Color.cyan); // for debug and see direction of ray

            // Checks if we hit an object with the "Button" tag.
            if (objectHit.tag == "Button") {
                // Activates the isActivated boolean in the script. (I've just used a test script, so this needs to be corrected when we have the right objects with the right scripts)
                objectHit.GetComponent<TestButtonScript>().isActivated = true;
            } 
			else if(objectHit.tag == "Mirror"){
				Ray newRay = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal)); 	// makes new reflection ray from mirror
				Utilities.RayChecker(newRay, linRend, i); 										// loops function to check if if the ray hits something
            }
        }
		else{
			// draw ray even though it doesnt hit anything
			linRend.SetVertexCount(++i);
			linRend.SetPosition(i-1, ray.origin + (ray.direction * 10));
			Debug.DrawRay(ray.origin, ray.direction * 10, Color.cyan);
		}
	}
}
