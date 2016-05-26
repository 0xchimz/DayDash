using UnityEngine;
using System.Collections;

public class MagicAttack : MonoBehaviour {
	public int damage = 20;

	GameObject enemy;
	EnemyHealth enemyHealth;


	// Use this for initialization
	void Start () {
		enemy = GameObject.FindGameObjectWithTag ("Enemy");
		enemyHealth = enemy.GetComponent <EnemyHealth> ();

		//Debug.Log (enemyHealth + " found.");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other) {

		if (other.tag == "Enemy") {
			Debug.Log ("PLAYER used DARK CIRCLE! \nIt's not very effective. . . ");

			enemyHealth.TakeDamage (damage);


		} 

	
	}
}
