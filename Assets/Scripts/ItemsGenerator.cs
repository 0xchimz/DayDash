using UnityEngine;
using System.Collections;

public class ItemsGenerator : MonoBehaviour {

	public GameObject key;
	public GameObject door;

	public void GenerateItems (PositionRandomizer randomizer, JSONObject[] items) {
		for (int i = 0; i < items.Length; i++) {
			string temp = items [i].ToString ();
			Debug.Log ("temp: " + temp);
			string item = temp.Substring (1, temp.Length - 2);
			Debug.Log ("item: " + item);

			if (item.Equals ("KEY")) {
				Debug.Log ("add key");
				Instantiate (key, randomizer.RandomPosition (PositionRandomizer.ITEM), Quaternion.identity);
			} else if (item.Equals ("DOOR")) {
				Debug.Log ("add door");
				Instantiate (door, randomizer.RandomPosition (PositionRandomizer.ITEM), Quaternion.identity);
			}
		}
	}
}
