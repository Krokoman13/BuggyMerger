using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class MObjectMaterialPropperty : MObjectPropperty
{
    public List<Material> materials = null;

    public bool first = false;
    MObject mObject;

    public int preferedColor = -1;

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
        if (!first && preferedColor < 1) RandomExtention.Shuffle(materials);

        mObject = GetComponent<MObject>();
        int matCount = 0;

        foreach (Transform child in mObject.model)
        {
            MeshRenderer renderer;
            if (!child.TryGetComponent(out renderer)) continue;

            Material[] array = new Material[renderer.materials.Length];

            for (int i = 0; i < renderer.materials.Count(); i++)
            {
                if (!first && preferedColor >= 0) matCount = preferedColor;
                if (matCount >= materials.Count) matCount = 0;
                array[i] = materials[matCount];
                matCount++;
            }
            
            renderer.materials = array;
        }

        first = false;
    }

    public MObjectMaterialPropperty overrideWith(MObjectMaterialPropperty toClone)
    {
        first = false;
        materials = toClone.materials;
        preferedColor = toClone.preferedColor;
        return this;
    }
}