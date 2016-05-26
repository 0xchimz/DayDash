using UnityEngine;
using System.Collections;
using RAIN.Navigation.NavMesh;
using RAIN.Navigation;
using RAIN.Navigation.Targets;
using RAIN.Core;
using RAIN.BehaviorTrees;
using RAIN.Minds;
using RAIN.Memory;
using RAIN;

public class NavigationMesh : MonoBehaviour {

	private int _threadCount = 4;
	public NavMeshRig tRig;

	public void CreateNavMesh() {
		Debug.Log ("Creating Nav Mesh");
		tRig = gameObject.GetComponentInChildren<RAIN.Navigation.NavMesh.NavMeshRig>();
//		tRig = GetComponent<NavMeshRig>();
		StartCoroutine (GenerateNavMesh ());
	}

	IEnumerator GenerateNavMesh () {
		Debug.Log ("Generating Nav Mesh");
		tRig.NavMesh.UnregisterNavigationGraph ();
		tRig.NavMesh.Size = 128;
		float startTime = Time.time;
		tRig.NavMesh.StartCreatingContours (tRig, _threadCount);
		while (tRig.NavMesh.Creating) {
			tRig.NavMesh.CreateContours ();

			yield return new WaitForSeconds (1);
		}
		//isNavMeshDone = true;
		float endTime = Time.time;
		Debug.Log ("NavMesh generated in " + (endTime - startTime) + "s");
		tRig.NavMesh.RegisterNavigationGraph ();
		tRig.Awake ();
		GameController.instance.socket.Emit ("GAME_STATUS_READY");
	}
}