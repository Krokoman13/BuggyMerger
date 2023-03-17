using UnityEngine;

namespace FPSepController
{
    /// <summary>
    /// Inherit from this and receive access to core player methods/info
    /// </summary>
    public abstract class PlayerComponent : MonoBehaviour
    {
        [HideInInspector] public Player subject = null;
        bool eventsAssigned = false;
        void Awake()
        {
            //Player-Components must always be attached to the Player-Object.
            if (!TryGetComponent<Player>(out subject))
                Debug.LogError($"{this} couldn't find 'Player' component.\nEnsure this object has such component attached.", this.gameObject);

            AssignEvents();
            OnPlayerAwake(subject);
        }

        void AssignEvents()
        {
            subject.onPlayerStart += OnPlayerStart;
            subject.onPlayerUpdate += OnPlayerUpdate;
            subject.onPlayerFixedUpdate += OnPlayerFixedUpdate;
            subject.onPause += OnPlayerPause;
            subject.onUnpause += OnPlayerUnpause;
            subject.onPlayerDeath += OnPlayerDeath;
            eventsAssigned = true;
        }

        void DeassignEvents()
        {
            subject.onPlayerUpdate -= OnPlayerUpdate;
            subject.onPlayerFixedUpdate -= OnPlayerFixedUpdate;
            subject.onPause -= OnPlayerPause;
            subject.onUnpause -= OnPlayerUnpause;
            subject.onPlayerDeath -= OnPlayerDeath;
            eventsAssigned = false;
        }

        void OnDisable()
        {
            if (eventsAssigned)
                DeassignEvents();            
        }
        void OnDestroy()
        {
            if (eventsAssigned)
                DeassignEvents();
        }

        void OnEnable()
        {
            if (!eventsAssigned)
                AssignEvents();            
        }


        //Start will pass a player-reference -- makes it easier to link back to it.
        public virtual void OnPlayerAwake(Player _player) { }
        public virtual void OnPlayerStart(Player _player) { }
        public virtual void OnPlayerUpdate() { }
        public virtual void OnPlayerFixedUpdate() { }
        public virtual void OnPlayerPause() { }
        public virtual void OnPlayerUnpause() { }
        public virtual void OnPlayerDeath() { }
    }
}