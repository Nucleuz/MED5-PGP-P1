using UnityEngine;
using UnityEditor;
using System.Collections;

public class RailConstructorInterface : EditorWindow {
	private RailConstructor construct;

	[MenuItem ("Window/Rail Constructor")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		RailConstructorInterface window = (RailConstructorInterface)EditorWindow.GetWindow (typeof (RailConstructorInterface));
		window.Show();
	}

	void OnGUI () {
		if(Selection.activeGameObject) {
			if(Selection.activeGameObject.GetComponent<RailConstructor>() != null)
				construct = Selection.activeGameObject.GetComponent<RailConstructor>();
			else if(Selection.activeGameObject.transform.parent.GetComponent<RailConstructor>() != null)
				construct = Selection.activeGameObject.transform.parent.GetComponent<RailConstructor>();
			else
				return;

			GUILayout.Label ("Placeable Objects", EditorStyles.boldLabel);
			foreach(var v in construct.placeableObjects) {
				if(GUILayout.Button(v.name)) {
					construct.PlaceObject(v);
				}
			}

			GUILayout.Label("Utilities", EditorStyles.boldLabel);
			if(GUILayout.Button("Delete Last Placed")) {
				construct.DeleteLastPlaced();
			}
			if(GUILayout.Button("Delete First Placed")) {
				construct.DeleteFirstPlaced();
			}
		} else {
			GUILayout.Label ("No Rail System Selected!", EditorStyles.boldLabel);
		}
	}

}
