using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveChild : MonoBehaviour
{
    public List<Transform> children;

    private void OnDestroy()
    {
        if (transform.parent != null) return;

        foreach (Transform child in children)
        {
            child.SetParent(null);
            child.gameObject.SetActive(true);
        }
    }
}
