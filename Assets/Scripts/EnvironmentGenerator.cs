using UnityEngine;
using System.Collections;

public class EnvironmentGenerator : MonoBehaviour {

	const int MAX = 50;

	public GameObject treePine01;
	public GameObject treePine04;
	public GameObject treePine07;

	public GameObject brush01;
	public GameObject brush02;
	public GameObject brush03;

	public GameObject treeStump;
	public GameObject cloud01;
	public GameObject cloud02;

	public GameObject rock;
	public GameObject flower;

	public void GenerateEnvironment (PositionRandomizer randomizer) {
		for (int i = 0; i < MAX; i++) {
			switch (Random.Range (0, 3)) {
			case 0:
				treePine01.layer = 8;
				treePine01.tag = "Environment";
				Create (randomizer, treePine01);
				break;
			case 1:
				treePine04.layer = 8;
				treePine04.tag = "Environment";
				Create (randomizer, treePine04);
				break;
			case 2:
				treePine07.layer = 8;
				treePine07.tag = "Environment";
				Create (randomizer, treePine07);
				break;
			}
		}

		for (int i = 0; i < 20; i++) {
			switch (Random.Range (0, 3)) {
			case 0:
				brush02.layer = 8;
				brush02.tag = "Environment";
				Create (randomizer, brush02);
				break;
			case 1:
				brush03.layer = 8;
				brush03.tag = "Environment";
				Create (randomizer, brush03);
				break;
			case 2:
				rock.layer = 8;
				rock.tag = "Environment";
				Create (randomizer, rock);
				break;
			}
		}

		for (int i = 0; i < 10; i++) {
			switch (Random.Range (0, 3)) {
			case 0:
				treeStump.layer = 8;
				treeStump.tag = "Environment";
				Create (randomizer, treeStump);
				break;
			case 1:
				cloud01.layer = 8;
				cloud01.tag = "Environment";
				Create (randomizer, cloud01);
				break;
			case 2:
				cloud02.layer = 8;
				cloud02.tag = "Environment";
				Create (randomizer, cloud02);
				break;
			}
		}
	}

	void Create (PositionRandomizer randomizer, GameObject obj) {
		var rotation = Quaternion.Euler (0, Random.Range (0, 360), 0);
		Instantiate (obj, randomizer.RandomPosition (PositionRandomizer.ENVI), rotation);
	}
}
