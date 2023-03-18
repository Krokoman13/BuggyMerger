using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Renderer))]
public class ColorFader : MonoBehaviour
{
    [SerializeField] Color colorA;
    [SerializeField] Color colorB;

    [SerializeField] float frequancy;

    Renderer rend = null;

    private void Start()
    {
        if (rend == null) rend = GetComponent<Renderer>();
    }

    private void OnValidate()
    {
        if (rend == null) rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        float value = Mathf.Sin(Time.time * frequancy);
        value += 1;
        value /= 2;
        rend.material.color = Color.Lerp(colorA, colorB,value);
    }
}
