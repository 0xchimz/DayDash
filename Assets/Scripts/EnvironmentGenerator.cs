using UnityEngine;
using System.Collections;

public class EnvironmentGenerator : MonoBehaviour {

	const int MAX = 50;

	public GameObject treePine01;
	public GameObject treePine04;
	public GameObject treePine07;

	public void GenerateEnvironment (PositionRandomizer randomizer) {
		for (int i = 0; i < MAX; i++) {
			switch (Random.Range (0, 3)) {
			case 0:
				treePine01.tag = "Environment";
				Create (randomizer, treePine01);
				break;
			case 1:
				treePine04.tag = "Environment";
				Create (randomizer, treePine04);
				break;
			case 2:
				treePine07.tag = "Environment";
				Create (randomizer, treePine07);
				break;
			}
		}
	}

	void Create (PositionRandomizer randomizer, GameObject obj) {
		var rotation = Quaternion.Euler (0, Random.Range (0, 360), 0);
		Instantiate (obj, randomizer.RandomPosition (PositionRandomizer.ENVI), rotation);
	}
}
