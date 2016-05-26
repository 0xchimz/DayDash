using UnityEngine;
using System.Collections;

public class MagicAttack : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider gameElement) {
		Debug.Log (".... Cause someone damage");
	
	}
}
