using System.Collections;
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

	// Voxel trace line points
	List<Vector3> linePoints;
	public GameObject tracedLines;
	public Color tracedLinesColorStart = Color.red;
	public Color tracedLinesColorEnd = Color.blue;

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
        // Calculate the CA state, save the new state, display the CA and increment time frame
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
            // Increment the current frame count
            currentFrame++;
        }
        // Spin the CA if spacebar is pressed (be careful, GPU instancing will be lost!)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameObject.GetComponent<ModelDisplay>() == null)
            {
                gameObject.AddComponent<ModelDisplay>();
            }
            else 
            {
                Destroy(gameObject.GetComponent<ModelDisplay>());
            }
        }
		// Trace the top voxels and show/hide them
		if(currentFrame == timeEnd-1){
			if(Input.GetKeyDown(KeyCode.T)){
				TraceCA ();
				gameObject.SetActive (false);
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
					currentVoxel.GetComponent<Voxel>().SetupVoxel(i,j,k,1);
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

    // Save the CA states
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

	// Trace CA
	void TraceCA(){
		// Save in a list all the alive voxels from the last layer
		List<GameObject> aliveVoxels = new List<GameObject> ();
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				GameObject currentVoxel = voxelGrid [i,j,height-2];
				if(currentVoxel.GetComponent<Voxel>().GetState() == 1){
					aliveVoxels.Add (currentVoxel);
				}
			}
		}
		// Get a random alive voxel from the last layer
		for (int i = 0; i < aliveVoxels.Count; i++) {
			GameObject randomVoxel = aliveVoxels[i];
			// Initalize the list that will save the polyline points
			linePoints = new List<Vector3> ();
			linePoints.Add (randomVoxel.GetComponent<Transform>().position);
			// Generate path below
			int currentHeight = height-2;
			while (currentHeight > 1) {
				// Get the oldest voxel below the currentVoxel
				int currentI = (int) randomVoxel.GetComponent<Voxel> ().address.x;
				int currentJ = (int) randomVoxel.GetComponent<Voxel> ().address.y;
				int currentK = (int) randomVoxel.GetComponent<Voxel> ().address.z;
				Vector2 oldestVoxelBelowAddress = PathCalculator (currentI, currentJ, currentK);
				GameObject oldestVoxelBelow = voxelGrid [(int)oldestVoxelBelowAddress.x, (int)oldestVoxelBelowAddress.y, (int)currentK-1];
				// Add the position of the oldest voxel below the the polyline list
				linePoints.Add(oldestVoxelBelow.GetComponent<Transform>().position);
				// Set the current voxel to the oldest voxel below
				randomVoxel = oldestVoxelBelow;
				// Start calculation for the layer below
				currentHeight--;
			}
			// Create a line render based on the traced path
			GameObject currentLine = new GameObject();
			currentLine.name = "line(" + i.ToString ()+")";
			currentLine.transform.parent = tracedLines.transform;
			LineRenderer lineRenderer = currentLine.AddComponent<LineRenderer>();
			lineRenderer.material = new Material (Shader.Find("Particles/Multiply"));
			lineRenderer.widthMultiplier = 0.2f;
			lineRenderer.positionCount = linePoints.Count;
			float alpha = 1.0f;
			Gradient gradient = new Gradient();
			gradient.SetKeys(
				new GradientColorKey[] { new GradientColorKey(tracedLinesColorStart, 0.0f), new GradientColorKey(tracedLinesColorEnd, 1.0f) },
				new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.5f), new GradientAlphaKey(alpha, 1.0f) }
			);
			lineRenderer.colorGradient = gradient;
			lineRenderer.SetPositions (linePoints.ToArray());
		}
	}

	// Path calculator
	Vector2 PathCalculator(int i, int j, int k){
		int oldestAge = 0;
		Vector2 voxelWithOldestAgeAddress = new Vector2(0,0);
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				GameObject currentVoxel = voxelGrid [i + x, j + y, k - 1];
				int currentVoxelAge = currentVoxel.GetComponent<Voxel> ().GetAge();
				if (currentVoxelAge > oldestAge) {
					oldestAge = currentVoxelAge;
					voxelWithOldestAgeAddress.x = i + x;
					voxelWithOldestAgeAddress.y = j + y;
				}
			}
		}
		return voxelWithOldestAgeAddress;
	}
}
