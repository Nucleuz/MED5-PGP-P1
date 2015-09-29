using UnityEngine;
using System.Collections;
using DarkRift;

/*
By KasperHdL

Receiver for another player

*/

public class NetPlayerSender : MonoBehaviour {

	//reference to reduce when it sends data to everyone else
	Quaternion lastRotation;
	Vector3 lastPosition;

	//for testing some speed variables 
	public float speed;
	public float strafeSpeed;

	//ref
	Rigidbody rigidbody;


	void Start(){
		DarkRiftAPI.onPlayerDisconnected += PlayerDisconnected;
		rigidbody = GetComponent<Rigidbody>();
	}

	void Update () {
		//get the mouse position in the world
		Vector3 c = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//set y to be on the some level as the player
		c.y = transform.position.y;
		//look at the position
		transform.LookAt(c);

		//get input
		float v = Input.GetAxis("Vertical");
		float h = Input.GetAxis("Horizontal");

		//apply forces
		rigidbody.AddForce(transform.forward * v * Time.deltaTime * speed);
		rigidbody.AddForce(transform.right * h * Time.deltaTime * strafeSpeed);

		if( DarkRiftAPI.isConnected){
			//has the rotation or position changed since last sent message
			if( transform.rotation != lastRotation || transform.position != lastPosition){
				//pack player infomation
				PlayerInfo info = new PlayerInfo();
				info.position = new SVector3(transform.position);
				info.rotation = new SQuaternion(transform.rotation);

				//send it to everyone else
				DarkRiftAPI.SendMessageToOthers(Network.Tag.Player, Network.Subject.PlayerUpdate, info);
			}

			//save the sent position and rotation
			lastPosition = transform.position;
			lastRotation = transform.rotation;
		}
	}

	//When the player disconnects destroy it
	void PlayerDisconnected(ushort ID){
		Destroy(gameObject);
	}
}
