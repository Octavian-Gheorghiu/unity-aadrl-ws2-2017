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
		// Calculate the future state of the voxels
		CalculateCA ();
		// Update the voxels and display them
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				GameObject currentVoxel = voxelGrid [i, j];
				currentVoxel.GetComponent<Voxel> ().Update ();
				currentVoxel.GetComponent<Voxel> ().DisplayVoxel ();
			}
		}		
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
				// Attach the new voxel to the grid game object
				currentVoxel.transform.parent = gameObject.transform;
				// Save the current voxel in the voxelGrid array
				voxelGrid[i,j] = currentVoxel;
			}
		}
	}

	void CalculateCA(){
		// Go over all the voxels stored in the voxels array
		for (int i = 1; i < width-1; i++) {
			for (int j = 1; j < length-1; j++) {
				GameObject currentVoxel = voxelGrid[i,j];
				int currentVoxelState = currentVoxel.GetComponent<Voxel> ().GetState ();
				int aliveNeighbours = 0;
				int currentVoxelFutureState = 0;

				// Calculate how many alive neighbours are oround the current voxel
				for (int x = -1; x <= 1; x++) {
					for (int y = -1; y <= 1; y++) {
						GameObject currentNeigbour = voxelGrid [i + x, j + y];
						int currentNeigbourState = currentNeigbour.GetComponent<Voxel> ().GetState();
						aliveNeighbours += currentNeigbourState;
					}
				}
				aliveNeighbours -= currentVoxelState;
				// Rule Set 1: for voxels that are alive
				if (currentVoxelState == 1) {
					// If there are less than two neighbours I am going to die
					if (aliveNeighbours < 2) {
						currentVoxel.GetComponent<Voxel> ().SetFutureState (0);
					}
					// If there are two or three neigbours alive I am going to stay alive
					if(aliveNeighbours == 2 || aliveNeighbours == 3){
						currentVoxel.GetComponent<Voxel> ().SetFutureState (1);
					}
					// If there are more than three neighbours I am going to die
					if (aliveNeighbours > 3) {
						currentVoxel.GetComponent<Voxel> ().SetFutureState (0);
					}
				}
				// Rule Set 2: for voxels that are death
				if(currentVoxelState == 0){
					// If there are exactly three alive neighbours I will become alive
					if(aliveNeighbours == 3){
						currentVoxel.GetComponent<Voxel> ().SetFutureState (1);
					}
				}
			}
		}
	}
}
