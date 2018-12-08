using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Assertions.Must;

public class DialogueManager : MonoBehaviour {

    public enum DialogueState
    {
        preDialogue,
        zoomAndFade,
        menu,
        chat,
        quitDialogue,
    }

    public DialogueState CurrentDialogueState = DialogueState.preDialogue;

    private bool inDialogue = false;

    private GameStateManager gameManager;

    private GameObject mainCamera;
    private float cameraSpeed = 10f;
    private GameObject UICanvas;

    public Image fade;
    private float alpha = 0;

    public GameObject dialogueMenu;
    public GameObject dialogueChat;
    public TextMeshProUGUI newMessageText;

    public Button OpenChatButton;
    public TextMeshProUGUI openChatText;

    private bool messagesRead = false;
    private bool messagesLocked = false;
    private float newMessageTimer = 0f;
    private float newMessageYellowChange = 1f;
    private bool newMessageIncreasing = false;

    public List<TextMeshProUGUI> chatTextList = new List<TextMeshProUGUI>();
    public List<string> chatTexts = new List<string>();
    public int chatPosition;

    private bool chatCursorBlinking = true;
    public GameObject typeCursor;
    private float cursorBlinkTime = 0.25f;

    public float letterPause = 0.1f;
    private bool hasTextChanged;

    public bool hasTriggered = true;
    public bool hasTextTyped = false;

    private Color greenText = new Color(0,1,0,1);
    private Color redText = new Color(1,0,0,1);

    public GameObject TwoChatOptions;
    public GameObject ThreeChatOptions;
    public GameObject InputChatOptions;
    public GameObject chatTextHolder;

    private bool responseTrigger = false;
    private int numberOfResponses = 0;
    private int whichResponseNeeded;

    private bool inputtingText = false;
    public TextMeshProUGUI inputTextBox;
    public GameObject inputTypeCursor;
    private float inputCursorBlinkTime = 0.25f;
    private bool inputCursorBlinking = true;
    private float blinkPause = 1f;

    private CompletionKeeper completionTracker;

    public doorControl door;

    public string playerName = "";
    private bool chatHacked = false;
    private int chatHackPos = 0;
    private float chatHackTime = 0.2f;
    private bool skipText = false;

    private AudioSource audioSource;
    public AudioSource morseCodeSoundSource;
    public AudioSource demonHackSoundSource;
    public AudioSource buttonSoundSource;



    // Use this for initialization
    void Start ()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStateManager>();
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        UICanvas = GameObject.FindGameObjectWithTag("UICanvas");
        completionTracker = GameObject.FindGameObjectWithTag("CompletionKeeper").GetComponent<CompletionKeeper>();
        
	}
	
	// Update is called once per frame
	void Update () {
	    if (inDialogue == true)
	    {
	        
	        if (CurrentDialogueState==DialogueState.preDialogue)
	        {
	            CurrentDialogueState = DialogueState.zoomAndFade;
	        }else 
	        if (CurrentDialogueState == DialogueState.zoomAndFade)
	        {
                mainCamera.transform.Translate(0,0, cameraSpeed * Time.deltaTime);
	            mainCamera.GetComponent<Camera>().fieldOfView += cameraSpeed*2 * Time.deltaTime;
	            alpha += Time.deltaTime / 2;
                fade.color = new Color(0,0,0,alpha);
                mainCamera.transform.GetChild(0).GetComponent<Camera>().fieldOfView += cameraSpeed * 2 * Time.deltaTime;
                if (fade.color.a >0.97f)
                {
                    UICanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
                    mainCamera.GetComponent<cameraModeSwap>().SwapCameraMode();
                    fade.color = new Color(0, 0, 0, 1);
                    dialogueMenu.SetActive(true);
	                CurrentDialogueState = DialogueState.menu;
	            }
	        }else if (CurrentDialogueState == DialogueState.menu)
	        {
	            if (completionTracker != null)
	            {
	                completionTracker.GetComponent<AudioSource>().volume = 0.02f;
	            }
                if (messagesLocked==false)
	            {
	                if (messagesRead == false)
	                {
	                    newMessageTimer += Time.deltaTime;
	                    if (newMessageTimer > 0.5f && newMessageTimer < 1.2f)
	                    {
	                        newMessageText.text = "1 new message";
	                    }
	                    if (newMessageTimer > 1.2f)
	                    {
	                        newMessageText.text = "2 new message";
	                    }

	                    if (newMessageIncreasing == false)
	                    {
	                        newMessageYellowChange -= Time.deltaTime * 2;
	                        if (newMessageYellowChange < 0.1f)
	                        {
	                            newMessageIncreasing = true;
	                        }
	                    }
	                    else
	                    {
	                        newMessageYellowChange += Time.deltaTime * 2;
	                        if (newMessageYellowChange > 0.95f)
	                        {
	                            newMessageIncreasing = false;
	                        }
	                    }

	                    newMessageText.color = new Color(1, 1, newMessageYellowChange);

	                }
	                else
	                {
	                    newMessageText.text = "";
	                }
                }
	            else
	            {
                    //Debug.Log("messages locked");
	                if (OpenChatButton.interactable==true)
	                {
	                    openChatText.color = redText;
	                    OpenChatButton.interactable = false;
	                    newMessageText.text = "";
                    }
	            }
	            
	            
	        }else if (CurrentDialogueState == DialogueState.chat)
	        {
	            if (Input.GetKeyDown(KeyCode.Mouse0))
	            {
	                Tap();
                }
	            if (chatCursorBlinking==true)
	            {
	                cursorBlinkTime -= Time.deltaTime;
	                if (cursorBlinkTime < 0)
	                {
	                    if (typeCursor.activeSelf == false)
	                    {
	                        typeCursor.SetActive(true);
	                    }
	                    else
	                    {
	                        typeCursor.SetActive(false);
	                    }
	                    cursorBlinkTime = 0.25f;
	                }
                }

	            if (responseTrigger==true)
	            {
                    //Debug.Log("makeResponse");
	                if (chatPosition >= 12)
	                {
	                    chatTextHolder.GetComponent<RectTransform>().localPosition = new Vector2(chatTextHolder.GetComponent<RectTransform>().localPosition.x, chatTextHolder.GetComponent<RectTransform>().localPosition.y + 35);
	                }

	                
                    StartCoroutine(TypeText(chatTexts[whichResponseNeeded],chatTextList[chatPosition]));
	                if (whichResponseNeeded == 23)
	                {
                        demonHackSoundSource.Play();
	                    chatHacked = true;
	                }
                    if (whichResponseNeeded==5)
	                {
	                    whichResponseNeeded = 6;
	                }else if (whichResponseNeeded==6)
	                {
	                    whichResponseNeeded = 7;
	                }else if (whichResponseNeeded==13)
	                {
	                    whichResponseNeeded = 14;
	                }else if (whichResponseNeeded==14)
	                {
	                    whichResponseNeeded = 15;
	                }else if (whichResponseNeeded==11)
	                {
	                    whichResponseNeeded = 15;
	                }else if (whichResponseNeeded==12)
	                {
	                    whichResponseNeeded = 15;
	                }else if (whichResponseNeeded == 16)
	                {
	                    whichResponseNeeded = 19;
                    }
	                else if (whichResponseNeeded == 19)
	                {
	                    whichResponseNeeded = 17;
	                }
	                else if (whichResponseNeeded == 17)
	                {
	                    whichResponseNeeded = 18;
	                }else if (whichResponseNeeded == 18)
	                {
	                    whichResponseNeeded = 21;
	                }
	                else if (whichResponseNeeded == 21)
	                {
	                    whichResponseNeeded = 22;
	                }
	                else if (whichResponseNeeded == 22)
	                {
	                    whichResponseNeeded = 23;
	                }


                    responseTrigger = false;
	            }

	            if (inputtingText==true)
	            {
	                foreach (char c in Input.inputString)
	                {
	                    if (c == '\b') // has backspace/delete been pressed?
	                    {

	                        if (inputTextBox.text.Length != 0)
	                        {
	                            inputTextBox.text = inputTextBox.text.Substring(0, inputTextBox.text.Length - 1);
	                        }
	                    }
	                    else if ((c == '\n') || (c == '\r')) // enter/return
	                    {
	                        print("User entered their name: " + inputTextBox.text);
                            Submit();
	                    }
	                    else if (inputTextBox.text.Length<10)
	                    {
	                        inputCursorBlinking = false;
	                        inputTextBox.text += c;
	                    }
	                }

	                if (inputCursorBlinking==true)
	                {
	                    inputCursorBlinkTime -= Time.deltaTime;
	                    if (inputCursorBlinkTime < 0)
	                    {
	                        if (inputTypeCursor.activeSelf == false)
	                        {
	                            inputTypeCursor.SetActive(true);
	                        }
	                        else
	                        {
	                            inputTypeCursor.SetActive(false);
	                        }
	                        inputCursorBlinkTime = 0.25f;
	                    }
                    }
	                else
	                {
                        inputTypeCursor.SetActive(false);
	                }
                }

	            if (chatHacked == true)
	            {
                    if(chatHackPos<=chatTextList.Count)
	                {
	                    if (chatTextList[chatHackPos].text == "")
	                    {
	                        messagesLocked = true;
                            demonHackSoundSource.Stop();
                            BackToConsole();
	                    }
	                    chatTextList[chatHackPos].color = redText;
	                    chatTextList[chatHackPos].text = "STOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPS";
                    }
	                chatHackTime -= Time.deltaTime;
	                if (chatHackTime<=0)
	                {

	                    chatHackPos++;
	                    chatHackTime = 0.2f;
	                }

	            }
	        }else if (CurrentDialogueState==DialogueState.quitDialogue)
	        {
	            dialogueMenu.SetActive(false);
                mainCamera.transform.Translate(0, 0, -cameraSpeed * Time.deltaTime);
	            mainCamera.GetComponent<Camera>().fieldOfView -= cameraSpeed * 2 * Time.deltaTime;
	            alpha -= Time.deltaTime / 2;
	            fade.color = new Color(0, 0, 0, alpha);
	            mainCamera.transform.GetChild(0).GetComponent<Camera>().fieldOfView -= cameraSpeed * 2 * Time.deltaTime;
	            if (fade.color.a < 0.03f)
	            {
	                
	                fade.color = new Color(0, 0, 0, 0);
	                gameManager.currentGameState = GameStateManager.GameState.levelPlaying;
	                CurrentDialogueState = DialogueState.preDialogue;
	                inDialogue = false;
	            }
            }
        }
	    else
	    {
	        completionTracker.GetComponent<AudioSource>().volume = 0.06f;
        }
	}

    public void Trigger()
    {
        inDialogue = true;
    }

    public void OpenChat()
    {
        messagesRead = true;
        dialogueMenu.SetActive(false);
        dialogueChat.SetActive(true);
        CurrentDialogueState = DialogueState.chat;

    }

    public void OpenDoor()
    {
        door.doorOpen = true;
        
    }

    public void ButtonSound()
    {
        buttonSoundSource.Play();
    }

    public void QuitConsole()
    {
        mainCamera.GetComponent<cameraModeSwap>().SwapCameraMode();
        UICanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        dialogueMenu.SetActive(false);
        CurrentDialogueState = DialogueState.quitDialogue;
    }

    public void BackToConsole()
    {
        dialogueMenu.SetActive(true);
        dialogueChat.SetActive(false);
        CurrentDialogueState = DialogueState.menu;
    }

    public void TwoChatOption1()
    {
        skipText = false;
        chatTextList[chatPosition].color = greenText;
        StartCoroutine(TypeText(chatTexts[0], chatTextList[chatPosition]));
        whichResponseNeeded = 4;
        TwoChatOptions.SetActive(false);
    }

    public void TwoChatOption2()
    {
        skipText = false;
        chatTextList[chatPosition].color = greenText;
        StartCoroutine(TypeText(chatTexts[1], chatTextList[chatPosition]));
        whichResponseNeeded = 5;
        TwoChatOptions.SetActive(false);
    }

    public void ThreeChatOption1()
    {
        skipText = false;
        chatTextList[chatPosition].color = greenText;
        StartCoroutine(TypeText(chatTexts[8], chatTextList[chatPosition]));
        whichResponseNeeded = 11;
        ThreeChatOptions.SetActive(false);
    }
    public void ThreeChatOption2()
    {
        skipText = false;
        chatTextList[chatPosition].color = greenText;
        StartCoroutine(TypeText(chatTexts[9], chatTextList[chatPosition]));
        whichResponseNeeded = 12;
        ThreeChatOptions.SetActive(false);
    }
    public void ThreeChatOption3()
    {
        skipText = false;
        chatTextList[chatPosition].color = greenText;
        StartCoroutine(TypeText(chatTexts[10], chatTextList[chatPosition]));
        whichResponseNeeded = 13;
        ThreeChatOptions.SetActive(false);
    }

    public void Submit()
    {

        playerName = inputTextBox.text;
        InputChatOptions.SetActive(false);
        whichResponseNeeded = 16;
        //responseTrigger = true;
        if (completionTracker!=null)
        {
            completionTracker.playername = playerName;
            completionTracker.UpdatePlayerName();
        }
        chatTexts[20] = "{"+playerName + "}#@";
        chatTexts[19] = "~£"+playerName+"$#@";
        chatTextList[chatPosition].color = greenText;
        StartCoroutine(TypeText(chatTexts[20], chatTextList[chatPosition]));

    }

    IEnumerator TypeText(string messageToType, TextMeshProUGUI textField)
    {
        hasTextTyped = true;
        foreach (char letter in messageToType.ToCharArray())
        {
            if (skipText==false)
            {


                if (letter.ToString() == "~")
                {
                    typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                        typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.x,
                        typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.y - 30);
                    chatCursorBlinking = true;
                    yield return WaitForSecondsOrTap(1f);



                }
                else if (letter.ToString() == "_")
                {
                    chatCursorBlinking = false;
                    typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                        typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.x,
                        typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.y - 30);
                }
                else if (letter.ToString() == "{")
                {
                    audioSource.Play();
                }
                else if (letter.ToString() == "}")
                {
                    audioSource.Stop();
                }
                else if (letter.ToString() == "£")
                {
                    morseCodeSoundSource.Play();
                }
                else if (letter.ToString() == "$")
                {
                    morseCodeSoundSource.Stop();
                }
                else if (letter.ToString() == ":")
                {
                    chatCursorBlinking = false;
                    typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                        typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.x,
                        typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.y - 30);
                }
                else if (letter.ToString() == ";")
                {
                    ThreeChatOptions.SetActive(true);
                }
                else if (letter.ToString() == "]")
                {
                    InputChatOptions.SetActive(true);
                    inputtingText = true;

                }
                else if (letter.ToString() == "[")
                {
                    chatHacked = true;
                }
                else if (letter.ToString() == "+")
                {
                    //letterPause = letterPause - 0.1f;
                    chatCursorBlinking = true;
                }
                else if (letter.ToString() == "@")
                {
                    numberOfResponses += 1;
                    responseTrigger = true;
                }
                else if (letter.ToString() == "#")
                {
                    //textTyperToTrigger.GetComponent<TextTypingController>().hasTriggered = true;
                    chatPosition++;
                }
                else
                {
                    chatCursorBlinking = false;
                    typeCursor.SetActive(false);
                    textField.text += letter;

                    yield return new WaitForSeconds(letterPause);

                }
            }
            else
            {
                if (letter.ToString() == "~")
                {



                }
                else if (letter.ToString() == "_")
                {
                    chatCursorBlinking = false;
                    typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                        typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.x,
                        typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.y - 30);
                }
                else if (letter.ToString() == "{")
                {
                    audioSource.Play();
                }
                else if (letter.ToString() == "}")
                {
                    audioSource.Stop();
                }
                else if (letter.ToString() == "£")
                {
                    morseCodeSoundSource.Play();
                }
                else if (letter.ToString() == "$")
                {
                    morseCodeSoundSource.Stop();
                }
                else if (letter.ToString() == ":")
                {
                    chatCursorBlinking = false;
                    typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                        typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.x,
                        typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.y - 30);
                }
                else if (letter.ToString() == ";")
                {
                    ThreeChatOptions.SetActive(true);
                }
                else if (letter.ToString() == "]")
                {
                    InputChatOptions.SetActive(true);
                    inputtingText = true;

                }
                else if (letter.ToString() == "[")
                {
                    chatHacked = true;
                }
                else if (letter.ToString() == "+")
                {
                    //letterPause = letterPause - 0.1f;
                    chatCursorBlinking = true;
                }
                else if (letter.ToString() == "@")
                {
                    numberOfResponses += 1;
                    responseTrigger = true;
                }
                else if (letter.ToString() == "#")
                {
                    //textTyperToTrigger.GetComponent<TextTypingController>().hasTriggered = true;
                    chatPosition++;
                }
                else
                {
                    chatCursorBlinking = false;
                    typeCursor.SetActive(false);
                    textField.text += letter;

                }
            }
        }

        skipText = false;
    }

    IEnumerator WaitForSecondsOrTap(float seconds)
    {
        blinkPause = seconds;
        while (blinkPause>0f)
        {
            blinkPause -= Time.deltaTime;
            yield return 0f;
        }
    }

    private void Tap()
    {
        blinkPause = 0;
        skipText = true;
    }
}
