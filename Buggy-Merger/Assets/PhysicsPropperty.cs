using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPropperty : MObjectMaterialPropperty
{
    public PhysicMaterial physicMaterial;

    public PhysicsPropperty OverrideWith (PhysicsPropperty pToClone)
    {
        base.overrideWith(pToClone);
        physicMaterial = pToClone.physicMaterial;
        return this;
    }

    public override void Apply()
    {
        base.Apply();

        foreach (Transform child in transform)
        {
            Collider[] cols = child.GetComponents<Collider>();

            foreach (Collider col in cols)
            {
                col.material = physicMaterial;
            }
        }
    }

    protected override void Update()
    { 
        
    }
}
