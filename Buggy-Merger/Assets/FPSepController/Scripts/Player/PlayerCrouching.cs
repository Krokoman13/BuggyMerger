using UnityEngine;
using UnityEngine.Events;

namespace FPSepController
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerCrouching : PlayerComponent
    {
        PlayerInput pInput = null;
        PlayerMovement pMovement = null;

        [SerializeField, Tooltip("Used to force the player to uncrouching state when airborne. Leave empty if you want the player to be able to freely crouch midair.")]
        PlayerGroundCheck groundcheck = null;                

        [SerializeField, Tooltip("Input Button index for crouching")]
        int inputIndex_Crouching = 1;
        bool inputIsPressed = false;        
        [HideInInspector] public bool isCurrentlyCrouching = false;

        [Header("Player Colliders")]
        [SerializeField, Tooltip("Default collider for the player.")]
        Collider col = null;
        [SerializeField, Tooltip("Collider used when player is crouching. Should ideally be shorter in height.")]
        Collider crouchCol = null;

        [Header("Camera Transition")]
        [SerializeField, Tooltip("The default position for the camera -- when not crouching.\nNote that this is an empty object that the camera will follow. Not the camera itself.")]
        Transform localCamPos_Default = null;
        [SerializeField, Tooltip("The position the camera'll take when the player is crouching.\nNote that this is an empty object. The camera will use it's position while Player is in crouched-state.")]
        Transform localCamPos_Crouching = null;
        Vector3 cachedCamPos = Vector3.zero;

        [SerializeField, Tooltip("The approximate time it will take for the camera to transition between default and crouching positions.")]
        float crouchTransitionTime = 0.08f;

        [Header("Ceiling Detecting")]
        [SerializeField, Tooltip("Determines what objects will obstruct the player from uncrouching whist under a low ceiling.")]
        LayerMask ceilingCheckMask = 1;

        [SerializeField, Tooltip("Triggered when player succesfully starts crouching.")]
        UnityEvent onCrouch = null;
        [SerializeField, Tooltip("Triggered as the player stops it's crouch. Also happens when automatically uncrouching, e.g.: after being forced to stay crouched.")]
        UnityEvent onUncrouch = null;
        

        [Space(5), SerializeField, Tooltip("Used to lower the amount of checks for uncrouching checks and ceiling detection. 1 = every frame. Somewhere around 10 should be fine for a 60 fps game. Lower when affected systems become inconsistent.")]
        int crouchCeilingCheckInterval = 5;

        Vector3 defaultCol_TopOffset = Vector3.zero;

        [SerializeField] PlayerMovementOverride crouchMovementOverride = null;

        void OnStartCrouch()
        {
            //update state
            isCurrentlyCrouching = true;            
            SwitchColliders(crouchCol, col);
            onCrouch?.Invoke();
        }

        void OnEndCrouch()
        {
            //Cannot uncrouch under low ceilings. Prevents clipping through the floor/ceiling            
            if (!CanUncrouch()) return;

            //Update state
            isCurrentlyCrouching = false;            
            pMovement.ResetOverride();
            SwitchColliders(col, crouchCol);

            onUncrouch?.Invoke();
        }


        public bool CanUncrouch()
        {            
            //Draw a line to check if there's a low ceiling that prevents us from uncrouching.
            Vector3 pos1 = crouchCol.bounds.center;
            Vector3 pos2 = col.bounds.center + defaultCol_TopOffset;
            
            Physics.Linecast(pos1, pos2, out RaycastHit hit, ceilingCheckMask, QueryTriggerInteraction.Ignore);
            Debug.DrawLine(pos1, pos2, Color.magenta);

            //Return true if no ceiling has been hit. if it has, then return false.
            return hit.collider == null;
        }



        void SwitchColliders(Collider newCol, Collider oldCol)
        {
            oldCol.gameObject.SetActive(false);
            newCol.gameObject.SetActive(true);
        }


        public override void OnPlayerUpdate()
        {
            CrouchAnimTimer();
            //inputIsPressed = pInput.allButtons[inputIndex_Crouching].isPressed;
            inputIsPressed = pInput.GetInputButtonValue(inputIndex_Crouching);

            //Only run this at a lower interval to avoid unnecessary calls.
            //(note: interval at 1 means every frame. higher means every X frames)
            if (Time.frameCount % crouchCeilingCheckInterval != 0)
                return;

            InputCrouch();
            CheckUncrouch();
        }

        void InputCrouch()
        {
            //Start crouching when input is given and we're not already crouching.
            if (!isCurrentlyCrouching && inputIsPressed )
            {
                //Ignore the input if we're airborne.
                if (groundcheck != null && !groundcheck.isGrounded)
                    return;

                //Only allow crouching when there's no other movement override currently active (e.g.: Sprinting)
                if(pMovement.SetNewOverride(crouchMovementOverride))
                    OnStartCrouch();
            }
        }

        void CheckUncrouch()
        {
            //Can only uncrouch when we are still crouching now
            if (isCurrentlyCrouching)
            {
                //Uncrouch when the button isn't being held + there's no ceiling in the way.
                if (!inputIsPressed)
                {
                    if (CanUncrouch())
                        OnEndCrouch();
                }

                //Force uncrouch when airborne regardless of button state
                if (groundcheck != null && !groundcheck.isGrounded)
                {
                    if (CanUncrouch())
                        OnEndCrouch();
                }
            }
        }


        Vector3 smoothDampVelo = Vector3.zero;
        void CrouchAnimTimer()
        {
            //Set targt based on crouching-state
            Vector3 target = cachedCamPos;
            if (isCurrentlyCrouching)
                target = localCamPos_Crouching.localPosition;

            if (localCamPos_Default.localPosition == target)
                return;

            //Smoothly translate the camera to the target position
            localCamPos_Default.localPosition = Vector3.SmoothDamp(localCamPos_Default.localPosition, target, ref smoothDampVelo, crouchTransitionTime);
        }

        public override void OnPlayerAwake(Player _player)
        {
            TryGetComponent<PlayerInput>(out pInput);
            TryGetComponent<PlayerMovement>(out pMovement);            
                        
            Debugging();
        }

        public override void OnPlayerStart(Player _player)
        {
            //float offsetY = col.bounds.center.y + (Vector3.up * col.bounds.max.y);
            defaultCol_TopOffset = Vector3.up * (col.bounds.max.y/2);
            cachedCamPos = localCamPos_Default.localPosition;
        }



        void Debugging()
        {
            //Check Colliders
            if (col == null || crouchCol == null)
            {
                Debug.LogError("Player colliders not assigned. Disabling PlayerFPSCrouching Component...", this);
                this.enabled = false;
            }
        }
    }
}