using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextTypingController : MonoBehaviour // this script controls typing text, both on the intro and on all levels
{

    public float letterPause = 0.2f;
    public float beginningPause = 10f;
    private string messageToType;
    public TextMeshProUGUI textField;
    private bool hasTextChanged;

    public Image textTypingCursor;
    private bool cursorBlinking = false;
    public float cursorBlinkTime = 0.25f;

    public bool hasTriggered = true;
    public bool hasTextTyped = false;

    //GlitchyTextVariables
    private Vector2 savedTextPos;
    public TextMeshProUGUI blueTextField;
    //public TextMeshProUGUI redTextField;
    private float randomOffsetX;
    private float randomOffsetY;
    //public Image GlitchImage;
    //private Vector2 glitchImagePos;
    public float GlitchinessOfText = 1f;
    public float ChromaticAbberationOffset = 1f;
    public Color GlitchTextBaseColor;
    public Color RedTextBaseColor;
    public bool VerticlePositionGlitching = false;

    public GameObject textTyperToTrigger;
    private float loadTimer = 3;
    private bool startToLoad = false;
    public string sceneToLoad;
    public GameObject noiseObject;
    public noiseSpriteData noiseSprites;
    public GameObject noiseBar;
    private float barMoveSpeed = 10f;

    public AudioClip thisTypingNoise;

    public AudioSource staticNoise;

    public enum TextType
    {
        whiteText,
        glitchyText,
        redText,
        greenText,
    }

    public TextType textType = TextType.whiteText;

    private AudioSource audioSource;

    private GameObject completionKeeper;

	// Use this for initialization
	void Start ()
	{
	    savedTextPos = textField.gameObject.GetComponent<RectTransform>().anchoredPosition;
	    //glitchImagePos = GlitchImage.gameObject.GetComponent<RectTransform>().anchoredPosition;
	    messageToType = textField.text;
	    textField.text = "";
	    audioSource = gameObject.GetComponent<AudioSource>();
        blueTextField.text = "";
	    //redTextField.text = "";
	    if (textType == TextType.glitchyText)
	    {
	        blueTextField.enabled = true;
	        //redTextField.enabled = true;
            //GlitchImage.enabled = true;
	        textField.color = GlitchTextBaseColor;
	        //textTypingCursor.color = GlitchTextBaseColor;
	    }else if (textType == TextType.redText)
	    {
	        
	        //GlitchImage.enabled = true;
            blueTextField.enabled = true;
	        textField.color = RedTextBaseColor;
	        //textTypingCursor.color = RedTextBaseColor;
	    }
	    if (hasTriggered)
	    {
	        StartCoroutine(TypeText());
        }
        completionKeeper = GameObject.FindGameObjectWithTag("CompletionKeeper");
	    
	}

    void Update()
    {

        if (cursorBlinking)
        {
            cursorBlinkTime -= Time.deltaTime;
            if (cursorBlinkTime<0)
            {
                if (textTypingCursor.enabled==false)
                {
                    textTypingCursor.enabled = true;
                }
                else
                {
                    textTypingCursor.enabled = false;
                }
                cursorBlinkTime = 0.25f;
            }
        }

        if (textType == TextType.glitchyText)
        {
            randomOffsetX = Random.Range(-GlitchinessOfText, GlitchinessOfText);
            if (VerticlePositionGlitching==true)
            {
                randomOffsetY = Random.Range(-GlitchinessOfText, GlitchinessOfText);
            }

            //ChromaticAbberationOffset = Random.Range(0.5f, 2f);
            //GlitchImage.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(glitchImagePos.x,glitchImagePos.y+Random.Range(-15f,15f));
            textField.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(savedTextPos.x+randomOffsetX,savedTextPos.y+randomOffsetY);
            blueTextField.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(savedTextPos.x + randomOffsetX - (ChromaticAbberationOffset/2), savedTextPos.y + randomOffsetY);
            //redTextField.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(savedTextPos.x + randomOffsetX + ChromaticAbberationOffset, savedTextPos.y + randomOffsetY);
        }else if (textType==TextType.redText)
        {
            //ChromaticAbberationOffset = Random.Range(0.5f, 2f);
            //GlitchImage.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(glitchImagePos.x+Random.Range(-3f,3f), glitchImagePos.y + Random.Range(-15f, 15f));
            //float ChromaticAbberationOffsetY = Random.Range(-1.5f, 1.5f);
            blueTextField.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(savedTextPos.x + randomOffsetX - (ChromaticAbberationOffset / 2), savedTextPos.y + randomOffsetY);
        }
        if (hasTriggered && hasTextTyped==false)   
        {
            StartCoroutine(TypeText());
        }

        if (startToLoad==true)
        {
            
            loadScene();
        }

        if (SceneManager.GetActiveScene().name=="IntroLevel")
        {
            if (Input.GetKeyDown(KeyCode.Escape)&&startToLoad==false)
            {
                loadTimer = 2;      
                startToLoad = true;
            }
        }
    }

    IEnumerator TypeText()
    {
        hasTextTyped = true;
        foreach (char letter in messageToType.ToCharArray())
        {
            if (letter.ToString() == "~")
            {
                textTypingCursor.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(textTypingCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.x, textTypingCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.y-30);
                cursorBlinking = true;
                yield return new WaitForSeconds(beginningPause);
            }else if (letter.ToString()=="_")
            {
                textTypingCursor.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(textTypingCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.x, textTypingCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.y - 30);
            }else if (letter.ToString()=="{")
            {
                audioSource.clip = thisTypingNoise;
                if (thisTypingNoise.name=="morsecodeConsole1"|| thisTypingNoise.name == "morsecodeIntro")
                {
                    audioSource.volume = 0.015f;
                }
                audioSource.Play();
            }
            else if (letter.ToString() == "}")
            {
                audioSource.Stop();
                audioSource.volume = 0.1f;
            }
            else if (letter.ToString() == "-")
            {
                letterPause = letterPause + 0.1f;
            }
            else if (letter.ToString() == "+")
            {
                letterPause = letterPause - 0.1f;
            }else if (letter.ToString()=="@")
            {
                startToLoad = true;
            }
            else if (letter.ToString()=="#")
            {
                textTyperToTrigger.GetComponent<TextTypingController>().hasTriggered = true;
            }
            else
            {
                cursorBlinking = false;
                textTypingCursor.enabled = false;
                textField.text += letter;
                if (textType == TextType.glitchyText)
                {
                    blueTextField.text += letter;
                    //redTextField.text += letter;
                }else if (textType == TextType.redText)
                {
                    blueTextField.text += letter;
                }
                yield return new WaitForSeconds(letterPause);
            }
            
        }
    }

    void loadScene()
    {
        loadTimer -= Time.deltaTime;
        if (loadTimer<2)
        {
            if (noiseObject.activeSelf == false)
            {
                staticNoise.Play();
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
        }
        

        if (loadTimer<0)
        {
            completionKeeper.GetComponent<AudioSource>().volume = 0.06f;
            SceneManager.LoadScene(sceneToLoad);
        }

    }
}
