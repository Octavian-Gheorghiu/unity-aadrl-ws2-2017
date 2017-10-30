using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CA_Grid : MonoBehaviour {

	// VARIABLES

	// Array for storing voxels
	public int width = 20;
	public int length = 20;
	GameObject[,] voxelGrid;

	// Reference to the voxel we are using
	public GameObject voxelPrefab;

	// Spacing between voxels
	float spacing = 1.2f;

	// FUNCTIONS

	// Use this for initialization
	void Start () {
		CreateGrid ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Create grid function
	void CreateGrid(){
		// Allocate space in memory for the array
		voxelGrid = new GameObject[width, length];
		// Populate the array with voxels, randomly alive
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				// Create values for the transfrom of the new voxel
				Vector3 currentVoxelPos = new Vector3 (i*spacing,0,j*spacing);
				Quaternion currentVoxelRot = Quaternion.identity;
				GameObject currentVoxel = Instantiate (voxelPrefab, currentVoxelPos, currentVoxelRot);
				// Create a new random state
				int currentVoxelState = Random.Range (0, 2);
				// Set the state of the new voxel
				currentVoxel.GetComponent<Voxel> ().SetState (currentVoxelState);

				currentVoxel.GetComponent<Voxel> ().SetColor ();
				currentVoxel.transform.parent = this.transform;
				// Save the current voxel in the voxelGrid array
				voxelGrid[i,j] = currentVoxel;
			}
		}
	}
}
