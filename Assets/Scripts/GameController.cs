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
using PlayFab;
using PlayFab.ClientModels;

public class GameController : MonoBehaviour {

	public readonly int CONNECTING = 0, CONNECTED = 1, FINDING_MATCH = 2, JOINED = 3, STARTING = 4, LANDING = 5;
	public static GameController instance;
	public SocketIOComponent socket;

	private GameObject player;
	private Player playerComponent;
	private Room room;
	private int statusGame;
	private MessageText msgText;

	public Text status;
	public InputField input;
	public Button btn;
	public GameObject ui;
	public GameObject map;
	public GameObject items;
	public GameObject environment;
	public GameObject enemies;
	public string PlayFabId;

	public bool isGamePlay = false;

	void Awake () {
		PlayFabSettings.TitleId = "8CF2";
		instance = this;
	}

	void Start () {
		Login ("8CF2");
		player = GameObject.Find ("Player");
		playerComponent = player.GetComponent<Player> ();
		player.SetActive (false);
		playerComponent.GetComponentInChildren<NavigationTargetRig> ().Target.MountPoint = playerComponent.transform;
		playerComponent.GetComponentInChildren<NavigationTargetRig> ().Target.TargetName = "NavTarget";

		ui.SetActive (true);
		input.gameObject.SetActive (false);
		btn.gameObject.SetActive (false);

		GameObject go = GameObject.Find ("SocketIO");
		socket = go.GetComponent<SocketIOComponent> ();

		socket.On ("JOIN_RESPONSE", onJoined);
		socket.On ("START_GAME", onStart);
		socket.On ("PLAY_GAME", onPlay);
		socket.On ("EVENT", onEvent);
		socket.On ("USER_DISCONNECTED", onUserDisconnected);
		socket.On ("NEXT_MATCH", onNextMatch);
		socket.On ("DEAD", onDead);

		socket.On ("error", onError);
		socket.On ("connect", onUserConnected);

		statusGame = CONNECTING;

		status.text = "CONNECTING TO SERVER...";

		msgText = GetComponent<MessageText> ();
	}

	void Update () {
		if (statusGame == CONNECTING) {
			status.text = "CONNECTING TO SERVER...";
		} else if (statusGame == FINDING_MATCH) {
			status.text = "FINDING MATCH...";
		} else if (statusGame == JOINED) {
			status.text = "JOINED, WAIT OTHER PLAYER...";
		} else if (statusGame == STARTING) {
			status.text = "STARTING GAME...";
		} else if (statusGame == LANDING) {
			status.text = "Please Input your name.";
		}
	}

	void onError (SocketIOEvent e) {
		statusGame = CONNECTING;
		Debug.Log ("Connect error received: " + e.name + " " + e.data);
		ui.SetActive (true);
		player.SetActive (false);
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
		Debug.Log ("Starting Game");
		Debug.Log (e.data.ToString ());
		JSONObject[] itemList = e.data.GetField ("data").GetField ("items").list.ToArray ();
		Debug.Log (itemList);

		MapGenerator mapGen = map.GetComponent<MapGenerator> ();
		mapGen.GenerateMap ();

		Debug.Log ("Generate Randomizer");
		PositionRandomizer randomizer = map.GetComponent<PositionRandomizer> ();

		ItemsGenerator itemsGen = items.GetComponent<ItemsGenerator> ();
		itemsGen.GenerateItems (randomizer, itemList);

		EnvironmentGenerator enviGen = environment.GetComponent<EnvironmentGenerator> ();
		enviGen.GenerateEnvironment (randomizer);

		player.transform.position = randomizer.RandomPosition (PositionRandomizer.PLAYER);

		EnemyManager enemyManager = enemies.GetComponent<EnemyManager> ();
		enemyManager.CreateEnemyManager (randomizer);

		socket.Emit ("GAME_STATUS_READY");
	}

	void onNextMatch (SocketIOEvent e) {
		
		GameObject[] objects = GameObject.FindGameObjectsWithTag ("Environment");
		for (int i = 0; i < objects.Length; i++) {
			Destroy (objects [i]);
		}

		GameObject[] _enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i = 0; i < _enemies.Length; i++) {
			Destroy (_enemies [i]);
		}

		GameObject[] doors = GameObject.FindGameObjectsWithTag ("Door");
		for (int i = 0; i < doors.Length; i++) {
			Destroy (doors [i]);
		}

		GameObject[] keys = GameObject.FindGameObjectsWithTag ("Key");
		for (int i = 0; i < keys.Length; i++) {
			Destroy (keys [i]);
		}

		ui.SetActive (true);
		player.SetActive (false);
		onStart (e);
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

	void landingPage () {
		statusGame = LANDING;
		ui.SetActive (true);
		input.gameObject.SetActive (true);
		btn.gameObject.SetActive (true);

		btn.onClick.AddListener (delegate {
			status.text = "Saving...";
			btn.gameObject.SetActive(false);
			input.gameObject.SetActive(false);
			clickFindMatch ();	
		});
	}

	void clickFindMatch () {
		if (input.text != "") {
			UpdateUserTitleDisplayNameRequest request = new UpdateUserTitleDisplayNameRequest ();
			request.DisplayName = input.text;
			PlayFabClientAPI.UpdateUserTitleDisplayName (request, (UpdateUserTitleDisplayNameResult result) => {
				Debug.Log ("Update Display Success");
				FindingMatch ();
			}, (PlayFabError error) => {
				status.text = "Cannot update display name...";
				Debug.Log ("Update Display Error");
				FindingMatch ();
			});
		}
	}

	void Login (string titleId) {
		LoginWithCustomIDRequest request = new LoginWithCustomIDRequest () {
			TitleId = titleId,
			CreateAccount = true,
			CustomId = SystemInfo.deviceUniqueIdentifier
		};

		PlayFabClientAPI.LoginWithCustomID (request, (result) => {
			PlayFabId = result.PlayFabId;
			Debug.Log ("Got PlayFabID: " + PlayFabId);

			if (result.NewlyCreated) {
				Debug.Log ("(new account)");
			} else {
				Debug.Log ("(existing account)");
			}
		},
			(error) => {
				Debug.Log ("Error logging in player with custom ID:");
				Debug.Log (error.ErrorMessage);
			});
	}

	public void onUserConnected (SocketIOEvent obj) {
		Debug.Log ("Connected.");
		statusGame = CONNECTED;
		landingPage ();
		getUserData ();
	}

	void getUserData () {
		GetUserDataRequest request2 = new GetUserDataRequest () {
			PlayFabId = PlayFabId,
			Keys = null
		};
		PlayFabClientAPI.GetUserData (request2, (result) => {
			Debug.Log ("Got user data:");
			if ((result.Data == null) || (result.Data.Count == 0)) {
				Debug.Log ("No user data available");
			} else {
				foreach (var item in result.Data) {
					Debug.Log ("    " + item.Key + " == " + item.Value.Value);
				}
			}
		}, (error) => {
			Debug.Log ("Got error retrieving user data:");
			Debug.Log (error.ErrorMessage);
		});
	}

	string JsonToString (string target, string s) {
		string[] newString = Regex.Split (target, s);
		return newString [1];
	}

	public void onDead(SocketIOEvent e){

		GameObject[] objects = GameObject.FindGameObjectsWithTag ("Environment");
		for (int i = 0; i < objects.Length; i++) {
			Destroy (objects [i]);
		}

		GameObject[] _enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i = 0; i < _enemies.Length; i++) {
			Destroy (_enemies [i]);
		}

		GameObject[] doors = GameObject.FindGameObjectsWithTag ("Door");
		for (int i = 0; i < doors.Length; i++) {
			Destroy (doors [i]);
		}

		GameObject[] keys = GameObject.FindGameObjectsWithTag ("Key");
		for (int i = 0; i < keys.Length; i++) {
			Destroy (keys [i]);
		}

		landingPage ();
	}

	public void playerDead () {
		onDead(null);
		socket.Emit ("DEAD");
	}
}