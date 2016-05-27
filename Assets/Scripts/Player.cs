using UnityEngine;
using System.Collections;
using SocketIO;
using RAIN.Navigation;
using RAIN.Navigation.Targets;

public class Player : MonoBehaviour {
	
	public Vector3 position;
	public string id;

	private SocketIOComponent socket;

	void Start () {

		GameObject go = GameObject.Find ("SocketIO");
		socket = go.GetComponent<SocketIOComponent> ();

		gameObject.GetComponentInChildren<NavigationTargetRig> ().Target.MountPoint = gameObject.transform;
		gameObject.GetComponentInChildren<NavigationTargetRig> ().Target.TargetName = "NavTarget";
	}

	void Update () {
		Debug.Log ((transform.position.y < -10.0));
		if (transform.position.y < -10.0f) {
			GameController.instance.playerDead ();
		}
	}

	void OnCollisionEnter (Collision e){
		if (e.gameObject.tag == "Door") {
			socket.Emit ("PLAYER_ENTER_DOOR");
		}
	}

	void OnCollisionExit (Collision e){
		if (e.gameObject.tag == "Door") {
			socket.Emit ("PLAYER_EXIT_DOOR");
		}
	}

	void OnTriggerEnter (Collider gameElement) {
		if (gameElement.tag == "Key") {
			Debug.Log ("Player got key");
			gameElement.gameObject.SetActive (false);
			socket.Emit ("FOUND_KEY");
		}
		if (gameElement.tag == "Door") {
			Debug.Log ("Player on the door");
			gameElement.gameObject.SetActive (false);
		}
	}

	public void FoundKey (SocketIOEvent e) {
		Debug.Log ("Hey everyone, I found a key!");
		socket.Emit ("aKey");
	}

}
