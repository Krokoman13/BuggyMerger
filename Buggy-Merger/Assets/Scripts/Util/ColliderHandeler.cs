using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class ColliderHandeler : MonoBehaviour
{
    [SerializeField] bool destroyOnCollision;

    [SerializeField] List<string> tags = new List<string> { "Untagged" };

    [SerializeField] UnityEvent onCollision;

    private void Awake()
    {
        Collider collider = null;
        bool found = TryGetComponent<Collider>(out collider);

        if (!found)
        {
            Debug.LogWarning("No collider is attached to the GameObject " + gameObject.name + " the ColliderHandeler cannot function without one");
            return;
        }

        if (!collider.isTrigger)
        {
            Debug.LogWarning("The attached collider to the GameObject " + gameObject.name + " is not marked as trigger, this makes the ColliderHandeler not function");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (tags.Contains(other.gameObject.tag))
        {
            onCollision.Invoke();
            if (destroyOnCollision) Destroy(gameObject);
        }
    }
}
