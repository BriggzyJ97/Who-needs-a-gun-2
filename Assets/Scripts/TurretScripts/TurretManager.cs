using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour {//This script manages all of the different turrets 


    public List<GameObject> turretList = new List<GameObject>(); //list of all the turrets in the level

    public float turretCooldown; //
    private float turretDuration = 0; // this is a temp variable for shooting
    public float maxTurretDuration; // this is how often the turret can shoot

    private CompletionKeeper completionKeeper;

    public enum turretStates //different states of the turret 
    {
        shooting,
        idle
    }

    public turretStates globalTurretState = turretStates.idle;

	// Use this for initialization
	void Start ()
	{
	    completionKeeper = GameObject.FindGameObjectWithTag("CompletionKeeper").GetComponent<CompletionKeeper>();
	    if (completionKeeper!=null)
	    {
	        if (completionKeeper.toggleLasers==false)
	        {
	            foreach (GameObject turret in turretList)
	            {
	                turret.GetComponent<TurretShooter>().isTargettingLaserOn = false;
	            }
            }
	        else
	        {
	            foreach (GameObject turret in turretList)
	            {
	                turret.GetComponent<TurretShooter>().isTargettingLaserOn = true;
	            }
            }
	    }
	}
	
	// Update is called once per frame
	void Update () {
	    if (turretDuration>0)
	    {
	        turretDuration -= Time.deltaTime;
	    }

	    if (Input.GetMouseButtonDown(0)&&turretDuration<=0&&gameObject.GetComponent<GameStateManager>().isGamePaused==false) //when the player clicks the mouse the turrets shoot
	    {
	        foreach (GameObject turret in turretList)//make all turrets in list of turrets shoot
	        {
	            if (turret.transform.parent.transform.parent.GetComponent<TurretTurner>().turretDown==false)
	            {
	                turret.GetComponent<TurretShooter>().thisTurretsState = turretStates.shooting;
	                turretDuration = maxTurretDuration;
                }
	            
	        }
	    }

	    if (AreAllTurretsDead()==true&&gameObject.GetComponent<GameStateManager>().currentGameState==GameStateManager.GameState.levelPlaying)//changes the gameState to win if all the turrets are down
	    {
	        gameObject.GetComponent<GameStateManager>().currentGameState = GameStateManager.GameState.levelWin;
	    }
	}

    private bool AreAllTurretsDead()//checks if all of the turrets are dead and returns true if they are
    {
        for (int i = 0; i < turretList.Count; i++)
        {
            if (turretList[i].transform.parent.parent.GetComponent<TurretTurner>().turretDown==false&& turretList[i].transform.parent.parent.tag!= "WallTurret")
            {
                return false;
            }
        }

        foreach (GameObject turret in turretList)
        {
            turret.transform.parent.parent.GetComponent<TurretTurner>().turretDownTimer = 100f;
        }
        return true;
    }
}
