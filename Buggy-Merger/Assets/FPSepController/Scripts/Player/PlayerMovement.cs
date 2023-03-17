using FPSepController;
using UnityEngine.Events;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerGroundCheck))]
public class PlayerMovement : PlayerComponent
{
    PlayerInput pInput = null;
    Rigidbody rb = null;
    PlayerGroundCheck groundCheck = null;

    [Header("Input Axis Indexes")]
    [InspectorName("Player Input Horizontal Index")]
    public int inputAxis_Horizontal_index = 0;
    [InspectorName("Player Input Vertical Index")]
    public int inputAxis_Vertical_index = 1;

    [Header("Movement Variables")]
    [Tooltip("The speed at which the player will move when grounded")]
    public float acc_speed = 30f;
    private float acc_speed_original = 0;
    [Tooltip("The player's movespeed when aerial.")]
    public float acc_airspeed = 15f;
    private float acc_airspeed_original = 0;

    [Space(8)]
    [Tooltip("Speed at which the player will fall when airborne. This is combined with the rigidbody fall-speed and is not directly affected by mass.")]
    public float fallspeed = 15f;
    [Tooltip("Speed at which the player will slide down steep slopes.")]
    public float slopeFallSpeed = 5f;

    [Header("Move Events")]
    [SerializeField, Tooltip("Called when the player's movement input is no longer: [x=0,y=0].")]
    UnityEvent onStartMove = null;
    [SerializeField, Tooltip("Called when the player's movement input has been reset to [x=0,y=0].")]
    UnityEvent onStopMove = null;


    [HideInInspector] public Vector2 inputs = Vector2.zero;     //Cached input variable.
    [HideInInspector] public bool isMoving = false;             //cached check
    Transform slopeT = null;        //Used for converting movement to 'slope-space'

    PlayerMovementOverride currentOverride = null;



    void DoMove()
    {
        //Save velocity in a new vec2 to gain more control before applying the changes.        
        Vector3 velo = rb.velocity;
        UpdateSlopeTransform();


        //---Player-Space-Changes---
        velo = slopeT.InverseTransformVector(velo);     //Convert velocity from world-space to 'slope-space'.


        velo += Acceleration();                         //Apply acceleration based on Player Input
        velo += FallSpeed();                            //Apply additional fall-speed

        velo = slopeT.TransformVector(velo);            //Convert the velocity back to world-space
        //--------------------------      


        SlopeCheck(ref velo);                           //apply slope-based changes to Velocity [note: ref parameter]                
        rb.velocity = velo;                             //Apply final velocity to rigidbody
    }

    Vector3 Acceleration()
    {
        if (inputs != Vector2.zero) isMoving = true;
        else isMoving = false;

        //Get grounded or aerial accelleration.
        float acc = acc_speed;
        if (!groundCheck.isGrounded) acc = acc_airspeed;

        //Move based on input and acceleration
        Vector3 result = slopeT.forward * inputs.y * acc * Time.fixedDeltaTime;
        result += slopeT.right * inputs.x * acc * Time.fixedDeltaTime;

        //Return the acceleration in player-space
        return transform.TransformVector(result);
    }

    Vector3 FallSpeed()
    {
        //Don't increase fallspeed if groundcheck isn't done. If it is, then prevent applying fallspeed when grounded or rising (velocity upwards)
        if (groundCheck == null || (groundCheck != null && (groundCheck.isGrounded || rb.velocity.y > 0)))
            return Vector3.zero;

        //Lower velocity based on fallspeed. (note that this is negative).        
        float acc_Y = -(fallspeed * Time.fixedDeltaTime);
        return transform.up * acc_Y;
    }

    void SlopeCheck(ref Vector3 velo)
    {
        float angle = groundCheck.currentSlopeAngle;
        Vector3 dir = groundCheck.currentSlopeDir;

        //Fall down steep slopes
        if (groundCheck.tooSteep)
        {
            rb.useGravity = true;       //angle is too steep: force player to airborne state
            velo += dir * angle * slopeFallSpeed * Time.fixedDeltaTime;     //Force player down the slope a bit.
            return;
        }
    }

    //Called as soon as the player leaves the ground.
    void OnAerial()
    {
    }
    //Called when the player initially touches the ground.
    void OnLanding()
    {
        //Reset the player's Y velocity.
        if (rb.velocity.y < 0)
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }



    public override void OnPlayerFixedUpdate()
    {
        //Player won't use gravity when grounded to prevent unintentionally sliding down slopes.
        rb.useGravity = !groundCheck.isGrounded;
        DoMove();   //Applying movement is done in FixedUpdate
    }

    public override void OnPlayerUpdate()
    {
        Vector2 oldInputs = inputs;
        inputs = GetInput();

        if (oldInputs == Vector2.zero && inputs != Vector2.zero)
        {
            //Wasn't moving before but is now --> start moving.
            onStartMove?.Invoke();
        }
        else if (oldInputs != Vector2.zero && inputs == Vector2.zero)
        {
            //Was moving before but isn't now --> stop moving.
            onStopMove?.Invoke();
        }
    }

    void UpdateSlopeTransform()
    {
        slopeT.position = transform.position;
        slopeT.rotation = transform.rotation;
        slopeT.up = groundCheck.currentSlopeNormal;
    }

    Vector2 GetInput()
    {
        float x = pInput.GetInputAxisValue(inputAxis_Horizontal_index);
        float y = pInput.GetInputAxisValue(inputAxis_Vertical_index);
        inputs = new Vector2(x, y).normalized;

        return inputs;
    }

    public bool SetNewOverride(PlayerMovementOverride newOverride)
    {
        //Current Override has to be reset before new one can take it's place. This also can't be null.
        if (currentOverride != null || newOverride == null)
            return false;

        //Overrides replace default movement values.
        currentOverride = newOverride;
        acc_speed = currentOverride.override_acc_Speed;
        acc_airspeed = currentOverride.override_acc_Airspeed;
        return true;
    }
    public void ResetOverride()
    {
        //Reset values.
        currentOverride = null;
        acc_speed = acc_speed_original;
        acc_airspeed = acc_airspeed_original;
    }

    public override void OnPlayerAwake(Player _player)
    {
        GetAllComponents();
    }
    public override void OnPlayerStart(Player _player)
    {
        groundCheck.onGroundLeave.AddListener(OnAerial);
        groundCheck.onLanding.AddListener(OnLanding);
    }
    void GetAllComponents()
    {
        TryGetComponent<PlayerInput>(out pInput);
        TryGetComponent<Rigidbody>(out rb);
        TryGetComponent<PlayerGroundCheck>(out groundCheck);

        acc_speed_original = acc_speed;
        acc_airspeed_original = acc_airspeed;

        slopeT = new GameObject("SlopeTransform").transform;
    }
}
