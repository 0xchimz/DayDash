using UnityEngine;
using System.Collections;
using SocketIO;

public class Player : MonoBehaviour {

	public string playerName;
	public Vector3 position;
	public string id;
	public string mapType;

	public GameObject key; 
	private SocketIOComponent socket;

	void Start () {
		this.name = playerName;

		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();

		socket.On("aKey", FoundKey);
	}

	void OnTriggerEnter (Collider gameElement)
	{
		if (gameElement.tag == "Key") {
			Debug.Log ("Player got key");
			socket.Emit ("aKey");
			key.active = false;
		
		}
			
	}

	public void FoundKey(SocketIOEvent e)
	{
		Debug.Log("Hey everyone, I found a key!");
	}

}
