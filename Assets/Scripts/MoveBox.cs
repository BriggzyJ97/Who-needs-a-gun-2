using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MoveBox : MonoBehaviour
{
    public bool isThisLeftBox = false;
    public railController railControlScript;

    

    void OnTriggerStay(Collider other)
    {
        if (other!=transform.parent)
        {
            if (other.tag == "Player")
            {
                if (isThisLeftBox == true)
                {
                    railControlScript.CurrentDirectionOfRailMovement = railController.railMovement.movingLeft;
                }
                else
                {
                    railControlScript.CurrentDirectionOfRailMovement = railController.railMovement.movingRight;
                }

                if (other.GetComponent<playerController>() != null)
                {
                    if (other.GetComponent<playerController>().isPlayerSprinting == true)
                    {
                        railControlScript.isPlayerSprinting = true;
                    }
                }
                else if (other.GetComponent<playerDouble>() != null)
                {
                    if (other.GetComponent<playerDouble>().isPlayerSprinting == true)
                    {
                        railControlScript.isPlayerSprinting = true;
                    }
                }
                else
                {
                    railControlScript.isPlayerSprinting = false;

                }
            }
        }
        
        
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other != transform.parent)
        {
            if (other.tag == "Player")
            {
                railControlScript.CurrentDirectionOfRailMovement = railController.railMovement.idle;
                railControlScript.isPlayerSprinting = false;
            }
        }

        
    }
}
