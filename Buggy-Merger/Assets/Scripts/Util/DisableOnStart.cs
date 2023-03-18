using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnStart : MonoBehaviour
{
    [SerializeField] bool disable;

    // Start is called before the first frame update
    private void Update()
    {
        gameObject.SetActive(!disable);
        Destroy(this);
    }
}
