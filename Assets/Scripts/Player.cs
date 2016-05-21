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
	}

	void OnTriggerEnter (Collider gameElement)
	{
		if (gameElement.tag == "Key") {
			Debug.Log ("Player got key");
			key.active = false;
		
		}
			
	}

}
