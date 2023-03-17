using System.Collections.Generic;
using FPSepController;
using UnityEngine.Events;
using UnityEngine;

public class PlayerGroundCheck : PlayerComponent
{        
    //The class determines the outcome of this bool, which is used to alter movement based on grounded/aerial state.
    [HideInInspector] public bool isGrounded = false;

    [Header("Detection Dimensions")]
    public Transform floorCheckBox = null;

    [Header("Limit Detection")]
    [SerializeField, Tooltip("Layermask limits what objects will be detected (by layer)")]  //Don't forget to exclude the player's own collider.
    LayerMask groundDetectMask = 1;
    [SerializeField, Tooltip("Further limit what objects will be detected by using tags.")]
    List<string> floorTags = new List<string>(1) { "Untagged" };

    [Tooltip("The max angle that allows the player to walk on a slope before being forced to aerial state.")]
    public float maxSlopeAngle = 35;
    [HideInInspector] public bool tooSteep = false;
    [HideInInspector] public float currentSlopeAngle = 0;    

    //Current slope orientation.
    [HideInInspector] public Vector3 currentSlopeNormal = Vector3.up;
    [HideInInspector] public Vector3 slopeRight = Vector3.right;
    [HideInInspector] public Vector3 currentSlopeDir = Vector3.zero;


    [Header("Events")]
    public UnityEvent onLanding = null;
    public UnityEvent onGroundLeave = null;


    [HideInInspector] public Vector3[] floorcheck_bounds = new Vector3[4];   //starting from close-left, goes clockwise order (farleft next)
    void CheckGround()
    {
        //Cast a ray downwards from the character        
        RaycastHit hit = new RaycastHit();
        Vector3 halfExtents = floorCheckBox.localScale/2;        
        Vector3 startpos = floorCheckBox.position + (transform.up * halfExtents.y);        

        //Check for ground collision in a rectangle-shape
        RaycastHit[] hits = new RaycastHit[5];
        Vector3 dir = -transform.up;
        float maxDistance = floorCheckBox.localScale.y;
        floorcheck_bounds[0] = (startpos - (transform.right * halfExtents.x)) - (transform.forward * halfExtents.z);
        floorcheck_bounds[1] = (startpos - (transform.right * halfExtents.x)) + (transform.forward * halfExtents.z);
        floorcheck_bounds[2] = (startpos + (transform.right * halfExtents.x)) + (transform.forward * halfExtents.z);
        floorcheck_bounds[3] = (startpos + (transform.right * halfExtents.x)) - (transform.forward * halfExtents.z);

        //Also check right below the player
        Physics.Raycast(startpos, dir, out hits[0], maxDistance, groundDetectMask, QueryTriggerInteraction.Ignore);
        for(int i = 0; i < floorcheck_bounds.Length; i++)
        {
            Physics.Raycast(floorcheck_bounds[i], dir, out hits[i+1], maxDistance, groundDetectMask, QueryTriggerInteraction.Ignore);
        }

        //Get a result from the array.
        foreach(RaycastHit h in hits)
        {
            if (h.transform != null)
            {
                hit = h;
                break;
            }
        }

        //On hit: 
        if (hit.collider != null && floorTags.Contains(hit.collider.tag))
        {
            currentSlopeNormal = hit.normal;

            //Get slope info
            currentSlopeAngle = Vector3.Angle(transform.up, hit.normal); //Calc angle between normal and character
            slopeRight = Vector3.Cross(hit.normal, Vector3.up).normalized;
            currentSlopeDir = Vector3.Cross(hit.normal, slopeRight).normalized; //get the direction from the slope downwards            
            
            //SLOPE IS TOO STEEP
            if (currentSlopeAngle > maxSlopeAngle)
            {
                tooSteep = true;
                if (isGrounded)
                    onGroundLeave?.Invoke();
                
                isGrounded = false;
                return;
            }

            //SLOPE IS FINE
            else if (!isGrounded)            
                onLanding?.Invoke();    //perhaps check if player had enough airtime to warrant a 'landing'          

            tooSteep = false;
            isGrounded = true;
        }
        //No hit:
        else
        {            
            //Was grounded before --> player leaves the ground now.
            if (isGrounded)                            
                onGroundLeave?.Invoke();            

            isGrounded = false;
            //tooSteep = false;       //reset to base value
            currentSlopeNormal = transform.up;
        }
    }





    public void FixNormal(Vector3 position, ref RaycastHit hit, int layermask)
    {        
        RaycastHit rayHit;
        Physics.Raycast(position, hit.point - position, out rayHit, 2 * hit.distance, layermask);
        hit.normal = rayHit.normal;
    }

    public override void OnPlayerFixedUpdate()
    {
        CheckGround();
    }
    public override void OnPlayerStart(Player pPlayer)
    {
        if (floorCheckBox == null)
        {
            Debug.LogError("Error, the floorcheckbox has not been assigned so the player can't detect the floor", this);
            this.enabled = false;
            return;
        }
    }
}