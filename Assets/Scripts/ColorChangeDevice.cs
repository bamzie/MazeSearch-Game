using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeDevice : MonoBehaviour {
	public void Operate() { 	// Declare a method with the same name as the door script. 
		Color random = new Color(Random.Range(0f,1f),Random.Range(0f,1f), Random.Range(0f,1f)); //Generates random RGB values to chang color. 
			GetComponent<Renderer>().material.color = random; 	// The color is set in the material attached in the object. 
			} 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
