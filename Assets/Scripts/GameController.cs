using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SocketIO;

public class GameController : MonoBehaviour {

	public readonly int CONNECTING = 0, CONNECTED = 1, FINDING_MATCH = 2, JOINED = 3, STARTING = 4;

	public SocketIOComponent socket;

	private Room room;
	private int statusGame;

	public Text status;

	void Start () {
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();

		socket.On ("USER_CONNECTED", onUserConnected);
		socket.On ("JOIN_RESPONSE", onJoined);
		socket.On ("ROOM", onRoom);
		socket.On ("USER_DISCONNECTED", onUserDisconnected );

		socket.On ("error", onError);
		socket.On ("connect", onUserConnected);

		statusGame = CONNECTING;

		status.text = "CONNECTING TO SERVER...";
	}

	// Update is called once per frame
	void Update () {
		if (statusGame == CONNECTING) {
			status.text = "CONNECTING TO SERVER...";
		} else if (statusGame == FINDING_MATCH) {
			status.text = "FINDING MATCH...";
		} else if (statusGame == JOINED) {
			status.text = "JOINED, WAIT OTHER PLAYER...";
		} else if (statusGame == STARTING) {
			status.text = "GAME IS STARTING...";
		}
	}

	void onError(SocketIOEvent e){
		Debug.Log ("Connect error received: " + e.name + " " + e.data);
	}

	void FindingMatch(){
		Debug.Log("Finding match");
		socket.Emit("JOIN_ROOM");
	}

	void onDisconnect (SocketIOEvent obj){
		Debug.Log ("Disconnect");
	}

	void onRoom (SocketIOEvent obj){
		statusGame = STARTING;
		Debug.Log ("onRoom");
		Debug.Log (obj);
	}

	void onJoined (SocketIOEvent obj) {
		statusGame = JOINED;
		Debug.Log ("Joined");
		JSONObject response = obj.data.GetField ("roomInfo");
		string status = response.GetField ("status").ToString();
		string no = response.GetField ("no").ToString();
		string level = response.GetField ("currentLevel").ToString();
		string keyNo = response.GetField ("keyNo").ToString ();
		keyNo = keyNo.Substring (1, keyNo.Length - 2);

		string[] tmpKey = keyNo.Split (',');
		int[] keyList = new int[tmpKey.Length];
		for (int i = 0; i < tmpKey.Length; i++) {
			keyList [i] = int.Parse (tmpKey [i]);
		}

		room = new Room (no, level, status, keyList);
		Debug.Log ("Room no." + no + " Level: " + level);
	}

	void onUserDisconnected (SocketIOEvent obj) {
		Debug.Log ("Desconnect from server.");
	}

	void onUserConnected (SocketIOEvent obj) {
		Debug.Log ("Connected.");
		statusGame = CONNECTED;
		FindingMatch ();
	}

	string JsonToString( string target, string s){
		string[] newString = Regex.Split(target,s);
		return newString[1];
	}
}