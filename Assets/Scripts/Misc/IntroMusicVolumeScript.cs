using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMusicVolumeScript : MonoBehaviour
{

    private CompletionKeeper completionKeeper;

	// Use this for initialization
	void Start ()
	{
	    completionKeeper = GameObject.FindGameObjectWithTag("CompletionKeeper").GetComponent<CompletionKeeper>();
	    completionKeeper.gameObject.GetComponent<AudioSource>().volume = 0;
	}
	
}
