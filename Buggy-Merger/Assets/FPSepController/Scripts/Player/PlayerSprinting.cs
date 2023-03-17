using UnityEngine;
using UnityEngine.Events;

namespace FPSepController
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerGroundCheck))]
    public class PlayerSprinting : PlayerComponent
    {
        PlayerMovement pMovement = null;
        PlayerInput pInput = null;
        PlayerGroundCheck groundcheck = null;

        [SerializeField, Tooltip("Index of the Sprinting button")]
        int inputIndex = 2;

        //Sprinting State
        bool sprintInput = false;
        [HideInInspector] public bool isSprinting = false;

        [SerializeField] PlayerMovementOverride sprintMovementOverride = null;

        [Header("Events")]
        [SerializeField, Tooltip("Triggered when player starts sprinting")]
        UnityEvent onSprint = null;
        [SerializeField, Tooltip("Triggered when player stops sprinting")]
        UnityEvent onEndSprint = null;



        public override void OnPlayerFixedUpdate()
        {
            //Cancel sprint if movement input isn't forward-based or the ground is too steep.
            bool sprintingNotAllowed = pMovement.inputs.y <= 0 || groundcheck.tooSteep;

            //Currently Sprinting
            if (isSprinting)
            {
                if (sprintingNotAllowed || !sprintInput)
                {
                    pMovement.ResetOverride();
                    OnSprintStop();
                }
            }
            //Currently not sprinting
            else if (sprintInput && !sprintingNotAllowed)
            {
                //Only allow sprinting if movement doesn't already have another override active.
                if (pMovement.SetNewOverride(sprintMovementOverride))
                    OnSprint();
            }
        }


        void OnSprint()
        {
            isSprinting = true;
            onSprint?.Invoke();
        }

        void OnSprintStop()
        {
            isSprinting = false;
            onEndSprint?.Invoke();
        }

        public override void OnPlayerStart(Player pPlayer)
        {
            TryGetComponent<PlayerInput>(out pInput);
            TryGetComponent<PlayerMovement>(out pMovement);
            TryGetComponent<PlayerGroundCheck>(out groundcheck);
        }

        public override void OnPlayerUpdate()
        {            
            sprintInput = pInput.GetInputButtonValue(inputIndex);
        }

    }
}