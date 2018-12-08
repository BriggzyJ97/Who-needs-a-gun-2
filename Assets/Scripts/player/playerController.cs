using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class playerController : MonoBehaviour //script controls the player
{
    //player movement speed variables
    public float playerSpeedHorizontal;
    public float playerSpeedVertical;
    public float playerSpeedMin;
    public float playerSpeedMax;
    public float movingAcceleration = 1.1f;
    public float movingDecceleration = 0.9f;

    private Rigidbody myRB;

    public Vector3 moveVelocity;

    private Vector3 mousePosition;

    private float sprintMultiplier = 2.3f;

    public enum LeftRightState//movement states
    {
        left,
        right,
        idle
    }

    private LeftRightState currentLeftRightState;

    public enum UpDownState
    {
        up,
        down,
        idle
    }

    private UpDownState currentUpDownState;

    private LeftRightState lastLeftRightState;
    private UpDownState lastUpDownState;

    private GameStateManager gameStateManager;//game state manager

    public GameObject playerDeathParticles;//prefab for players death particles

    private AudioSource playerAudioSource;
    private bool audioLock = false;
    public AudioClip playerDeathExplosion;

    public bool isPlayerSprinting = false;

    // Use this for initialization
    void Start ()//assign variables
    {
        myRB = gameObject.GetComponent<Rigidbody>();
        gameStateManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStateManager>();
        playerAudioSource = gameObject.GetComponent<AudioSource>();
        //TurretTurner.playerList.Add(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, 0.9f, transform.position.z);//always make sure the players height sounds the same 
        //PLAYER MOVEMENT
	    if (gameStateManager.isGamePaused==false&& gameStateManager.currentGameState!=GameStateManager.GameState.levelDialogue)//can only move if the game isn't paused 
	    {
	        if (Input.GetKey(KeyCode.A) == true && Input.GetKey(KeyCode.D) != true)
	        {
	            currentLeftRightState = LeftRightState.left;
	            lastLeftRightState = LeftRightState.left;
	        }
	        else if (Input.GetKey(KeyCode.A) != true && Input.GetKey(KeyCode.D) == true)
	        {
	            currentLeftRightState = LeftRightState.right;
	            lastLeftRightState = LeftRightState.right;
	        }
	        else if (Input.GetKey(KeyCode.A) == true && Input.GetKey(KeyCode.D) == true)
	        {
	            currentLeftRightState = LeftRightState.idle;
	        }
	        else
	        {
	            currentLeftRightState = LeftRightState.idle;
	            if (playerSpeedHorizontal > playerSpeedMin)
	            {
	                playerSpeedHorizontal = playerSpeedHorizontal * movingDecceleration;
	            }
	        }

	        if (Input.GetKey(KeyCode.W) == true && Input.GetKey(KeyCode.S) != true)
	        {
	            currentUpDownState = UpDownState.up;
	            lastUpDownState = UpDownState.up;
	        }
	        else if (Input.GetKey(KeyCode.W) != true && Input.GetKey(KeyCode.S) == true)
	        {
	            currentUpDownState = UpDownState.down;
	            lastUpDownState = UpDownState.down;
	        }
	        else if (Input.GetKey(KeyCode.W) == true && Input.GetKey(KeyCode.S) == true)
	        {
	            currentUpDownState = UpDownState.idle;
	        }
	        else
	        {
	            currentUpDownState = UpDownState.idle;
	            if (playerSpeedVertical > playerSpeedMin)
	            {
	                playerSpeedVertical = playerSpeedVertical * movingDecceleration;
	            }
	        }
        }
	    else
	    {
	        currentLeftRightState = LeftRightState.idle;
	        currentUpDownState = UpDownState.idle;
	        if (playerSpeedVertical > playerSpeedMin)
	        {
	            playerSpeedVertical = playerSpeedVertical * movingDecceleration;
	        }
	        if (playerSpeedHorizontal > playerSpeedMin)
	        {
	            playerSpeedHorizontal = playerSpeedHorizontal * movingDecceleration;
	        }
        }
	    
        //making the player move 
	    if (currentLeftRightState==LeftRightState.left)
	    {
	        if (audioLock == false)
	        {
	            playerAudioSource.loop = true;
                playerAudioSource.Play();
	            audioLock = true;
	        }
	        if (playerSpeedHorizontal<playerSpeedMax)
	        {
	            playerSpeedHorizontal = playerSpeedHorizontal * (movingAcceleration);
	        }
	        moveVelocity = new Vector3(-playerSpeedHorizontal, moveVelocity.y, moveVelocity.z);
	    }else if (currentLeftRightState == LeftRightState.right)
	    {
	        if (audioLock == false)
	        {
	            playerAudioSource.loop = true;
                playerAudioSource.Play();
	            audioLock = true;
	        }
            if (playerSpeedHorizontal < playerSpeedMax)
	        {
                playerSpeedHorizontal = playerSpeedHorizontal * (movingAcceleration);
	        }
            moveVelocity = new Vector3(playerSpeedHorizontal, moveVelocity.y, moveVelocity.z);
        }
        else if (currentLeftRightState == LeftRightState.idle)
	    {
	        if (playerSpeedHorizontal>playerSpeedMin)
	        {
	            if (lastLeftRightState==LeftRightState.left)
	            {
	                moveVelocity = new Vector3(-playerSpeedHorizontal, moveVelocity.y, moveVelocity.z);
                }
	            else if (lastLeftRightState ==LeftRightState.right)
	            {
	                moveVelocity = new Vector3(playerSpeedHorizontal, moveVelocity.y, moveVelocity.z);
                }
	            
            }
	        else
	        {
	            moveVelocity = new Vector3(0, moveVelocity.y, moveVelocity.z);
	        }
	    }
	    if (currentUpDownState == UpDownState.up)
	    {
	        if (audioLock == false)
	        {
	            playerAudioSource.loop = true;
                playerAudioSource.Play();
	            audioLock = true;
	        }
            if (playerSpeedVertical < playerSpeedMax)
	        {
                playerSpeedVertical = playerSpeedVertical * (movingAcceleration);
	        }
            moveVelocity = new Vector3(moveVelocity.x, moveVelocity.y, playerSpeedVertical);
	    }
	    else if (currentUpDownState == UpDownState.down)
	    {
	        if (audioLock == false)
	        {
	            playerAudioSource.loop = true;
                playerAudioSource.Play();
	            audioLock = true;
	        }
            if (playerSpeedVertical < playerSpeedMax)
	        {
                playerSpeedVertical = playerSpeedVertical * (movingAcceleration);
	        }
            moveVelocity = new Vector3(moveVelocity.x, moveVelocity.y, -playerSpeedVertical);
	    }
	    else if (currentUpDownState == UpDownState.idle)
	    {

	        if (playerSpeedVertical > playerSpeedMin)
	        {
	            if (lastUpDownState==UpDownState.up)
	            {
	                moveVelocity = new Vector3(moveVelocity.x, moveVelocity.y, playerSpeedVertical);
                }else if (lastUpDownState ==UpDownState.down)
	            {
	                moveVelocity = new Vector3(moveVelocity.x, moveVelocity.y, -playerSpeedVertical);
                }
	        }
	        else
	        {
	            moveVelocity = new Vector3(moveVelocity.x, moveVelocity.y, 0);
	        }
        }
        

    }
    void FixedUpdate()
    {
        //apply move velocity to the rigidbody
        if (Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.Mouse1))
        {
            myRB.velocity = (moveVelocity * Time.deltaTime)*sprintMultiplier;
            isPlayerSprinting = true;
        }
        else
        {
            myRB.velocity = moveVelocity * Time.deltaTime;
            isPlayerSprinting = false;
        }
        

        if (moveVelocity.x <= 0.01f && moveVelocity.x >= -0.01f)
        {
            moveVelocity.x = 0f;
        }
        if (moveVelocity.y <= 0.01f && moveVelocity.y >= -0.01f)
        {
            moveVelocity.y = 0f;
        }
        if (moveVelocity.z <= 0.01f && moveVelocity.z >= -0.01f)
        {
            moveVelocity.z = 0f;
        }

        if (moveVelocity.x == 0f &&moveVelocity.y == 0f && moveVelocity.z == 0f)
        {
            playerAudioSource.loop = false;
            audioLock = false;
        }

    }

    public void Die()
    {
        //when the player dies, make his model invisible and spawn the feath particles
        playerAudioSource.clip = playerDeathExplosion;
        playerAudioSource.loop = false;
        playerAudioSource.Play();
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        Instantiate(playerDeathParticles, transform.position, Quaternion.identity);
    }
}
