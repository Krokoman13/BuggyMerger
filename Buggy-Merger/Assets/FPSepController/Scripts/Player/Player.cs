using System;
using UnityEngine;

namespace FPSepController
{
    /// <summary>
    /// The Player's core component. Any PlayerComponents must have reference to this in order to use synced methods.
    /// </summary>
    public class Player : MonoBehaviour
    {
        //PLAYER COMPONENTS
        public delegate void PlayerDelegate(Player player);
        public PlayerDelegate onPlayerAwake = null;
        public PlayerDelegate onPlayerStart = null;
        public Action onPlayerUpdate = null;
        public Action onPlayerFixedUpdate = null;
        public Action onPlayerDeath = null;

        //PAUSE
        public Action onPause = null;
        public Action onUnpause = null;
        bool isPaused = false;

        [SerializeField, Tooltip("When ticked, the player will run it's pause/unpause events when the 'Player'-Component gets Disabled/Enabled.")]
        bool pauseOnDisable = true;


        void Awake()
        {            
            onPlayerAwake?.Invoke(this);            
        }
        void Start()
        {          
            onPlayerStart?.Invoke(this);
        }

        void Update()
        {
            if (isPaused) return;
            onPlayerUpdate?.Invoke();
        }

        void FixedUpdate()
        {
            if (isPaused) return;
            onPlayerFixedUpdate?.Invoke();
        }        


        public void OnPause()
        {
            isPaused = true;
            onPause?.Invoke();
        }
        public void OnUnpause()
        {
            isPaused = false;
            onUnpause?.Invoke();
        }

        void OnEnable()
        {
            if(pauseOnDisable)
                onUnpause?.Invoke();
        }
        void OnDisable()
        {
            if (pauseOnDisable)
                onPause?.Invoke();
        }


        public void PlayerDead()
        {
            onPlayerDeath?.Invoke();
        }
    }
}
