using UnityEngine;
using System.Collections;

public class ItemsGenerator : MonoBehaviour {

	public const int LAND_TYPE = 0;
	public GameObject key;
	public GameObject door;

	float squareSize;
	int[,] map;

	public void GenerateItems (int[,] originalMap, float sqSize, JSONObject[] items) {
		Debug.Log ("Generating Items");

		squareSize = sqSize;

		map = originalMap;

		Debug.Log (items);
		Debug.Log (items.Length);

		for (int i = 0; i < items.Length; i++) {
			string temp = items [i].ToString ();
			Debug.Log ("temp: " + temp);
			string item = temp.Substring (1, temp.Length - 2);
			Debug.Log ("item: " + item);

			if (item.Equals ("KEY")) {
				Debug.Log ("add key");
				Instantiate (key, RandomPosition(), Quaternion.identity);
			} else if (item.Equals ("DOOR")) {
				Debug.Log ("add door");
				Instantiate (door, RandomPosition(), Quaternion.identity);
			}
		}
	}

	Vector3 RandomPosition() {
		int nodeCountX = map.GetLength (0);
		int nodeCountY = map.GetLength (1);
		float mapWidth = nodeCountX * squareSize;
		float mapHeight = nodeCountY * squareSize;

		bool available = false;
		int x, y;

		while (!available) {
			x = Random.Range (0, nodeCountX);
			y = Random.Range (0, nodeCountY);
			Debug.Log (x + ", " + y);

			if (map [x, y] == 0) {
				map [x, y] = 99;
				Debug.Log ("pos: " + x + ", " + y);
				return new Vector3 (-mapWidth / 2 + x * squareSize + squareSize / 2, 0.1f, -mapHeight / 2 + y * squareSize + squareSize / 2);
				available = true;
			}
		}

		return new Vector3 (0, 0.1f, 0);
	}
}
