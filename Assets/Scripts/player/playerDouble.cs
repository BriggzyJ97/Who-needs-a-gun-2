using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDouble : MonoBehaviour { //this script controls the player doubles

    private bool isDoubleOn = false;
    //albedo changes 
    private float albedoIncreaseRate = 2.5f;
    private float emissionIncreaseRate = 2.5f;
    //movement variables
    private Rigidbody myRB;
    private Vector3 moveVelocity;
    

    private GameStateManager gameStateManager;

    public GameObject playerDeathParticles;
    public GameObject originalPlayer;

    private AudioSource soundPlayer;
    public AudioClip deathExplosionAudioClip;

    public bool isPlayerSprinting = false;

    // Use this for initialization
    void Start () {//assign variables
        myRB = gameObject.GetComponent<Rigidbody>();
        gameStateManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStateManager>();
        soundPlayer = gameObject.GetComponent<AudioSource>();
        originalPlayer = GameObject.FindGameObjectWithTag("Player");
        gameObject.tag = "Player";
        
        TurretTurner.playerList.Add(gameObject);
        isDoubleOn = true;
    }
	
	// Update is called once per frame
	void Update () {

	    if (TurretTurner.playerList.Contains(gameObject)!=true)
	    {
	        TurretTurner.playerList.Add(gameObject);
        }
	    if (albedoIncreaseRate<250&&isDoubleOn==true)//make the player double slowly light up when activated
	    {
	        albedoIncreaseRate += 5f;
	        emissionIncreaseRate += 5f;
            gameObject.GetComponent<Renderer>().material.color = new Color(albedoIncreaseRate/255,albedoIncreaseRate/255,albedoIncreaseRate/255);
            gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(emissionIncreaseRate/255,emissionIncreaseRate/255,emissionIncreaseRate/255));
	    }else if (albedoIncreaseRate>0&&isDoubleOn==false)//make the player double slowly drop in light when activated
        {
	        albedoIncreaseRate -= 5f;
	        emissionIncreaseRate -= 5f;
	        gameObject.GetComponent<Renderer>().material.color = new Color(albedoIncreaseRate / 255, albedoIncreaseRate / 255, albedoIncreaseRate / 255);
	        gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(emissionIncreaseRate / 255, emissionIncreaseRate / 255, emissionIncreaseRate / 255));
        }

	    if (isDoubleOn==true)
	    {
            transform.position = new Vector3(transform.position.x, 0.9f, transform.position.z);
            //PLAYER MOVEMENT
            
	        moveVelocity = originalPlayer.GetComponent<playerController>().moveVelocity;
	    }
	}

    void FixedUpdate()
    {
        //Set the Rigidbody to retreieve the moveVelocity;
        //Debug.Log("Velocity before applying: "+moveVelocity);
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Mouse1))
        {
            myRB.velocity = (moveVelocity * Time.deltaTime) * 2.3f;
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

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag=="Player")//if the double touches a player, turn on the double
        {
            soundPlayer.Play();
            gameObject.tag = "Player";
            originalPlayer = other.gameObject;
            TurretTurner.playerList.Add(gameObject);
            isDoubleOn = true;
        }
    }

    public void Die()
    {
        //player double dying
        soundPlayer.clip = deathExplosionAudioClip;
        soundPlayer.Play();
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        TurretTurner.playerList.Remove(gameObject);
        Instantiate(playerDeathParticles, transform.position, Quaternion.identity);
    }

    public void Disable()
    {
        //disable the player double
        isDoubleOn = false;
    }
}
