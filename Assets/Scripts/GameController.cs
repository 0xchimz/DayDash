using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SocketIO;
using RAIN.Navigation;
using RAIN.Navigation.Targets;
using RAIN.Core;
using RAIN.BehaviorTrees;
using RAIN.Minds;
using RAIN.Memory;
using RAIN;

public class GameController : MonoBehaviour {

	public readonly int CONNECTING = 0, CONNECTED = 1, FINDING_MATCH = 2, JOINED = 3, STARTING = 4;
	public static GameController instance;
	public SocketIOComponent socket;

	private GameObject player;
	private Player playerComponent;
	private Room room;
	private int statusGame;
	private ArrayList gameItems = new ArrayList ();
	private MessageText msgText;

	public Text status;
	public GameObject ui;
	public GameObject map;
	public ItemList itemList;
	public bool isGamePlay = false;

	void Awake () {
		instance = this;
	}

	void Start () {
		player = GameObject.Find ("Player");
		playerComponent = player.GetComponent<Player> ();
		player.SetActive (false);
		playerComponent.GetComponentInChildren<NavigationTargetRig> ().Target.MountPoint = playerComponent.transform;
		playerComponent.GetComponentInChildren<NavigationTargetRig> ().Target.TargetName = "NavTarget";

		ui.SetActive (true);

		GameObject go = GameObject.Find ("SocketIO");
		socket = go.GetComponent<SocketIOComponent> ();

		socket.On ("USER_CONNECTED", onUserConnected);
		socket.On ("JOIN_RESPONSE", onJoined);
		socket.On ("START_GAME", onStart);
		socket.On ("PLAY_GAME", onPlay);
		socket.On ("GENERATE_ITEM", itemInfo);
		socket.On ("EVENT", onEvent);
		socket.On ("USER_DISCONNECTED", onUserDisconnected);

		socket.On ("error", onError);
		socket.On ("connect", onUserConnected);

		statusGame = CONNECTING;

		status.text = "CONNECTING TO SERVER...";

		msgText = GetComponent<MessageText> ();
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

	void onError (SocketIOEvent e) {
		statusGame = CONNECTING;
		Debug.Log ("Connect error received: " + e.name + " " + e.data);
		ui.SetActive (true);
		player.SetActive (false);
	}

	void itemInfo (SocketIOEvent e) {
		Debug.Log ("Generate Item");
		JSONObject[] arr = e.data.GetField ("data").list.ToArray ();
		string[] items = new string[arr.Length];
		for (int i = 0; i < arr.Length; i++) {
			string item = arr [i].ToString ();
			items [i] = item.Substring (1, item.Length - 2);
		}
		generateItem (items);
	}

	void onEvent (SocketIOEvent e) {
		string evn = e.data.GetField ("name").ToString ();
		string tmp = evn.Substring (1, evn.Length - 2);
		Debug.Log (tmp);
		if (tmp.Equals ("ENABLE_DOOR")) {
			Debug.Log ("Enable the door");
			msgText.setText ("THE DOOR IS OPEN, FIND THE DOOR TO ESCAPE!");
			msgText.show ();
		}
	}

	void generateItem (string[] items) {
		Debug.Log ("GenerateItem from string array");
		Vector3 min = map.GetComponent<MeshRenderer> ().bounds.min;
		Vector3 max = map.GetComponent<MeshRenderer> ().bounds.max;
		Debug.Log (min + max);
		for (int i = 0; i < items.Length; i++) {
			float x = Random.Range (min.x, max.x);
			float z = Random.Range (min.z, max.z);

			if (items [i].Equals ("KEY")) {
				GameObject tmp = Instantiate (itemList.key, new Vector3 (x, 0.2f, z), Quaternion.identity) as GameObject;
				gameItems.Add (tmp);
			}
			if (items [i].Equals ("DOOR")) {
				GameObject tmp = Instantiate (itemList.door, new Vector3 (x, 0.2f, z), Quaternion.identity) as GameObject;
				gameItems.Add (tmp);	
			}
		}
	}

	void FindingMatch () {
		Debug.Log ("Finding match");
		socket.Emit ("JOIN_ROOM");
	}

	void onDisconnect (SocketIOEvent obj) {
		Debug.Log ("Disconnect");
	}

	void onPlay (SocketIOEvent obj) {
		ui.SetActive (false);
		player.SetActive (true);
		this.isGamePlay = true;
	}

	void onStart (SocketIOEvent e) {
		statusGame = STARTING;
		JSONObject[] items = e.data.GetField ("data").list.ToArray ();
		Debug.Log ("Starting Game");
		MapGenerator genMapComponent = map.GetComponent<MapGenerator> ();
		genMapComponent.GenerateMap (items);
		socket.Emit ("GAME_STATUS_READY");
	}

	void onJoined (SocketIOEvent obj) {
		statusGame = JOINED;
		Debug.Log ("Joined");
		JSONObject response = obj.data.GetField ("roomInfo");
		string status = response.GetField ("status").ToString ();
		string no = response.GetField ("no").ToString ();
		string level = response.GetField ("currentLevel").ToString ();
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

	public void onUserConnected (SocketIOEvent obj) {
		Debug.Log ("Connected.");
		statusGame = CONNECTED;
		FindingMatch ();
	}

	string JsonToString (string target, string s) {
		string[] newString = Regex.Split (target, s);
		return newString [1];
	}
}