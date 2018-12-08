using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretTurner : MonoBehaviour // this script makes the turrets turn and manages them being down
{

    public static List<GameObject> playerList = new List<GameObject>(); // static list of the player and all player doubles
    public bool turretDown = false; //is this turret down
    public float turretDownTimer = 3f; //temp variable for how long the turret is down for
    public float turretDownTimerMax = 3f;// how long the turret is down for 
    public float convertedDownTimerPercent; // down timer as percentage for using with ui circle

    public GameObject deathTimerSprite; //prefab of the circle sprite that shows when turrets are down
    private GameObject tempDeathTimerSprite; //temp variable for spawned circle ui sprite
    public GameObject canvas;//ui canvas

    public GameObject targetPlayer;// the current closest player
    private AudioSource smokeSound; // the sound for the smoke 
    public AudioClip boopSound;// boop sound to show turret back online
    public AudioClip smokeSoundClip;

    private CompletionKeeper completionKeeper;

    public bool onDoubleLevel = false;

	// Use this for initialization
	void Start ()
	{
        //clear the player list and add the original player in the level to it 
	    playerList.Clear();
        playerList.Add(GameObject.FindGameObjectWithTag("Player"));
        //assign canvas 
        canvas = GameObject.FindGameObjectWithTag("UICanvas");
        //spawn death circle ui sprite, parent it correctly and position it above turret
	    tempDeathTimerSprite = Instantiate(deathTimerSprite, transform.position, Quaternion.identity);
        tempDeathTimerSprite.transform.SetParent(canvas.transform);
        tempDeathTimerSprite.GetComponent<RectTransform>().SetAsFirstSibling();
	    Vector2 SliderPos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint((this.transform.position));
        tempDeathTimerSprite.transform.position = SliderPos;
        //assign sound source
	    smokeSound = gameObject.GetComponent<AudioSource>();
	    completionKeeper = GameObject.FindGameObjectWithTag("CompletionKeeper").GetComponent<CompletionKeeper>();
	}
	
	// Update is called once per frame
	void Update () {
        //fin the closest player
        //Debug.Log(playerList);
	    if (onDoubleLevel==false)
	    {
	        targetPlayer = findClosestPlayer();
        }
	    //targetPlayer = findClosestPlayer();


        if (turretDown==false&&targetPlayer!=null)//look at the closest player
	    {
	        transform.LookAt(new Vector3(targetPlayer.transform.position.x, transform.position.y, targetPlayer.transform.position.z));
        }

	    if (turretDown==true)//if the turret is down, make respawn start and show circle ui going up
	    {
	        gameObject.transform.GetChild(0).GetComponentInChildren<TurretShooter>().isTargettingLaserOn = false;
	        turretDownTimer -= Time.deltaTime;
	        convertedDownTimerPercent = 1-(turretDownTimer / turretDownTimerMax);
            //Debug.Log(convertedDownTimerPercent);
	        tempDeathTimerSprite.GetComponent<Image>().fillAmount = convertedDownTimerPercent;
	        tempDeathTimerSprite.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount =
	            convertedDownTimerPercent;

	    }
	    if (turretDownTimer<0)// if the turret respawn timer ends bring the turret back up
	    {
	        if (tag!= "WallTurret")
	        {
	            gameObject.GetComponent<ParticleSystem>().Stop();
            }

	        if (completionKeeper.toggleLasers==true)
	        {
	            gameObject.transform.GetChild(0).GetComponentInChildren<TurretShooter>().isTargettingLaserOn = true;
            }
            smokeSound.Stop();
	        smokeSound.loop = false;
	        smokeSound.pitch = 0.75f;
	        smokeSound.volume = 0.035f;
	        smokeSound.clip = boopSound;
            smokeSound.Play();
	        tempDeathTimerSprite.GetComponent<Image>().fillAmount = 0;
	        tempDeathTimerSprite.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 0;
	        turretDown = false;
	        turretDownTimer = turretDownTimerMax;
	    }

	    

	}

    private GameObject findClosestPlayer()// find the closest player 
    {
        GameObject closestPlayer = null;
        float distanceBetweenClosestPlayerAndTurret = 0;
        foreach (GameObject player in playerList)
        {
            if (closestPlayer == null)
            {
                closestPlayer = player;
                distanceBetweenClosestPlayerAndTurret = Vector3.Distance(transform.position, player.transform.position);
            }else if (Vector3.Distance(transform.position, player.transform.position)< distanceBetweenClosestPlayerAndTurret)
            {
                closestPlayer = player;
                distanceBetweenClosestPlayerAndTurret = Vector3.Distance(transform.position, player.transform.position);
            }
        }

        return closestPlayer;
    }

    public void ChangeToSmokeSound()
    {
        smokeSound.loop = true;
        smokeSound.volume = 0.1f;
        smokeSound.pitch = 1f;
        smokeSound.clip = smokeSoundClip;
    }

    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
}
