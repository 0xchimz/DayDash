using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour {

	public GameObject player;
	private Vector3 position;

	void Update () {
		position = new Vector3 (player.transform.position.x, player.transform.position.y + 5f, player.transform.position.z);
		transform.position = position;
	}
}
