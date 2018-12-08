using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotate : MonoBehaviour // This script rotates the camera around in the main menu
{


    public float speed;//speed of camera rotation
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate(new Vector3(0,speed*Time.deltaTime,0));
	}
}
