using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PostProcessing;


public class mainMenuController : MonoBehaviour//this script controls the main menu
{
    //Button Variables
    [Header("Button Variables")] // these are all variables for the buttons 
    private bool pageMoving = false; // level select screen moving
    private string directionOfPageMove = ""; 
    private int currentPage = 1;
    public int pageMoveSpeed;
    private Vector3 currentPagePosition;
    private Vector3 targetPagePosition;
    public GameObject ButtonHolder;
    public GameObject upButton;
    public GameObject upButton2;
    public GameObject downButton;
    public GameObject downButton2;
    public GameObject mainMenuObjects;
    public GameObject optionsObjects;
    public GameObject levelSelectOptions;
    public GameObject storageLevelsObjects;
    public GameObject creditsObjects;
    public GameObject backButton;
    public GameObject startOrContinueText;
    public List<GameObject> levelButtons = new List<GameObject>();
    public AudioSource dingSound;
    public AudioSource noiseSound;

    private AudioSource buttonNoise;
    //Camera Variables
    public GameObject cameraHolder; //object that holds the camera
    public float cameraMoveSpeed;
    public Camera mainCamera;
    private string PPSetting; //post processing settings
    
    //States
    public enum pageChangeState //level select movement states
    {
        idle,
        bounceForward,
        move,
        bounceBack
    }

    private pageChangeState currentPageChangeState = pageChangeState.idle;

    private enum mainMenuState //main menu state
    {
        opening,
        main,
        levelSelect,
        options
    }

    private mainMenuState currentMainMenuState = mainMenuState.opening;
    private CompletionKeeper completionKeeper; //keeps track of how many levels completed
    //Noise static variables
    public GameObject noiseObject; //noise variables for the static when loading new scene
    public GameObject noiseBar;
    private float noiseTimer = 1f;
    public noiseSpriteData noiseSprites;
    private float barMoveSpeed = 10f;
    //loading variables
    private bool continuingToNextLevel = false; //for loading levels 
    private bool loadingLevel = false;
    private int whichLevelToLoad;
    

	// Use this for initialization
	void Start ()
	{
        //assign dynamic variables and setting post processing settings
	    buttonNoise = gameObject.GetComponent<AudioSource>();
	    completionKeeper = GameObject.FindGameObjectWithTag("CompletionKeeper").GetComponent<CompletionKeeper>();
	    PPSetting = PlayerPrefs.GetString("PPSetting", "max");
	    if (PPSetting=="max")
	    {
	        completionKeeper.currentPostProcessingSettings = CompletionKeeper.PostProcessingSettings.max;
	        mainCamera.GetComponent<PostProcessingBehaviour>().profile = completionKeeper.mainMenuPPProfileMax;
        }
        else if (PPSetting=="min")
	    {
	        completionKeeper.currentPostProcessingSettings = CompletionKeeper.PostProcessingSettings.min;
	        mainCamera.GetComponent<PostProcessingBehaviour>().profile = completionKeeper.mainMenuPPProfileMin;
        }
        else if (PPSetting=="none")
	    {
	        completionKeeper.currentPostProcessingSettings = CompletionKeeper.PostProcessingSettings.none;
	        mainCamera.GetComponent<PostProcessingBehaviour>().profile = completionKeeper.mainMenuPPProfileNone;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.L))
	    {
            unlockAllLevels();
	    }
	    if (continuingToNextLevel!=true&&loadingLevel!=true)// if the game isnt loading into another scene
	    {
            if (currentMainMenuState == mainMenuState.opening) 
            {
                if (completionKeeper.howManyLevelsCompleted == 0)//if the player hasnt completed any levels, change the top left button from continue to start
                {
                    startOrContinueText.GetComponent<TextMeshProUGUI>().text = "Start";
                }
            }
            else if (currentMainMenuState == mainMenuState.main)
            {

            }
            else if (currentMainMenuState == mainMenuState.levelSelect)// level select screen
            {
                Debug.Log(ButtonHolder.GetComponent<RectTransform>().transform.localPosition.y);
                if (pageMoving == true)//moving the level buttons
                {

                    if (directionOfPageMove == "up")//moving up 
                    {
                        if (currentPageChangeState == pageChangeState.idle)// move page change onto the first moving state
                        {
                            currentPageChangeState = pageChangeState.bounceForward;
                        }
                        else if (currentPageChangeState == pageChangeState.bounceForward)//move the page down slightly before moving up to give it a little bounce
                        {
                            ButtonHolder.GetComponent<RectTransform>().Translate(0, pageMoveSpeed * Time.deltaTime, 0);
                            if (ButtonHolder.GetComponent<RectTransform>().transform.localPosition.y > targetPagePosition.y)
                            {
                                targetPagePosition.y = currentPagePosition.y - 650;
                                currentPageChangeState = pageChangeState.move;
                            }
                        }
                        else if (currentPageChangeState == pageChangeState.move)//move the page up
                        {
                            cameraHolder.transform.Translate(0, cameraMoveSpeed * Time.deltaTime, 0);
                            ButtonHolder.GetComponent<RectTransform>().Translate(0, -pageMoveSpeed * Time.deltaTime, 0);
                            if (ButtonHolder.GetComponent<RectTransform>().transform.localPosition.y < targetPagePosition.y)
                            {
                                targetPagePosition.y = currentPagePosition.y - 600;
                                currentPageChangeState = pageChangeState.bounceBack;
                            }
                        }
                        else if (currentPageChangeState == pageChangeState.bounceBack)//move it down a bit after moving to give more bounce
                        {
                            ButtonHolder.GetComponent<RectTransform>().Translate(0, pageMoveSpeed * Time.deltaTime, 0);
                            if (ButtonHolder.GetComponent<RectTransform>().transform.localPosition.y > targetPagePosition.y)
                            {
                                currentPage += 1;
                                directionOfPageMove = "";
                                pageMoving = false;
                                ButtonHolder.GetComponent<RectTransform>().localPosition = new Vector3(currentPagePosition.x, currentPagePosition.y - 600, currentPagePosition.z);

                                downButton.GetComponent<Button>().interactable = true;
                                downButton.GetComponent<Image>().enabled = true;
                                downButton2.GetComponent<Image>().enabled = true;
                                if (currentPage < 4)//make sure the player cant move up further then there is pages
                                {
                                    upButton.GetComponent<Button>().interactable = true;
                                    upButton.GetComponent<Image>().enabled = true;
                                    upButton2.GetComponent<Image>().enabled = true;
                                }
                                currentPageChangeState = pageChangeState.idle;
                            }
                        }
                    }
                    else if (directionOfPageMove == "down")//moving down
                    {
                        if (currentPageChangeState == pageChangeState.idle)// move page change onto the first moving state
                        {
                            currentPageChangeState = pageChangeState.bounceForward;
                        }
                        else if (currentPageChangeState == pageChangeState.bounceForward)//move the page up slightly before moving down to give it a little bounce
                        {
                            ButtonHolder.GetComponent<RectTransform>().Translate(0, -pageMoveSpeed * Time.deltaTime, 0);
                            if (ButtonHolder.GetComponent<RectTransform>().transform.localPosition.y < targetPagePosition.y)
                            {
                                targetPagePosition.y = currentPagePosition.y + 650;
                                currentPageChangeState = pageChangeState.move;
                            }
                        }
                        else if (currentPageChangeState == pageChangeState.move)//move down
                        {
                            cameraHolder.transform.Translate(0, -cameraMoveSpeed * Time.deltaTime, 0);
                            ButtonHolder.GetComponent<RectTransform>().Translate(0, pageMoveSpeed * Time.deltaTime, 0);
                            if (ButtonHolder.GetComponent<RectTransform>().transform.localPosition.y > targetPagePosition.y)
                            {
                                targetPagePosition.y = currentPagePosition.y + 600;
                                currentPageChangeState = pageChangeState.bounceBack;
                            }
                        }
                        else if (currentPageChangeState == pageChangeState.bounceBack)//move the page up slightly after moving down to give it a little bounce
                        {
                            ButtonHolder.GetComponent<RectTransform>().Translate(0, -pageMoveSpeed * Time.deltaTime, 0);
                            if (ButtonHolder.GetComponent<RectTransform>().transform.localPosition.y < targetPagePosition.y)
                            {
                                currentPage -= 1;
                                directionOfPageMove = "";
                                pageMoving = false;
                                ButtonHolder.GetComponent<RectTransform>().localPosition = new Vector3(currentPagePosition.x, currentPagePosition.y + 600, currentPagePosition.z);
                                upButton.GetComponent<Button>().interactable = true;
                                upButton.GetComponent<Image>().enabled = true;
                                upButton2.GetComponent<Image>().enabled = true;
                                if (currentPage > 1)//make sure the player can't move down if they on bottom page
                                {
                                    downButton.GetComponent<Button>().interactable = true;
                                    downButton.GetComponent<Image>().enabled = true;
                                    downButton2.GetComponent<Image>().enabled = true;
                                }
                                currentPageChangeState = pageChangeState.idle;
                            }
                        }
                    }
                }
            }
            else if (currentMainMenuState == mainMenuState.options)
            {

            }
        }

	    if (loadingLevel==true)//if level is loading trigger noise and then load whichever level button was clicked
	    {
	        if (noiseObject.activeSelf == false)
	        {
                noiseSound.Play();
	            noiseObject.SetActive(true);
	        }

	        if (noiseObject.GetComponent<Image>().sprite == noiseSprites.noise1)
	        {
	            noiseObject.GetComponent<Image>().sprite = noiseSprites.noise2;
	        }
	        else if (noiseObject.GetComponent<Image>().sprite == noiseSprites.noise2)
	        {
	            noiseObject.GetComponent<Image>().sprite = noiseSprites.noise3;
	        }
	        else if (noiseObject.GetComponent<Image>().sprite == noiseSprites.noise3)
	        {
	            noiseObject.GetComponent<Image>().sprite = noiseSprites.noise4;
	        }
	        else
	        {
	            noiseObject.GetComponent<Image>().sprite = noiseSprites.noise1;
	        }

	        noiseBar.GetComponent<RectTransform>().Translate(0, -barMoveSpeed * Time.deltaTime, 0);

	        noiseTimer -= Time.deltaTime;
	        if (noiseTimer < 0)
	        {
	            if (whichLevelToLoad==1)
	            {
	                SceneManager.LoadScene("IntroLevel");
                }
	            SceneManager.LoadScene(whichLevelToLoad);
	        }
        }

	    if (continuingToNextLevel == true) //trigger noise, then load the level that the player has gotten up to    
	    {
	        if (noiseObject.activeSelf == false)
	        {
                noiseSound.Play();
	            noiseObject.SetActive(true);
	        }

	        if (noiseObject.GetComponent<Image>().sprite == noiseSprites.noise1)
	        {
	            noiseObject.GetComponent<Image>().sprite = noiseSprites.noise2;
	        }
	        else if (noiseObject.GetComponent<Image>().sprite == noiseSprites.noise2)
	        {
	            noiseObject.GetComponent<Image>().sprite = noiseSprites.noise3;
	        }
	        else if (noiseObject.GetComponent<Image>().sprite == noiseSprites.noise3)
	        {
	            noiseObject.GetComponent<Image>().sprite = noiseSprites.noise4;
	        }
	        else
	        {
	            noiseObject.GetComponent<Image>().sprite = noiseSprites.noise1;
	        }

	        noiseBar.GetComponent<RectTransform>().Translate(0, -barMoveSpeed * Time.deltaTime, 0);

	        
            noiseTimer -= Time.deltaTime;
	        if (noiseTimer < 0)
	        {
	            if (completionKeeper.howManyLevelsCompleted+1==1)
	            {
                    SceneManager.LoadScene("IntroLevel");
                }
	            else
	            {
	                SceneManager.LoadScene(completionKeeper.howManyLevelsCompleted + 1);
                }
	            
	        }
        }
	    
	    
	}

    public void LoadLevel(int levelToLoad) //load level from level select buttons
    {
        loadingLevel = true;
        dingSound.Play();
        whichLevelToLoad = levelToLoad;
        foreach (GameObject button in levelButtons)//make the buttons un-interactable when loading
        {
            button.GetComponent<Button>().interactable = false;
        }
    }

    public void MoveUpPage() //move the level select page up
    {
        if (pageMoving==false)
        {
            upButton.GetComponent<Button>().interactable = false;
            upButton.GetComponent<Image>().enabled = false;
            upButton2.GetComponent<Image>().enabled = false;
            downButton.GetComponent<Button>().interactable = false;
            downButton.GetComponent<Image>().enabled = false;
            downButton2.GetComponent<Image>().enabled = false;
            directionOfPageMove = "up";
            currentPagePosition = ButtonHolder.GetComponent<RectTransform>().transform.localPosition;
            targetPagePosition = new Vector3(currentPagePosition.x, currentPagePosition.y+50, currentPagePosition.z);
            pageMoving = true;
        }
        
    }

    public void MoveDownPage() //move the level select page down
    {
        if (pageMoving == false)
        {
            upButton.GetComponent<Button>().interactable = false;
            upButton.GetComponent<Image>().enabled = false;
            upButton2.GetComponent<Image>().enabled = false;
            downButton.GetComponent<Button>().interactable = false;
            downButton.GetComponent<Image>().enabled = false;
            downButton2.GetComponent<Image>().enabled = false;
            directionOfPageMove = "down";
            
            currentPagePosition = ButtonHolder.GetComponent<RectTransform>().transform.localPosition;
            targetPagePosition = new Vector3(currentPagePosition.x, currentPagePosition.y-50 , currentPagePosition.z);
            pageMoving = true;
        }
    }

    public void changeToLevelSelect() // open level select menu
    {
        completionKeeper.RestoreDataFromPlayerPrefBackup();
        mainMenuObjects.SetActive(false);
        levelSelectOptions.SetActive(true);
        for (int i = levelButtons.Count - 1; i > (completionKeeper.howManyLevelsCompleted); i--)
        {
            Debug.Log(i);
            
            levelButtons[i].GetComponent<Button>().interactable = false;
        }
        currentMainMenuState = mainMenuState.levelSelect;
    }

    public void openStorageLevels()
    {
        storageLevelsObjects.SetActive(true);
        levelSelectOptions.SetActive(false);
    }
    public void openSecurityLevels()
    {
        storageLevelsObjects.SetActive(true);
        levelSelectOptions.SetActive(false);
    }
    public void openOfficeLevels()
    {
        storageLevelsObjects.SetActive(true);
        levelSelectOptions.SetActive(false);
    }
    public void openRnDLevels()
    {
        storageLevelsObjects.SetActive(true);
        levelSelectOptions.SetActive(false);
    }
    public void openProductionLevels()
    {
        storageLevelsObjects.SetActive(true);
        levelSelectOptions.SetActive(false);
    }
    public void openHeadOfficeLevels()
    {
        storageLevelsObjects.SetActive(true);
        levelSelectOptions.SetActive(false);
    }

    public void BackToAreaSelect()
    {
        storageLevelsObjects.SetActive(false);
        levelSelectOptions.SetActive(true);
    }

    public void backToMainMenu() //go back to main menu from other menus
    {
        mainMenuObjects.SetActive(true);
        if (completionKeeper.howManyLevelsCompleted == 0)
        {
            startOrContinueText.GetComponent<TextMeshProUGUI>().text = "Start";
        }
        levelSelectOptions.SetActive(false);
        optionsObjects.SetActive(false);
        backButton.SetActive(false);
        downButton.SetActive(false);
        currentMainMenuState = mainMenuState.main;
    }

    public void continueToNextLevel() //load the next level that the player should play
    {
        dingSound.Play();
        continuingToNextLevel = true;
        foreach (GameObject button in levelButtons)
        {
            button.GetComponent<Button>().interactable = false;
        }
    }

    public void quit() //quit the game
    {
        PlayerPrefs.SetInt("levelsCompleted",completionKeeper.howManyLevelsCompleted);
        Application.Quit();
    }

    public void optionsOpen() //open the options menu
    {
        mainMenuObjects.SetActive(false);
        optionsObjects.SetActive(true);
        currentMainMenuState = mainMenuState.options;
    }

    public void deleteData() //delete all saved data
    {
        PlayerPrefs.SetInt("levelsCompleted",0);
        PlayerPrefs.SetString("playerName","");
        completionKeeper.howManyLevelsCompleted = 0;
    }

    public void unlockAllLevels() // unlock all levels for player
    {
        PlayerPrefs.SetInt("levelsCompleted", 60);
        completionKeeper.howManyLevelsCompleted = 60;
        
    }

    public void openCredits() // open the credits page
    {
        optionsObjects.SetActive(false);
        creditsObjects.SetActive(true);
    }

    public void backToOptions() //go back to the options menu from the credits menu
    {
        optionsObjects.SetActive(true);
        creditsObjects.SetActive(false);
    }

    //change the post processing settings

    public void setPPtoMax()
    {
        completionKeeper.currentPostProcessingSettings = CompletionKeeper.PostProcessingSettings.max;
        mainCamera.GetComponent<PostProcessingBehaviour>().profile = completionKeeper.mainMenuPPProfileMax;
        PlayerPrefs.SetString("PPSetting","max");
    }

    public void setPPtoMin()
    {
        completionKeeper.currentPostProcessingSettings = CompletionKeeper.PostProcessingSettings.min;
        mainCamera.GetComponent<PostProcessingBehaviour>().profile = completionKeeper.mainMenuPPProfileMin;
        PlayerPrefs.SetString("PPSetting", "min");
    }

    public void setPPtoNone()
    {
        completionKeeper.currentPostProcessingSettings = CompletionKeeper.PostProcessingSettings.none;
        mainCamera.GetComponent<PostProcessingBehaviour>().profile = completionKeeper.mainMenuPPProfileNone;
        PlayerPrefs.SetString("PPSetting", "none");
    }

    public void muteMusic()
    {
        if (completionKeeper.gameObject.GetComponent<AudioSource>().mute == true)
        {
            completionKeeper.gameObject.GetComponent<AudioSource>().mute = false;
            completionKeeper.mutedMusic = false;
        }
        else
        {
            completionKeeper.gameObject.GetComponent<AudioSource>().mute = true;
            completionKeeper.mutedMusic = true;
        }
        completionKeeper.BackupDataToPlayerPrefs();
        

    }

    public void toggleLasers()
    {
        if (completionKeeper.toggleLasers==true)
        {
            completionKeeper.toggleLasers = false;
        }
        else
        {
            completionKeeper.toggleLasers = true;
        }
        completionKeeper.BackupDataToPlayerPrefs();
    }

    public void ButtonSound()
    {
        buttonNoise.Play();
    }
}
