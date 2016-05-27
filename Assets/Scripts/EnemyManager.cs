using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using RAIN.Navigation;
using RAIN.Navigation.Targets;
using RAIN.Core;
using RAIN.BehaviorTrees;
using RAIN.Minds;
using RAIN.Memory;
using RAIN;
using RAIN.Navigation.NavMesh;

public class EnemyManager : MonoBehaviour {

	//public PlayerHealth playerHealth;	// Reference to the player's health.
	public GameObject enemy;
	public float spawnTime = 3f;

	private MeshGenerator meshGen;
	PositionRandomizer randomizer;

	public void CreateEnemyManager (PositionRandomizer posr) {
		randomizer = posr;
		StartCoroutine (navMeshCheck ());
	}

	IEnumerator navMeshCheck () {

		yield return new WaitForSeconds (spawnTime);
		if (GameController.instance.isGamePlay) {
			InvokeRepeating ("Spawn", spawnTime, spawnTime);
		} else {
			StartCoroutine (navMeshCheck ());
		}
	}

	void Spawn () {
		// If the player has no health left...

		/*
		if(playerHealth.currentHealth <= 0f) {
			// ... exit the function.
			return;
		}
		*/

		var rotation = Quaternion.Euler (0, Random.Range (0, 360), 0);
		Instantiate (enemy, randomizer.RandomPosition (PositionRandomizer.MONSTER), rotation);

		// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
		//enemy.gameObject.GetComponentInChildren<AIRig>().AI.Motor.DefaultSpeed = 10;
		/**
		BTAsset tCustomAsset = ScriptableObject.CreateInstance<BTAsset>();
		TextAsset xml = Resources.Load("btrees/test2") as TextAsset;
		tCustomAsset.SetTreeData(xml.text);

		tCustomAsset.SetTreeBindings(new string[0] { });
		((BasicMind)enemy.gameObject.GetComponentInChildren<AIRig>().AI.Mind).SetBehavior(tCustomAsset, new List<BTAssetBinding>());
		**/
	}
}
