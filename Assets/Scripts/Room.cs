using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

	public string no;
	public string level;
	public string status;

	public Room(string no, string level, string status){
		this.no = no;
		this.level = level;
		this.status = status;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
