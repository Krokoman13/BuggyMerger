using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveChild : MonoBehaviour
{
    public List<Transform> children;

    private void OnDestroy()
    {
        foreach(Transform child in children) child.SetParent(null);
    }
}
