using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class cameraModeSwap : MonoBehaviour
{
    private bool inConsoleMode = false;

    public PostProcessingProfile consolePPP;
    private PostProcessingProfile mainPPP;

    private PostProcessingBehaviour PPBehaviour;

	// Use this for initialization
	void Start ()
	{
	    PPBehaviour = gameObject.GetComponent<PostProcessingBehaviour>();
	    mainPPP = PPBehaviour.profile;

	}

    public void SwapCameraMode()
    {
        if (inConsoleMode==true)
        {
            PPBehaviour.profile = mainPPP;
            gameObject.GetComponent<CRTDistortion>().enabled = false;
            inConsoleMode = false;

        }else if (inConsoleMode==false)
        {
            PPBehaviour.profile = consolePPP;
            gameObject.GetComponent<CRTDistortion>().enabled = true;
            inConsoleMode = true;
        }
    }
}
