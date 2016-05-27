using UnityEngine;
using System.Collections;

public class PositionRandomizer : MonoBehaviour {

	public const int LAND_TYPE = 0;
	public const int ENVI = -1, MONSTER = 0, PLAYER = -1, ITEM = 1;

	public GameObject key;
	public GameObject door;

	float squareSize;

	int[,] map;

	int nodeCountX;
	int nodeCountY;
	float mapWidth;
	float mapHeight;

	public void CreateRandomizer (int[,] originalMap, float sqSize) {
		squareSize = sqSize;
		map = originalMap;

		nodeCountX = map.GetLength (0);
		nodeCountY = map.GetLength (1);
		mapWidth = nodeCountX * squareSize;
		mapHeight = nodeCountY * squareSize;
	}

	public Vector3 RandomPosition (int mark) {
		bool available = false;
		int x, y;

		while (!available) {
			x = Random.Range (0, nodeCountX);
			y = Random.Range (0, nodeCountY);

			if (map [x, y] == 0 && mark >= 0) {
				if (mark != 0)
					map [x, y] = 99;
				return new Vector3 (-mapWidth / 2 + x * squareSize + squareSize / 2, 0.0f, -mapHeight / 2 + y * squareSize + squareSize / 2);
				available = true;
			} else if (map [x, y] == 0 && mark < 0 && isNearbyAvailable (x, y)) {
				map [x, y] = 99;
				return new Vector3 (-mapWidth / 2 + x * squareSize + squareSize / 2, 0.0f, -mapHeight / 2 + y * squareSize + squareSize / 2);
				available = true;
			}
		}

		return new Vector3 (0, 0.1f, 0);
	}

	bool isNearbyAvailable (int x, int y) {
		if (x < 1 || y < 1 || x >= nodeCountX || y >= nodeCountY) {
			return false;
		}

		if (map [x + 1, y] == 0 && map [x - 1, y] == 0 && map [x, y + 1] == 0 && map [x, y - 1] == 0) {
			return true;
		}

		return false;
	}

	public Vector3 GetRandomPosition () {
		return RandomPosition (99);
	}
}
