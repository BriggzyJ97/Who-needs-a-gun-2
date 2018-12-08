using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    private GameObject gameManager;
    private bool triggered = false;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("GameController");

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (triggered == false)
            {
                gameManager.GetComponent<GameStateManager>().currentGameState = GameStateManager.GameState.levelDialogue;
                triggered = true;
            }
            
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            triggered = false;

        }
    }
}
