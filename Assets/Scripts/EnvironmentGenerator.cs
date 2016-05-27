using UnityEngine;
using System.Collections;

public class EnvironmentGenerator : MonoBehaviour {

	const int MAX = 20;

	public GameObject treePine01;
	public GameObject treePine04;
	public GameObject treePine07;

	public void GenerateEnvironment(PositionRandomizer randomizer) {
		for (int i = 0; i < MAX; i++) {

			switch (Random.Range (0, 3)) {
			case 0:
				Instantiate (treePine01, randomizer.RandomPosition (PositionRandomizer.ENVI), Quaternion.identity);
				break;
			case 1:
				Instantiate (treePine04, randomizer.RandomPosition (PositionRandomizer.ENVI), Quaternion.identity);
				break;
			case 2:
				Instantiate (treePine07, randomizer.RandomPosition (PositionRandomizer.ENVI), Quaternion.identity);
				break;
			}
		}
	}
}
