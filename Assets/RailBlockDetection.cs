using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailBlockDetection : MonoBehaviour {

    public bool isThisLeftBox = false;
    public railController railControlScript;
    
    void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Wall" || other.tag == "VerticleMirror" || other.tag == "HorizontalMirror" || other.tag == "Turret")
        {
            Debug.Log("entering wall 1");
            if (isThisLeftBox == true)
            {
                Debug.Log("entering wall L");
                railControlScript.isLeftBlocked = true;
            }
            else
            {
                Debug.Log("entering wall R");
                railControlScript.isRightBlocked = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wall" || other.tag == "VerticleMirror" || other.tag == "HorizontalMirror" || other.tag == "Turret")
        {
            if (isThisLeftBox == true)
            {
                railControlScript.isLeftBlocked = false;
            }
            else
            {
               railControlScript.isRightBlocked = false;
            }
        }
    }
}
