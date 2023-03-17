using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TransformPlacer : MonoBehaviour
{
    Transform currentTransform;

    // Update is called once per frame
    protected virtual void Update()
    {
        if (currentTransform == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            OnPlace(currentTransform);
            currentTransform = null;
            return;
        }

        currentTransform.position = Input.mousePosition;
    }

    public virtual void SetCurrentTransform(Transform pTransform)
    {
        currentTransform = pTransform;
    }

    public virtual void OnPlace(Transform pTransform)
    { 
        
    }
}
