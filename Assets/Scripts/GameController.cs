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
		socketIO.On ("USER_CONNECTED", OnUserConnected);
		socketIO.On("USER_DISCONNECTED", OnUserDisconnected );

		GameObject player = GameObject.Instantiate (playerGameObj.gameObject, playerGameObj.position, Quaternion.identity) as GameObject;
		currentPlayer = player.GetComponent<Player> ();

		StartCoroutine( "CalltoServer" );
	}

	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator CalltoServer(){
		yield return new WaitForSeconds(1f);
		Debug.Log("Send join room message to the server");
		socketIO.Emit("JOIN_ROOM");
	}

	void OnUserDisconnected (SocketIOEvent obj) {
		Debug.Log ("Desconnect from server.");
	}

	void OnUserConnected (SocketIOEvent obj) {
		Debug.Log ("Joining Room.");
	}
}
