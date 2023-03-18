using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOpacityOfMaterial : MonoBehaviour
{
    [SerializeField] float startP = 100f;
    [SerializeField] float stop = 0f;

    [SerializeField] float frequancy;

    Renderer rend = null;

    private void Start()
    {
        if (rend == null) rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        float value = Mathf.Sin(Time.time * frequancy);
        value += 1;
        value /= 2;

        rend.material.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), value);
    }
}
