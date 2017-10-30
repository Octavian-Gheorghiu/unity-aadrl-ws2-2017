using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel : MonoBehaviour {

	// VARIABLES
	int alive;
	Color voxelColor;

	// CONSTRUCTOR
	Voxel(int _alive){
		alive = _alive;
		// Am I alive or not? Set color based on state.
		if (alive == 1) {
			voxelColor = Color.black;
		} else {
			voxelColor = Color.white;
		}
	}

	// FUNCTIONS
	
	// Update function
	void Update () {
		// Am I alive or not? Set color based on state.
		if (alive == 1) {
			voxelColor = Color.black;
		} else {
			voxelColor = Color.white;
		}
	}

	// Set the state of the voxel
	public void SetState(int _state){
		alive = _state;
	}

	// Get the state of the voxel
	public int GetState(){
		return alive;
	}

	// Set the color of the voxel
	public void SetColor(){
		if (alive == 1) {
			voxelColor = Color.black;
			this.GetComponent<MeshRenderer> ().material.color = voxelColor;
		} else {
			voxelColor = Color.white;
			this.GetComponent<MeshRenderer> ().material.color = voxelColor;
		}
	}
}
