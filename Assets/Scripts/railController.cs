using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class railController : MonoBehaviour
{

    public float railMoveSpeed;
    private float widthAdjustmentMultiplier;

    public Transform endStop1;
    public Transform endStop2;

    public Transform rail1;

    public GameObject movingObject;

    public string startPoint;

    public Transform endPointLeft;
    public Transform endPointRight;

    public bool isLeftBlocked = false;
    public bool isRightBlocked = false;

    public bool isPlayerSprinting = false;

    public enum railMovement
    {
        idle,
        movingLeft,
        movingRight
    }

    public railMovement CurrentDirectionOfRailMovement = railMovement.idle;

	// Use this for initialization
	void Start () {
		endStop1.LookAt(endStop2);
        endStop2.LookAt(endStop1);
	    Vector3 VectorThreeBetweenEndPoints = endStop1.position - endStop2.position;
	    rail1.position = endStop1.position - (VectorThreeBetweenEndPoints / 2);
        rail1.LookAt(endStop1.position);
        rail1.Rotate(0,90,0);

	    float DistanceBetweenEndPoints = Vector3.Distance(endStop1.position, endStop2.position);
        rail1.localScale = new Vector3((DistanceBetweenEndPoints-0.167f)/9.16f,1,1);
	    
	    float WidthOfMovingObject = movingObject.GetComponent<Renderer>().bounds.size.x;
        Debug.Log(WidthOfMovingObject);
	    widthAdjustmentMultiplier = 0.642f - (0.0507f * Mathf.Log(WidthOfMovingObject));
        Debug.Log(widthAdjustmentMultiplier);
	    endPointLeft.position = endStop1.transform.position - ((VectorThreeBetweenEndPoints.normalized) * (WidthOfMovingObject *widthAdjustmentMultiplier));
	    endPointRight.position = endStop2.transform.position + ((VectorThreeBetweenEndPoints.normalized) * (WidthOfMovingObject *widthAdjustmentMultiplier));

	    if (startPoint == "left")
	    {
	        movingObject.transform.position = endPointLeft.position;
	        movingObject.transform.localPosition = new Vector3(movingObject.transform.localPosition.x, 1.48f, movingObject.transform.localPosition.z);
	    }
	    else if (startPoint == "right")
	    {
	        movingObject.transform.position = endPointRight.position;
	        movingObject.transform.localPosition = new Vector3(movingObject.transform.localPosition.x, 1.48f, movingObject.transform.localPosition.z);
	    }
	    else
	    {
	        movingObject.transform.position = endStop1.position - (VectorThreeBetweenEndPoints / 2);
	        movingObject.transform.localPosition = new Vector3(movingObject.transform.localPosition.x, 1.48f, movingObject.transform.localPosition.z);
	    }

	    movingObject.transform.LookAt(new Vector3(endStop1.transform.position.x, movingObject.transform.localPosition.y, endStop1.transform.position.z));
	    movingObject.transform.rotation = Quaternion.Euler(0, movingObject.transform.rotation.eulerAngles.y, 0);
    }
	
	// Update is called once per frame
	void Update () {
	    if (CurrentDirectionOfRailMovement == railMovement.movingLeft&&isRightBlocked==false)
	    {
	        if (isPlayerSprinting==true)
	        {
	            movingObject.transform.position = Vector3.MoveTowards(movingObject.transform.position, new Vector3(endPointLeft.position.x,movingObject.transform.position.y, endPointLeft.position.z), 
	                railMoveSpeed *2f* Time.deltaTime);
            }
	        else
	        {
	            movingObject.transform.position = Vector3.MoveTowards(movingObject.transform.position, new Vector3(endPointLeft.position.x, movingObject.transform.position.y, endPointLeft.position.z),
	                railMoveSpeed * Time.deltaTime);
            }
	        
	    }else if (CurrentDirectionOfRailMovement==railMovement.movingRight&&isLeftBlocked==false)
	    {
	        if (isPlayerSprinting == true)
	        {
	            movingObject.transform.position = Vector3.MoveTowards(movingObject.transform.position, new Vector3(endPointRight.position.x, movingObject.transform.position.y, endPointRight.position.z),
	                railMoveSpeed *2f* Time.deltaTime);
            }
	        else
	        {
	            movingObject.transform.position = Vector3.MoveTowards(movingObject.transform.position, new Vector3(endPointRight.position.x, movingObject.transform.position.y, endPointRight.position.z),
	                railMoveSpeed * Time.deltaTime);
            }

	        
        }
	}
}
