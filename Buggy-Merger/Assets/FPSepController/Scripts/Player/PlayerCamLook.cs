using UnityEngine;

namespace FPSepController
{
    public class PlayerCamLook : PlayerComponent
    {        
        Transform t = null;                     //Caching for convenience
        Transform camT = null;                  //Camera Transform required for rotating the player        

        [SerializeField,Tooltip("When ticked, the cursor will disappear when the player gets paused. The cursor will reappear when player is unpaused.")]
        bool disableCursorDuringGameplay = true;

        
        public override void OnPlayerUpdate()
        {
            if (camT == null)
            {
                Debug.LogError("Camera Transform not found. Is there a Camera with tag: 'MainCamera'?", this);
                return;
            }

            //Player should look in the same direction as the cam but not tilt forward/sideways, so Y axis only.
            t.eulerAngles = new Vector3(t.eulerAngles.x, camT.eulerAngles.y, t.eulerAngles.z);
        }


        public override void OnPlayerAwake(Player _player)
        {
            GetAllComponents();
            if(disableCursorDuringGameplay)
                DisableCursor();
        }

        public override void OnPlayerUnpause()
        {
            if(disableCursorDuringGameplay)
                DisableCursor();
        }
        public override void OnPlayerPause()
        {
            if(disableCursorDuringGameplay)
                EnableCursor();
        }

        public void EnableCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void DisableCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }        



        void GetAllComponents()
        {
            t = this.transform;
            camT = Camera.main.transform;
        }
    }
}