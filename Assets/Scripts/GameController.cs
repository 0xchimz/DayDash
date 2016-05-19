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
		socketIO.On ("USER_CONNECTED", onUserConnected);
		socketIO.On ("USER_ROLE", onUserRole);
		socketIO.On ("USER_DISCONNECTED", onUserDisconnected );

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

	void onUserRole (SocketIOEvent obj) {
		string response = JsonToString (obj.data.GetField ("role").ToString (), "\"");
		playerGameObj.role = response;
	}

	void onUserDisconnected (SocketIOEvent obj) {
		Debug.Log ("Desconnect from server.");
	}

	void onUserConnected (SocketIOEvent obj) {
		Debug.Log ("Joining Room.");
	}

	string JsonToString( string target, string s){
		string[] newString = Regex.Split(target,s);
		return newString[1];
	}
}