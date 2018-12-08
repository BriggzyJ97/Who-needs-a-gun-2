using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDialogueLevelTrigger : MonoBehaviour
{

    public doorControl door;
    private GameStateManager gameManager;

	// Use this for initialization
	void Start ()
	{
	    gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStateManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (door.doorOpen==true)
            {
                gameManager.currentGameState = GameStateManager.GameState.levelWin;
            }
        }
    }
}
