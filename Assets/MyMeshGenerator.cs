using UnityEngine;
using System.Collections;

public class MyMeshGenerator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		RAIN.Navigation.NavMesh.NavMeshRig rig = GameObject.FindGameObjectWithTag ("NavMesh").GetComponent<RAIN.Navigation.NavMesh.NavMeshRig> ();
		RAIN.Navigation.NavMesh.NavMesh meshZ = rig.NavMesh;
		//clear the exitsting navmesh
		meshZ.UnregisterNavigationGraph();
		meshZ.StartCreatingContours(rig, 1);
		while(meshZ.Creating)  {
			meshZ.CreateContours ();
			System.Threading.Thread.Sleep (10);
		}

		rig.GenerateAllContourVisuals ();
		meshZ.RegisterNavigationGraph ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
