using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skycraperrandomer : MonoBehaviour // this script randomly lights up windows of the buildings in the main menu
{

    private int illuminated;

	// Use this for initialization
	void Start ()
	{
	    illuminated = Random.Range(0, 10);
	    if (illuminated<=6)
	    {
            
            gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f)));
	    }
	}
	
}
