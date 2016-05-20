using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

	public string no;
	public string currentLevel;
	public string status;
	public int[] levelKey;

	public Room(string no, string currentLevel, string status, int[] levelKey){
		this.no = no;
		this.currentLevel = currentLevel;
		this.status = status;
		this.levelKey = levelKey;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
