using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public GameObject player;
	private Vector3 camPosition;

	void Start () {
	
	}

	void Update () {
		camPosition = new Vector3 (player.transform.position.x - 0.5f, player.transform.position.y + 6, player.transform.position.z - 6);
		transform.position = camPosition;
	}
}
