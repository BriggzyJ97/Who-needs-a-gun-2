using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CompletionKeeper : MonoBehaviour //This singleton keeps track of how many levels have been completed by the player and saves that in player prefs.
//also keeps track of settings
{
    public int howManyLevelsCompleted = 1;
    private static bool keeperExists = false;

    public enum PostProcessingSettings
    {
        max,
        min,
        none
    }

    //different post-processing options

    public PostProcessingSettings currentPostProcessingSettings = PostProcessingSettings.max;
    public PostProcessingProfile mainMenuPPProfileMax;
    public PostProcessingProfile mainMenuPPProfileMin;
    public PostProcessingProfile mainMenuPPProfileNone;
    public PostProcessingProfile cctvPPProfileMax;
    public PostProcessingProfile cctvPPProfileMin;
    public PostProcessingProfile cctvPPProfileNone;

    public bool toggleLasers = true;
    public bool mutedMusic;

    public string playername;

    void Awake()
    {// TO make sure there is only one of this script and that it carries on through scene changes.
        if (keeperExists==false)
        {
            DontDestroyOnLoad(this.gameObject);

            keeperExists = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        //Restores variable data from player preferences 
		RestoreDataFromPlayerPrefBackup();
        RestorePlayerName();
	    if (mutedMusic==true)
	    {
	        gameObject.GetComponent<AudioSource>().mute = true;
	    }
	}

    public void BackupDataToPlayerPrefs()
    {
        //Saves data to player prefs
        PlayerPrefs.SetInt("levelsCompleted",howManyLevelsCompleted);
        if (mutedMusic==true)
        {
            PlayerPrefs.SetInt("musicMuted",1);
        }
        else
        {
            PlayerPrefs.SetInt("musicMuted",0);
        }

        if (toggleLasers==true)
        {
            PlayerPrefs.SetInt("lasersOn",1);
        }
        else
        {
            PlayerPrefs.SetInt("lasersOn",0);
        }
        
    }

    public void RestoreDataFromPlayerPrefBackup()
    {
        //Gets data from player prefs
        howManyLevelsCompleted=PlayerPrefs.GetInt("levelsCompleted");
        if (PlayerPrefs.GetInt("musicMuted")==1)
        {
            mutedMusic = true;
        }
        else
        {
            mutedMusic = false;
        }

        if (PlayerPrefs.GetInt("lasersOn")==1)
        {
            toggleLasers = true;
        }
        else
        {
            toggleLasers = false;
        }
    }

    public void UpdatePlayerName()
    {
        PlayerPrefs.SetString("playerName", playername);
    }

    public void RestorePlayerName()
    {
        playername = PlayerPrefs.GetString("playerName");
    }
}
