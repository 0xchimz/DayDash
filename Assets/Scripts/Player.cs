using UnityEngine;
using System.Collections;
using SocketIO;

public class Player : MonoBehaviour {
	
	public Vector3 position;
	public string id;

	private SocketIOComponent socket;

	void Start () {

		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
	}

	void OnTriggerEnter (Collider gameElement)
	{
		if (gameElement.tag == "Key") {
			Debug.Log ("Player got key");
			gameElement.gameObject.SetActive(false);
			socket.Emit ("FOUND_KEY");
		}
		if (gameElement.tag == "Door") {
			Debug.Log ("Player on the door");
			gameElement.gameObject.SetActive(false);
		}	
	}

	public void FoundKey(SocketIOEvent e)
	{
		Debug.Log("Hey everyone, I found a key!");
		socket.Emit ("aKey");
	}

}
