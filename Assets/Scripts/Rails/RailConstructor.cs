using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RailConstructor : MonoBehaviour {
	[Header("Reference Objects")]
	public GameObject currentObject;

	[Header("Placeable Objects")]
	public GameObject[] placeableObjects;

	public void PlaceObject(GameObject obj) {
		if(currentObject == null) {
			currentObject = Instantiate(obj, transform.position, transform.rotation) as GameObject;
			currentObject.transform.parent = transform;
			currentObject.name = "0 - " + obj.name;

			currentObject.transform.FindChild("StartAttachment").gameObject.AddComponent<Rail>();
		} else {
			GameObject placed = Instantiate(obj, Vector3.one, Quaternion.identity) as GameObject;
			placed.name = transform.childCount + " - " + obj.name;

			GameObject startAttachment = placed.transform.FindChild("StartAttachment").gameObject;
			startAttachment.transform.parent = null;
			placed.transform.parent = startAttachment.transform;

			GameObject endAttachment = currentObject.transform.FindChild("EndAttachment").gameObject;
			startAttachment.transform.position = endAttachment.transform.position;
			startAttachment.transform.rotation = endAttachment.transform.rotation;

			placed.transform.parent = transform;
			startAttachment.transform.parent = placed.transform;

			Rail rail = placed.transform.FindChild("StartAttachment").gameObject.AddComponent<Rail>();			//NOTE I only put Rail component on the starting nodes as it suffices
			Rail currentRail = currentObject.transform.FindChild("StartAttachment").gameObject.GetComponent<Rail>();
			rail.prev = currentRail.next == null ? currentRail : currentRail.next;
			rail.prev.next = rail;

			if(placed.transform.FindChild("Middle")) {
				Rail middle = placed.transform.Find("Middle").gameObject.AddComponent<Rail>() as Rail;
				rail.next = middle;
				middle.prev = rail;
			}

			currentObject = placed;
		}
	}

	public void DeleteLastPlaced() {
		if(!(transform.childCount <= 0)) {
			DestroyImmediate(transform.GetChild(transform.childCount - 1).gameObject);
			if(!(transform.childCount <= 0))
				currentObject = transform.GetChild(transform.childCount - 1).gameObject;
			else
				currentObject = null;
		}
	}

	public void DeleteFirstPlaced() {
		if(!(transform.childCount <= 0)) {
			DestroyImmediate(transform.GetChild(0).gameObject);
		}
	}
}
