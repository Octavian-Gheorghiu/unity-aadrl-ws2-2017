using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel : MonoBehaviour {

	// VARIABLES
	public int state = 0;
    private int futureState = 0;
    public int age = 0;
    int timeEndReference;
    private MaterialPropertyBlock props;

    // FUNCTIONS

    public void SetupVoxel()
    {
        timeEndReference = GameObject.Find("CA_Grid").GetComponent<CA_Grid>().timeEnd;
        props = new MaterialPropertyBlock();
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
	
	// Update function
	public void UpdateVoxel () {
		// Set the future state
		state = futureState;        
        // If voxel is alive update age
        if (state == 1)
        {
            age++;
        }
        // If voxel is death disable the game object mesh renderer and set age to zero
        if (state == 0)
        {
            age = 0;
        }
    }

    // Update the voxel display
    public void VoxelDisplay()
    {
        // Get the game object mesh renderer
        MeshRenderer renderer;
        renderer = gameObject.GetComponent<MeshRenderer>();
        if (state == 1)
        {            
            // Remap the color to age
            Color ageColor = new Color(Remap(age, 0, timeEndReference, 0.0f, 1.0f), 0, 0, 1);
            props.SetColor("_Color", ageColor);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }
        if(state == 0)
        {
            renderer.enabled = false;
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

    // Get the age of the voxel
	public void SetAge(int _age){
		age = _age;
	}

	// Get the state of the voxel
	public int GetState(){
		return state;
	}

	// Get the age of the voxel
	public int GetAge(){
		return age;
	}

    // Remap numbers
    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
