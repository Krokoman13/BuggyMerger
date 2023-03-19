using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnDestruction : MonoBehaviour
{
    public UnityEvent onDestruction = null;

    private void Awake()
    {
        onDestruction.AddListener(() => Debug.Log("Destroyed"));
    }

    private void OnDestroy()
    {
        onDestruction?.Invoke();
    }
}
