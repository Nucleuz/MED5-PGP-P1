using UnityEngine;
using System.Collections;

/**
  * Create by Jalict, larsen.frans@gmail.com
 **/

public class Vine : MonoBehaviour {
	[Header("Nodes")]
	public GameObject nodeA;		// Points that should connect the vines
	public GameObject nodeB;
	[Header("Attachment")]
	public Transform transformA;
	public Transform transformB;
	[Header("Settings")]
	[Range(1f, 100f)]
	public float lineLength;		// length 1 = perfect length between the points
	[Range(0.1f, 1f)]
	public float lineWidth;			// width of line
	public int resolution;			// How many extra points should be added TODO added according to length
	public float splittingDistance;	// Length to move in order to snap
	[Header("Assets")]
	public Material[] lineMaterials;	// Material for line
	public GameObject snappingParticles;// Particles to be Create when they snap

	private LineRenderer lineA;
	private LineRenderer lineB;
	private GameObject[] rigidPoints;
	private int numOfPoints;
	private int splittingIndex;
	private bool isSplit;
	private GameObject extraPoint;

	void Start () {
		// Calculate last things
		numOfPoints = resolution + 2;
		splittingIndex = Random.Range((int)((numOfPoints/2) - (numOfPoints/4)), (int)((numOfPoints/2) + (numOfPoints/4)));
		splittingDistance += Vector3.Distance(nodeA.transform.position, nodeB.transform.position);

		// Setup line renderer
		lineA = nodeA.AddComponent<LineRenderer>();
		lineB = nodeB.AddComponent<LineRenderer>();
		lineA.SetVertexCount(splittingIndex + 1);
		lineB.SetVertexCount(numOfPoints - splittingIndex);

		// Make copy of material and tile it according to length of vine (Approx.)
		Material mat = new Material(Shader.Find("Unlit/Transparent"));
		mat.CopyPropertiesFromMaterial(lineMaterials[(int)Random.Range(0, lineMaterials.Length)]);
		mat.mainTextureScale = new Vector2(Vector3.Distance(nodeA.transform.position, nodeB.transform.position) * 3, 1);
		lineA.material = mat;
		lineB.material = mat;

		// Instantiate Rigid points between the nodes
		rigidPoints = new GameObject[numOfPoints];	// resolution + node A & B
		rigidPoints[0] = nodeA;
		rigidPoints[numOfPoints-1] = nodeB;
		for(int i = 1; i < numOfPoints-1; i++) {
			// Instantiate Object and its position
			rigidPoints[i] = new GameObject();
			rigidPoints[i].name = "RigidPoint_"+i;
			rigidPoints[i].transform.position = Vector3.Lerp(nodeA.transform.position, nodeB.transform.position, (1f / (numOfPoints-1)) * i);
			rigidPoints[i].transform.parent = transform;

			// Add components
			rigidPoints[i].AddComponent<Rigidbody>();
			rigidPoints[i].AddComponent<SpringJoint>();
			rigidPoints[i].GetComponent<SpringJoint>().connectedBody = rigidPoints[i-1].GetComponent<Rigidbody>();
			rigidPoints[i].GetComponent<SpringJoint>().spring = 512f;
			rigidPoints[i].GetComponent<SpringJoint>().damper = 1f;
			rigidPoints[i].GetComponent<SpringJoint>().minDistance = 0.00125f;
			rigidPoints[i].GetComponent<SpringJoint>().maxDistance = 0.005f;

		}
		nodeA.GetComponent<FixedJoint>().connectedBody = rigidPoints[1].GetComponent<Rigidbody>();
		nodeB.GetComponent<FixedJoint>().connectedBody = rigidPoints[numOfPoints-2].GetComponent<Rigidbody>();

		if(transformA)
			nodeA.transform.parent = transformA;
		if(transformB)
			nodeB.transform.parent = transformB;

		isSplit = false;
	}

	void FixedUpdate () {
		if(Vector3.Distance(nodeA.transform.position, nodeB.transform.position) > splittingDistance && !isSplit) {
			Split();
		}

		lineA.SetWidth(lineWidth, lineWidth);
		lineB.SetWidth(lineWidth, lineWidth);

		for(int i = 0; i <= splittingIndex;i++) {
			if(!isSplit) {
				lineA.SetPosition(i, rigidPoints[i].transform.position);
			} else {
				lineA.SetPosition(i, i != splittingIndex ? rigidPoints[i].transform.position : extraPoint.transform.position);
			}
		}

		for(int i = 0; i < numOfPoints - splittingIndex;i++) {
			lineB.SetPosition(i, rigidPoints[i + splittingIndex].transform.position);
		}
	}

	void OnDrawGizmos() {
		// Draw Gizmos for Node A, B
		Gizmos.color = Color.cyan;
		Gizmos.DrawSphere(nodeA.transform.position, 0.1f);
		Gizmos.DrawSphere(nodeB.transform.position, 0.1f);

		if(rigidPoints != null) {
			Gizmos.color = Color.red;
			for (int i = 1; i < rigidPoints.Length-1; i++) {
				if(i == splittingIndex) {
					Gizmos.color = Color.black;
				} else {
					Gizmos.color = Color.red;
				}
				Vector3 v = rigidPoints[i].transform.position;
				Gizmos.DrawSphere(v, 0.025f);
			}
		}
		if(extraPoint != null) {
			Gizmos.color = Color.black;
			Gizmos.DrawSphere(extraPoint.transform.position, 0.025f);
		}
	}

	public void Split() {
		isSplit = true;

		Destroy(rigidPoints[splittingIndex].GetComponent<SpringJoint>());

		extraPoint = new GameObject();
		extraPoint.transform.position = rigidPoints[splittingIndex].transform.position;
		extraPoint.name = "RigidPoint_extra";
		extraPoint.transform.parent = transform;
		extraPoint.AddComponent<SpringJoint>();
		extraPoint.GetComponent<SpringJoint>().connectedBody = rigidPoints[splittingIndex-1].GetComponent<Rigidbody>();
		extraPoint.GetComponent<SpringJoint>().spring = 512f;
		extraPoint.GetComponent<SpringJoint>().damper = 1f;
		extraPoint.GetComponent<SpringJoint>().minDistance = 0.00125f;
		extraPoint.GetComponent<SpringJoint>().maxDistance = 0.005f;

		lineB.SetVertexCount(numOfPoints - splittingIndex);

		GameObject p = Instantiate(snappingParticles, extraPoint.transform.position, Quaternion.identity) as GameObject;
		p.transform.parent = transform;
	}
}
