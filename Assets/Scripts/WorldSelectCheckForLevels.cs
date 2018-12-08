using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSelectCheckForLevels : MonoBehaviour
{

    public int levelsNeededToBeCompletedForUnlock;
    private CompletionKeeper completionKeeper;

	// Use this for initialization
	void Start ()
	{
	    completionKeeper = GameObject.FindGameObjectWithTag("CompletionKeeper").GetComponent<CompletionKeeper>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void checkForUnlock()
    {
        if (completionKeeper.howManyLevelsCompleted>=levelsNeededToBeCompletedForUnlock)
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
    }
}
