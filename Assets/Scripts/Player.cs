using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public string playerName;
	public Vector3 position;
	public string id;
	public string mapType;

	public GameObject key; 

	void Start () {
		this.name = playerName;
	}

	void OnTriggerEnter (Collider gameElement)
	{
		//Debug.Log ("Player got key");
		if (gameElement.tag == "Key") {
			Debug.Log ("Player got key");
			key.active = false;
		
		}


	}

}
