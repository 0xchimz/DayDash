using UnityEngine;
using System.Collections;

public class DumbPlayer : MonoBehaviour {

	Rigidbody rigidbody;
	Vector3 velocity;

	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
	}

	void Update () {
		velocity = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized * .1f;
	}

	void FixedUpdate () {
		rigidbody.MovePosition (rigidbody.position + velocity * Time.fixedTime);
	}
}
