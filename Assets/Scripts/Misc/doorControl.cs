using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorControl : MonoBehaviour // This script opens the door when another changes its doorOpen variable
{

    public bool doorOpen = false;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public Transform leftDoorTarget;
    public Transform rightDoorTarget;
    public float doorOpenSpeed;

    private AudioSource doorSound;
    private bool soundTriggered=false;

	// Use this for initialization
	void Start ()
	{
	    doorSound = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (doorOpen==true)
	    {
	        if (soundTriggered==false)
	        {
                doorSound.Play();
	            soundTriggered = true;
	        }
	        rightDoor.transform.position = Vector3.MoveTowards(rightDoor.transform.position, rightDoorTarget.position, doorOpenSpeed * Time.deltaTime);
            leftDoor.transform.position = Vector3.MoveTowards(leftDoor.transform.position, leftDoorTarget.position, doorOpenSpeed*Time.deltaTime);
	    }
	}
}
