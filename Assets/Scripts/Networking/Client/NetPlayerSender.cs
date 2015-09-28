using UnityEngine;
using System.Collections;

using DarkRift;
public class NetPlayerSender : MonoBehaviour {



	Quaternion lastRotation;
	Vector3 lastPosition;

	public float speed;
	public float strafeSpeed;
	Rigidbody rigidbody;


	void Start(){

		//Also, make sure we're told if a player disconnects.
		DarkRiftAPI.onPlayerDisconnected += PlayerDisconnected;
		rigidbody = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update () {
		Vector3 c = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		c.y = transform.position.y;
		transform.LookAt(c);

		float v = Input.GetAxis("Vertical");
		float h = Input.GetAxis("Horizontal");

		rigidbody.AddForce(transform.forward * v * Time.deltaTime * speed);
		rigidbody.AddForce(transform.right * h * Time.deltaTime * strafeSpeed);


			//Only send data if we're connected and we own this player
		if( DarkRiftAPI.isConnected){
			//We're going to use a tag of 1 for movement messages
			//If we're conencted and have moved send our position with subject 0.
			if( transform.rotation != lastRotation || transform.position != lastPosition){
				PlayerInfo info = new PlayerInfo();
				info.position = new SVector3(transform.position);
				info.rotation = new SQuaternion(transform.rotation);

				DarkRiftAPI.SendMessageToOthers(Network.Tag.Player, Network.Subject.PlayerUpdate, info);
			}

			//Update stuff
			lastPosition = transform.position;
			lastRotation = transform.rotation;
		}
	}
	void PlayerDisconnected(ushort ID){
		Destroy(gameObject);
	}
}
