using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public Player playerGameObj;

	// Use this for initialization
	void Start () {
		GameObject player = GameObject.Instantiate (playerGameObj.gameObject, playerGameObj.position, Quaternion.identity) as GameObject;
		Player playerCom = player.GetComponent<Player> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
