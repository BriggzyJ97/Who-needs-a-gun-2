using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLighter : MonoBehaviour { //This script makes the buttons on the main menu light up if the button is interactable

    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    gameObject.GetComponent<Button>().interactable = gameObject.transform.parent.GetComponent<Button>().interactable;
	}
    
}
