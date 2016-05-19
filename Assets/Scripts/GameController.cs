using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SocketIO;

public class GameController : MonoBehaviour {

	public Player playerGameObj;
	public SocketIOComponent socketIO;
	Player currentPlayer;

	void Start () {
		GameObject player = GameObject.Instantiate (playerGameObj.gameObject, playerGameObj.position, Quaternion.identity) as GameObject;
		currentPlayer = player.GetComponent<Player> ();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
