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

	// FUNCTIONS - exposure type name () { instruction go here }
	// Use this for initialization 
	void Start () {
		// Make a cube in the origin
		Vector3 position = new Vector3(0f,0f,0f);
		Quaternion rotation = Quaternion.identity;
		GameObject cubePrefabClone = Instantiate (cubePrefab, position, rotation);
	}

	// Update is called once per frame
	void Update () {
		//Conditional statement
		if (doIPrintToTheConsole == true) {
			Debug.Log (textValue); // Action
		}
	}
}