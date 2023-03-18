using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switcher : MonoBehaviour
{
    public void Activate(int childIndex)
    {
        if (transform.childCount - 1 < childIndex) return;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            child.gameObject.SetActive(i == childIndex);
        }
    }
}
