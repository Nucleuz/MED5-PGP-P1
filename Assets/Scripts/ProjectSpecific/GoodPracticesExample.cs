using UnityEngine;
using System.Collections;

/*
by KasperHdL
v 0.1

This is an example script of some of the possible things you can do to change the way your script behaves in the inspector


*/


//this shows a little ? icon and open the following URL
[HelpURL("http://example.com/docs/MyComponent.html")]

[AddComponentMenu("GoodPractices")] //can be used so that everyone easily can add this from the component menu
[RequireComponent(typeof(TextMesh))] //requires the component and automatically adds it when adding this script to an object AND prohibits removal of component that is stated to be required


public class GoodPracticesExample : MonoBehaviour {
	[Header("Variables can be categories")]
	public GameObject someVar;

	[HideInInspector]
	public float thisMustNotBeChangedInTheInspector;
	[Header("other variables")]

	[Range(0,1)]
	public float range; 

	[Space(10)]
    [Tooltip("a Tool Tip!")]
	public int clickOnTheVarText;

	[Space(40)]
	[Header("Space above and below whatever ")]
	[Space(40)]

	[TextAreaAttribute(1,2)]
	public string multiLineString;

	[Space(20)]

	[ContextMenuItem("Reset", "Reset")] //add extra functionality on the right click context menu
    [Multiline(2)] //multline string 
 	public string rightClickHere = "";

 	//special function that is run when selected from the context menu
    void Reset() {
		//not sure to what extent this can be used

        rightClickHere = "";
		Debug.Log("stuff happens");
    }

}
