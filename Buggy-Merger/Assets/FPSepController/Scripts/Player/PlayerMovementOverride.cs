using UnityEngine;

namespace FPSepController
{
    [System.Serializable]
    public class PlayerMovementOverride
    {        
        public bool useOverride_acc_Speed = true;
        [Tooltip("The new grounded acceleration the player will have when this override is set. Use negative values to ignore.")]
        public float override_acc_Speed;

        public bool useOverride_Airspeed = true;
        [Tooltip("The new aerial acceleration the player will have when this override is set. Use negative values to ignore.")]
        public float override_acc_Airspeed;
    }
}