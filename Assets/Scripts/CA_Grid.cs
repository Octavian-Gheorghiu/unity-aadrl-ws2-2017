﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CA_Grid : MonoBehaviour {

	// VARIABLES

	// Texture to be used as start of CA input
	public Texture2D seedImage;

	// Time frame
	public int timeEnd = 100;
	int currentFrame = 0;

	// Array for storing voxels
	int width;
	int length;
	int height;
	GameObject[,,] voxelGrid;

	// Reference to the voxel we are using
	public GameObject voxelPrefab;

	// Spacing between voxels
	float spacing = 1.0f;

	// FUNCTIONS

	// Use this for initialization
	void Start () {
		// Read the image width and height
		width = seedImage.width;
		length = seedImage.height;
		height = timeEnd;
		// Create a new CA grid
		CreateGrid ();
	}
	
	// Update is called once per frame
	void Update () {
        // Increment time frame
        if (currentFrame < timeEnd - 1)
        {
            // Calculate the future state of the voxels
            CalculateCA();
            // Update the voxels that are printing
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    GameObject currentVoxel = voxelGrid[i, j, 0];
                    currentVoxel.GetComponent<Voxel>().UpdateVoxel();
                }
            }
            // Save the CA state
            SaveCA();
            currentFrame++;
        }

        // Display the printed voxels
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                for (int k = 1; k < height; k++)
                {
                    voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplay();
                }
            }
        }
    }

	// Create grid function
	void CreateGrid(){
		// Allocate space in memory for the array
		voxelGrid = new GameObject[width, length, height];
		// Populate the array with voxels from a base image
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				for (int k = 0; k < height; k++) {
					// Create values for the transform of the new voxel
					Vector3 currentVoxelPos = new Vector3 (i*spacing,k*spacing,j*spacing);
					Quaternion currentVoxelRot = Quaternion.identity;
					GameObject currentVoxel = Instantiate (voxelPrefab, currentVoxelPos, currentVoxelRot);
                    currentVoxel.GetComponent<Voxel>().SetupVoxel();
                    // Set the state of the voxels
                    if (k == 0) {						
						// Create a new state based on the input image
						int currentVoxelState = (int)seedImage.GetPixel (i, j).grayscale;
						currentVoxel.GetComponent<Voxel> ().SetState (currentVoxelState);
					} else {
                        // Set the state to death
                        currentVoxel.GetComponent<MeshRenderer>().enabled = false;
                        currentVoxel.GetComponent<Voxel> ().SetState (0);
					}
					// Save the current voxel in the voxelGrid array
					voxelGrid[i,j,k] = currentVoxel;
					// Attach the new voxel to the grid game object
					currentVoxel.transform.parent = gameObject.transform;
				}
			}
		}
	}

	// Calculate CA function
	void CalculateCA(){
		// Go over all the voxels stored in the voxels array
		for (int i = 1; i < width-1; i++) {
			for (int j = 1; j < length-1; j++) {
				GameObject currentVoxel = voxelGrid[i,j,0];
				int currentVoxelState = currentVoxel.GetComponent<Voxel> ().GetState ();
				int aliveNeighbours = 0;

				// Calculate how many alive neighbours are around the current voxel
				for (int x = -1; x <= 1; x++) {
					for (int y = -1; y <= 1; y++) {
						GameObject currentNeigbour = voxelGrid [i + x, j + y,0];
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
					// If there are two or three neighbours alive I am going to stay alive
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

	void SaveCA(){
		for(int i =0; i< width; i++){
			for (int j = 0; j < length; j++) {
                GameObject currentVoxel = voxelGrid[i, j, 0];
                int currentVoxelState = currentVoxel.GetComponent<Voxel>().GetState();
                // Save the voxel state
                GameObject savedVoxel = voxelGrid[i, j, currentFrame];
                savedVoxel.GetComponent<Voxel> ().SetState (currentVoxelState);                
                // Save the voxel age if voxel is alive
                if (currentVoxelState == 1) {
                    int currentVoxelAge = currentVoxel.GetComponent<Voxel>().GetAge();
                    savedVoxel.GetComponent<Voxel>().SetAge(currentVoxelAge);
                }
			}
		}
	}
}
