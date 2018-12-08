using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TurretShooter : MonoBehaviour //Controls the turret shooting
{
    public TurretManager.turretStates thisTurretsState = TurretManager.turretStates.idle; 

    private float turretDuration; //this  stops the turret shooting forever when activated;
    public float turretDurationMax;

    private float shootingCooldown; //this shooting cooldown makes sure the bullets aren't shot too quickly
    public float shootingCooldownMax;

    public GameObject bullet; //the bullet prefab
    public GameObject sparks; // the flash of the turret firing
    public GameObject sparksEmitter; //where the turret flash comes from
    private AudioSource shootingSound;

    public GameObject baseBox;// the big square box part of the turret
    private Material baseMat;// the material of the base of turret

    public bool isAutoShooter = false; //if this turret is a purple auto shooter one
    private Color purpleBaseColor = new Color(0.7f,0,0.7f,1); // the purple base colour for dynamic tweaking
    private Color redBaseColor = new Color(0.7f,0,0,1);

    public bool isTargettingLaserOn = false;
    public GameObject lineRenderer;
    private LineRenderer tempLineRenderer;
    private bool hasLineRendererBeenSpawned = false;
    public LayerMask targetLineLayerMask;
    
    // Use this for initialization
    void Start ()
    {
        //sets cooldowns to the base number
        turretDuration = turretDurationMax;
        if (isAutoShooter==true)
        {
            shootingCooldown = 0;
        }
        else
        {
            shootingCooldown = shootingCooldownMax;
        }
        
        shootingSound = gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
	   //Debug.Log(shootingCooldown);
	    if (isAutoShooter==true)//if this turret is an auto shooter and its not down then have it shoot every now and again, spawning bullet and gunflash
	    {
	        
	            turretDuration -= Time.deltaTime;
	            shootingCooldown += Time.deltaTime;
	            if (shootingCooldown >= shootingCooldownMax)
	            {
	                if (transform.parent.parent.GetComponent<TurretTurner>().turretDown == false)
	                {
	                    shootingSound.Play();
	                    Instantiate(bullet, transform.position, transform.rotation);
	                    Instantiate(sparks, sparksEmitter.transform.position, sparksEmitter.transform.rotation);
                    }
                    
                    shootingCooldown = 0f;
	            }
	        

	        Color finalColor = purpleBaseColor * Mathf.LinearToGammaSpace(shootingCooldown/shootingCooldownMax);//change the colour of the emission marks on the turret base to show it charging up 
            baseBox.GetComponent<Renderer>().material.SetColor("_EmissionColor", finalColor);
	        if (turretDuration <= 0)//stop the turret from shooting forever
	        {
	            thisTurretsState = TurretManager.turretStates.idle;
	            turretDuration = turretDurationMax;
	        }
        }
        else if (isAutoShooter==false)//if this turret is not an auto shooter then shoot when its not down and the state is shooting, which is changed by the turret manager
	    {
	        shootingCooldown += Time.deltaTime;
            if (thisTurretsState == TurretManager.turretStates.shooting && transform.parent.parent.GetComponent<TurretTurner>().turretDown == false)
	        {
	            turretDuration -= Time.deltaTime;
	            
	            if (shootingCooldown >= shootingCooldownMax)
	            {
	                shootingSound.Play();
                    Instantiate(bullet, transform.position, transform.rotation);
	                Instantiate(sparks, sparksEmitter.transform.position, sparksEmitter.transform.rotation);
                    shootingCooldown = 0;
	            }
	        }
	        Color finalColor = redBaseColor * Mathf.Min(Mathf.LinearToGammaSpace(shootingCooldown / shootingCooldownMax),1);//change the colour of the emission marks on the turret base to show it charging up 
	        baseBox.GetComponent<Renderer>().material.SetColor("_EmissionColor", finalColor);
            if (turretDuration <= 0)
	        {
	            thisTurretsState = TurretManager.turretStates.idle;
	            turretDuration = turretDurationMax;
	        }

        }

        if(isTargettingLaserOn==true)
        {
            if (hasLineRendererBeenSpawned==false)
            {
                tempLineRenderer=Instantiate(lineRenderer, transform.position, Quaternion.identity).GetComponent<LineRenderer>();
                hasLineRendererBeenSpawned = true;
            }
            RaycastHit hit;
	        if (Physics.Raycast(sparksEmitter.transform.position, sparksEmitter.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, targetLineLayerMask))
	        {
	            Debug.DrawRay(sparksEmitter.transform.position, sparksEmitter.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
	            tempLineRenderer.gameObject.transform.position =
	                sparksEmitter.transform.position - ((sparksEmitter.transform.position - hit.point) / 2);
                tempLineRenderer.SetPosition(0,new Vector3(sparksEmitter.transform.position.x, sparksEmitter.transform.position.y, sparksEmitter.transform.position.z));
                tempLineRenderer.SetPosition(1,new Vector3(hit.point.x, hit.point.y, hit.point.z));
                //Debug.Log("Did Hit");
            }
	        else
	        {
	            Debug.DrawRay(sparksEmitter.transform.position, sparksEmitter.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
	            //Debug.Log("Did not Hit");
	        }
        }
        else
        {
            if (tempLineRenderer!=null)
            {
                Destroy(tempLineRenderer);
                hasLineRendererBeenSpawned = false;
            }
        }
    }
}
