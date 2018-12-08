using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class bulletController : MonoBehaviour//THIS SCRIPT MOVES THE BULLET AND MANAGES COLLISIONS
{

    public float bulletSpeed; //Speed of bullet
    public GameObject sparks; //Sparks Prefab that spawns when bullet hits wall
    public GameObject puff;
    private Vector3 lastPosition; //saves the first position of bullet to calculate direction of the bullet
    public Vector3 directionOfBullet; //The direction that the bullet is moving

    private bool positionUpdateBool = false; //Bool used for updating "lastPosition" variable
    private bool isReflected = false;// Bool that keeps track of whether the bullet has been reflected
    public bool readyForReflected = true; //Makes sure that bullet doesnt keep reflecting infinitely when hitting mirror

    private AudioSource bulletHitSound;

    private float CollisionDelay = 0.2f;
    

	// Use this for initialization
	void Start ()
	{
	    lastPosition = transform.position; //Save first position to calculate direction
	    bulletHitSound = gameObject.GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (CollisionDelay>0)
	    {
	        CollisionDelay -= Time.deltaTime;
	        if (CollisionDelay<0.1f)
	        {
	            gameObject.layer = 0;
	        }
        }
	    
	    if (isReflected==false)
	    {
            //moves this bullet
	        transform.Translate(bulletSpeed * Time.deltaTime, 0, 0);
        }
        else
	    {
            //moves bullet along new reflected direction
            transform.Translate(directionOfBullet*bulletSpeed*Time.deltaTime, Space.World);
	    }
	    if (positionUpdateBool == false)
	    {
            //Calculate direction of bullet
	        directionOfBullet = UpdateDirection();
	        positionUpdateBool = true;
	    }

	    if (readyForReflected==false)
	    {
            //Makes sure th bullet doesnt infinitely reflect
	        new WaitForSeconds(0.5f);
	        readyForReflected = true;
	    }

	    if (directionOfBullet == new Vector3(0,0,0))
	    {
            //a failsafe that makes sure that if the bullet stops moving for whatever reason it deletes itself
            Destroy(this.gameObject);
	    }



    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")//When bullet hits wall, spawn sparks and delete itself
        {
            bulletHitSound.Play();
            Instantiate(sparks, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }else if (other.gameObject.tag == "WallTurret")//Or if bullet hits wall turret, spawn sparks and delete itself
        {
            bulletHitSound.Play();
            //Debug.Log("wallturret hit");
            Instantiate(sparks, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }else if (other.gameObject.tag == "Turret")//Or if bullet hits turret, shut it down, make it smoke, spawn sparks and delete bullet
        {
            bulletHitSound.Play();
            other.GetComponentInChildren<TurretTurner>().ChangeToSmokeSound();
            other.GetComponentInChildren<AudioSource>().Play();
            other.GetComponentInChildren<TurretTurner>().turretDown = true;
            other.GetComponentInChildren<TurretTurner>().turretDownTimer =
                other.GetComponentInChildren<TurretTurner>().turretDownTimerMax;
            other.GetComponentInChildren<ParticleSystem>().Play();
            Instantiate(sparks, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }else if (other.gameObject.tag == "VerticleMirror")// if bullet hits verticle mirror, Reflect bullet direction along the x-axis, rotate the bullet and make it now move by direction
        {
            if (readyForReflected==true)
            {
                bulletHitSound.Play();
                other.gameObject.GetComponent<AudioSource>().Play();
                isReflected = true;
                directionOfBullet = (Vector3.Reflect(directionOfBullet, Vector3.right)).normalized;
                transform.LookAt(transform.position + directionOfBullet);
                transform.Rotate(0,90,0);
                //Debug.Log(directionOfBullet);
                readyForReflected = false;
            }
        }
        else if (other.gameObject.tag == "DiagonalRightMirror"
        ) // if bullet hits verticle mirror, Reflect bullet direction along the x-axis, rotate the bullet and make it now move by direction
        {
            if (readyForReflected == true)
            {
                bulletHitSound.Play();
                other.gameObject.GetComponent<AudioSource>().Play();
                isReflected = true;
                float tempDirectionXValue = directionOfBullet.z;
                float tempDirectionZValue = directionOfBullet.x;
                
                directionOfBullet = new Vector3(tempDirectionXValue,0,tempDirectionZValue);
                transform.LookAt(transform.position + directionOfBullet);
                transform.Rotate(0, 90, 0);
                //Debug.Log(directionOfBullet);
                readyForReflected = false;
            }
        }
        else if (other.gameObject.tag == "DiagonalLeftMirror"
        ) // if bullet hits verticle mirror, Reflect bullet direction along the x-axis, rotate the bullet and make it now move by direction
        {
            if (readyForReflected == true)
            {
                bulletHitSound.Play();
                other.gameObject.GetComponent<AudioSource>().Play();
                isReflected = true;
                float tempDirectionXValue = -directionOfBullet.z;
                float tempDirectionZValue = -directionOfBullet.x;

                directionOfBullet = new Vector3(tempDirectionXValue, 0, tempDirectionZValue);
                transform.LookAt(transform.position + directionOfBullet);
                transform.Rotate(0, 90, 0);
                //Debug.Log(directionOfBullet);
                readyForReflected = false;
            }
        }
        else if (other.gameObject.tag == "HorizontalMirror")// if bullet hits verticle mirror, Reflect bullet direction along the y-axis, rotate the bullet and make it now move by direction
        {
            if (readyForReflected == true)
            {
                bulletHitSound.Play();
                other.gameObject.GetComponent<AudioSource>().Play();
                isReflected = true;
                directionOfBullet = (Vector3.Reflect(directionOfBullet, Vector3.forward)).normalized;
                transform.LookAt(transform.position + directionOfBullet);
                transform.Rotate(0, 90, 0);
                //Debug.Log(directionOfBullet);
                readyForReflected = false;
            }
        }else if (other.gameObject.tag =="Player")//if bullet hits player, spawn sparks, make the player die, make the game end and destroy the bullet
        {
            Instantiate(sparks, transform.position, Quaternion.identity);
            if (other.gameObject.GetComponent<playerController>()!=null)
            {
                other.gameObject.GetComponent<playerController>().Die();
            }
            if (other.gameObject.GetComponent<playerDouble>() != null)
            {
                other.gameObject.GetComponent<playerDouble>().Die();
            }
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStateManager>().currentGameState =
                GameStateManager.GameState.levelLose;
            Destroy(this.gameObject);
            
        }
        else if (other.gameObject.tag == "Player")//if bullet hits player, spawn sparks, make the player die, make the game end and destroy the bullet
        {
            Instantiate(sparks, transform.position, Quaternion.identity);
            if (other.gameObject.GetComponent<playerController>() != null)
            {
                other.gameObject.GetComponent<playerController>().Die();
            }
            if (other.gameObject.GetComponent<playerDouble>() != null)
            {
                other.gameObject.GetComponent<playerDouble>().Die();
            }
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStateManager>().currentGameState =
                GameStateManager.GameState.levelLose;
            Destroy(this.gameObject);

        }
        else if (other.gameObject.tag == "Box")//if bullet hits player, spawn sparks, make the player die, make the game end and destroy the bullet
        {
            Instantiate(sparks, transform.position, Quaternion.identity);
            Instantiate(puff, other.gameObject.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(this.gameObject);

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag=="Mirror")// another thing to stop bullet infinitely reflecting
        {
            readyForReflected = true;
        }
    }

    private Vector3 UpdateDirection()//calculate direction of bullet
    {
        Vector3 directionMoving = (transform.position - lastPosition).normalized;
        lastPosition = transform.position;
        return directionMoving;
    }
}
