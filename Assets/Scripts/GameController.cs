using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SocketIO;

public class GameController : MonoBehaviour {

	public SocketIOComponent socketIO;
	private Room room;

	void Start () {
		socketIO.On ("USER_CONNECTED", onUserConnected);
		socketIO.On ("JOIN_RESPONSE", onJoined);
		socketIO.On ("ROOM", onRoom);
		socketIO.On ("USER_DISCONNECTED", onUserDisconnected );

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

	void onRoom (SocketIOEvent obj){
		Debug.Log ("onRoom");
		Debug.Log (obj);
	}

	void onJoined (SocketIOEvent obj) {
		Debug.Log ("On Joined");
		JSONObject response = obj.data.GetField ("roomInfo");
		string status = response.GetField ("status").ToString();
		string no = response.GetField ("no").ToString();
		string level = response.GetField ("level").ToString();
		room = new Room (no, level, status);
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