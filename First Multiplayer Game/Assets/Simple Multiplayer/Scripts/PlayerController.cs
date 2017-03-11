using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	private float x;
	private float z;

	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	private bool cameraHasBeenAssigned = false;

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			Camera.main.transform.parent = transform;
			cameraHasBeenAssigned = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) {return;}

		if (!cameraHasBeenAssigned) {
			Camera.main.transform.parent = transform;
			cameraHasBeenAssigned = true;
		}
			
		x = Input.GetAxis ("Horizontal") * Time.deltaTime * 150.0f;
		z = Input.GetAxis ("Vertical") * Time.deltaTime * 3.0f;
		transform.Rotate (0, x, 0);
		transform.Translate (0, 0, z);

		if (Input.GetKeyDown (KeyCode.Space)) {
			CmdFire ();
		}
	}

	void LateUpdate(){
		//Camera.main.transform.position = transform.position + offset;
		//Camera.main.transform.rotation = Quaternion.Euler (transform.rotation.x, transform.rotation.y, transform.rotation.z);
	}
		
	[Command]
	void CmdFire(){		// run on the server
		// create bullet
		GameObject bullet = (GameObject)Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

		// Add velocity
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6.0f;

		// Spawn bullet on client
		NetworkServer.Spawn(bullet);

		// Destroy bullet after 2 seconds
		Destroy(bullet, 2);
	}

	public override void OnStartLocalPlayer(){
		GetComponent<MeshRenderer> ().material.color = Color.blue;
	}
}
