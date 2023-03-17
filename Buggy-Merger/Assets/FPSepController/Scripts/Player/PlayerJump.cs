using UnityEngine.Events;
using UnityEngine;

namespace FPSepController
{
    [RequireComponent(typeof(PlayerJump))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerGroundCheck))]
    public class PlayerJump : PlayerComponent
    {
        Rigidbody rb = null;
        PlayerInput pInput = null;
        PlayerGroundCheck groundCheck = null;
        PlayerCrouching pCrouch = null;

        [SerializeField, Tooltip("Index for the jump input, as set in PlayerInputs")]
        int inputIndex = 0;

        [Header("Jump Variables")]
        [SerializeField, Tooltip("Player's Y-velocity will be set to this value, acting as an instant momentum boost upwards.")]
        float jumpForce = 10;
        [SerializeField, Tooltip("The amount of times the player will be able to jump whilst mid-air. This exludes the ground-jump and refreshes upon landing.")]
        int airjumpAmount = 1;
        int currentAirjumpAmount = 0;


        public UnityEvent onJump = null;

        void DoJump()
        {            
            rb.useGravity = true;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            onJump?.Invoke();
        }


        void JumpInput()
        {
            //Jumping allowed if grounded...
            if (groundCheck.isGrounded && CrouchPreventJump())
            {
                DoJump();
            }
            //...or airborne with a spare airjump.
            if (!groundCheck.isGrounded && currentAirjumpAmount > 0)
            {
                DoJump();
                currentAirjumpAmount--;
            }
        }

        void RefreshAirjumps()
        {
            currentAirjumpAmount = airjumpAmount;
        }

        bool CrouchPreventJump()
        {
            return pCrouch != null && pCrouch.CanUncrouch();
        }


        public override void OnPlayerAwake(Player pPlayer)
        {
            GetAllComponents();
        }
        public override void OnPlayerStart(Player _player)
        {
            pInput.allButtons[inputIndex].onButtonDown += JumpInput;
            groundCheck.onLanding.AddListener(RefreshAirjumps);
        }

        void GetAllComponents()
        {
            TryGetComponent<Rigidbody>(out rb);
            TryGetComponent<PlayerInput>(out pInput);
            TryGetComponent<PlayerGroundCheck>(out groundCheck);
            TryGetComponent<PlayerCrouching>(out pCrouch);
        }
    }
}
