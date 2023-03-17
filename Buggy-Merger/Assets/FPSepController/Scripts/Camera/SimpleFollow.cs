using UnityEngine;

namespace FPSepController
{
    public class SimpleFollow : MonoBehaviour
    {
        [SerializeField] Transform targetT = null;      //Transform to follow
        Transform t = null;                             //This object's transform, for caching.

        void Update()
        {
            if (targetT == null)
            {
                Debug.LogError("Target Transform not assigned, Disabling this script.", this.gameObject);
                this.enabled = false;
                return;
            }

            //Set position to target's position.
            t.position = targetT.position;
        }

        void Awake()
        {
            TryGetComponent<Transform>(out t);
        }
    }
}