using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class MObjectMaterialPropperty : MObjectPropperty
{
    public List<Material> materials = null;

    [NonSerialized] public bool first = true;
    MObject mObject;


    public virtual void Awake()
    {
        if (materials == null || materials.Count > 1) return;

        mObject = GetComponent<MObject>();

        foreach (Transform child in mObject.model)
        {
            MeshRenderer renderer;
            if (!child.TryGetComponent(out renderer)) continue;

            for (int i = 0; i < renderer.materials.Count(); i++)
            {
                materials.Add(renderer.materials[i]);
            }
        }
    }

    public override void Apply()
    {
        if (materials.Count < 1) return;
        if (!first) RandomExtention.Shuffle(materials);

        mObject = GetComponent<MObject>();
        int matCount = 0;

        foreach (Transform child in mObject.model)
        {
            MeshRenderer renderer;
            if (!child.TryGetComponent(out renderer)) continue;

            Material[] array = new Material[renderer.materials.Length];

            for (int i = 0; i < renderer.materials.Count(); i++)
            {
                if (matCount >= materials.Count) matCount = 0;
                array[i] = materials[matCount];
                matCount++;
            }

            first = false;
            renderer.materials = array;
        }
    }

    public MObjectMaterialPropperty overrideWith(MObjectMaterialPropperty toClone)
    {
        first = toClone.first;
        materials = toClone.materials;
        return this;
    }
}