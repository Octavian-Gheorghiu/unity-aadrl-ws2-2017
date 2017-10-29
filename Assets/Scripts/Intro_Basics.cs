/* 
 * This is the introduction to unity c# scripting.
 * We are aiming to cover the basic code concepts of programming
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_Basics : MonoBehaviour {

	// VARIABLES - exposure type name value
	// Numbers
	public int number1Integer = 1; 
	float number1Float = 1.0f; // we end a line by adding a ;

	// Text
	public string textValue = "octavian`s string"; // strings are inside of ""

	// Conditionals
	bool trueCondition = true;
	bool falseCondition = false;
	public bool doIPrintToTheConsole = false;

	// Prefabs
	public GameObject cubePrefab;

	// Data structures
	int width = 20;
	int length = 20;
	GameObject[,] cubeArray = new GameObject[20,20];

	// FUNCTIONS - exposure type name () { instruction go here }
	// Use this for initialization 
	void Start () {
		// Make a cube in the origin
		Vector3 position = new Vector3(0f,0f,0f); //left-right, forward-backward, up-down
		Quaternion rotation = Quaternion.identity;
		GameObject cubePrefabClone = Instantiate (cubePrefab, position, rotation); //what, where, what rotation
		// Make a new cube 2 units to the right, using the same rotation of the first cube
		Vector3 position2 = new Vector3(2f,0f,0f); //left-right, forward-backward, up-down
		GameObject cubePrefabClone2 = Instantiate (cubePrefab, position2, rotation);
		// Make a new cube 4 units to the right, using the same rotation of the first cube
		Vector3 position3 = new Vector3(4f,0f,0f); //left-right, forward-backward, up-down
		GameObject cubePrefabClone3 = Instantiate (cubePrefab, position3, rotation);

		// Loops | For (create 20 cubes spaced apart on x axis 5 units after the inital 3 cubes)
		for (int i = 6; i <= 100; i=i+5) {
			Vector3 currentPos = new Vector3 (i, 0, 0);
			Quaternion currentRot = Quaternion.identity;
			GameObject currentCube = Instantiate (cubePrefab, currentPos, currentRot);
		}

		// Loops | While (create 20 cubes spaced apart on z axis 5 units)
		int currentValue = 0;
		int endValue = 100;
		while(currentValue > endValue){
			currentValue = currentValue + 5;
			Vector3 meliseVector = new Vector3 (0, 0, currentValue);
			Quaternion currentRotation = Quaternion.identity;
			GameObject name = Instantiate (cubePrefab, meliseVector, currentRotation);
		}

		// Embeded loops (create a grid of 20 cubes above the current cubes)
		float spacing = 1.5f;
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				Vector3 kimVec = new Vector3 (i*spacing, 4, j*spacing); // Unity Z axis is Y axis
				Quaternion kimQat = Quaternion.identity;
				GameObject kimCube = Instantiate (cubePrefab, kimVec, kimQat);
				cubeArray [i, j] = kimCube;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		// Conditional statement
		if (doIPrintToTheConsole == true) {
			Debug.Log (textValue); // Action
		}

		// Move each cube up and down 10 units
		float spacing = 1.5f;
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				GameObject currentCube = cubeArray [i, j];
				currentCube.GetComponent<Transform> ().position = new Vector3 (i*spacing, 10, j*spacing); // Moves cube 10 units up
				currentCube.GetComponent<MeshRenderer>().material.color = new Color(255,0,0);
			}
		}
	}
}