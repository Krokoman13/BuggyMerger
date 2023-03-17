using UnityEngine;


namespace FPSepController
{
    //Smoothly transitions the Field of View between the default and a given value.
    public class SetFOV_Camera : MonoBehaviour
    {
        [SerializeField, Tooltip("The default Virtual Camera following the player")]
        Camera cam = null;

        bool isCurrentlyActive = false;

        float defaultFOV = 0;
        [SerializeField, Tooltip("The FOV when the effect is active.")]
        float alteredFOV = 80f;
        [SerializeField, Tooltip("The time it'll take for the FOV to reach the target value.")]
        float fovChangeTime = 0.1f;


        public void SetFOVState(bool turnActive)
        {
            isCurrentlyActive = turnActive;
        }

        void Update()
        {
            FOVChangeTimer();
        }

        float smoothDampVelo = 0;
        void FOVChangeTimer()
        {
            //Set target based on crouching-state
            float target = defaultFOV;
            if (isCurrentlyActive)
                target = alteredFOV;

            //Stop smoothing once target value has been reached.
            if (cam.fieldOfView == target)
                return;

            //Smoothly translate the camera to the target position
            cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, target, ref smoothDampVelo, fovChangeTime);
        }


        void Awake()
        {
            defaultFOV = cam.fieldOfView;
        }
    }
}