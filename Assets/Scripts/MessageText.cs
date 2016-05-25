using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageText : MonoBehaviour {

	public GameObject GameCanvas;
	public Text msgText;

	void Start () {
		GameCanvas.SetActive (false);
	}
	
	void Update () {
		
	}

	public void setText(string message){
		msgText.text = message;
	}

	public void show(){
		GameCanvas.SetActive (true);
		StartCoroutine (Coroutine());
	}

	IEnumerator Coroutine(){
		yield return new WaitForSeconds (10.0f);
		GameCanvas.SetActive (false);
	}
}
