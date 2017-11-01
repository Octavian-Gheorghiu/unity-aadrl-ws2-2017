using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel : MonoBehaviour {

	// VARIABLES
	int state = 0;
	int futureState = 0;

	// FUNCTIONS
	
	// Update function
	public void Update () {
		state = futureState;
	}

	// Display the voxel
	public void DisplayVoxel(){
		MaterialPropertyBlock props = new MaterialPropertyBlock ();
		MeshRenderer renderer;
		if (state == 0) {
			props.SetColor("_Color", new Color(1,1,1,0));

			renderer = gameObject.GetComponent<MeshRenderer> ();
			renderer.SetPropertyBlock (props);
		}
		if (state == 1) {
			props.SetColor("_Color", Color.black);

			renderer = gameObject.GetComponent<MeshRenderer> ();
			renderer.SetPropertyBlock (props);
		}
	}

	// Set the state of the voxel
	public void SetState(int _state){
		state = _state;
	}

	// Set the future state of the voxel
	public void SetFutureState(int _futureState){
		futureState = _futureState;
	}

	// Get the state of the voxel
	public int GetState(){
		return state;
	}
}
