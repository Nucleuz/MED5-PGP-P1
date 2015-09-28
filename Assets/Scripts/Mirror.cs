using UnityEngine;
using System.Collections;

public class Mirror : MonoBehaviour {

	//private LineRenderer linRend;

	// Use this for initialization
	void Start () {
		/*
		linRend = gameObject.AddComponent<LineRenderer>();
		linRend.SetWidth(0.1f,0.1f);
		*/
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (Input.GetKey("space")) {
			linRend.enabled = true;
		} else {
			linRend.enabled = false;
		}
		*/
	}

	public void ReflectRay(Vector3 originPos, RaycastHit orHit, LineRenderer linRend, int ind){
		Vector3 incVec = orHit.point - originPos;
		Vector3 reflectVec = Vector3.Reflect(incVec, orHit.normal);
		Debug.DrawLine(originPos, orHit.point, Color.green);
		Debug.DrawRay(orHit.point, reflectVec, Color.blue);


		Ray ray = new Ray(orHit.point, reflectVec);

		RaycastHit hit = new RaycastHit();

		Vector3 hitPoint = Utilities.RayChecker(ray, hit, linRend, ind);

		linRend.SetVertexCount(ind+2);

        linRend.SetPosition(ind, ray.origin);
        linRend.SetPosition(ind+1, hitPoint);
	}

}
