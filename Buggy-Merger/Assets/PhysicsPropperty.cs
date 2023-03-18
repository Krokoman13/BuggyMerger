using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPropperty : MObjectPropperty
{
    public PhysicMaterial physicMaterial;

    public PhysicsPropperty OverrideBy (PhysicsPropperty pToClone)
    {
        priority = pToClone.priority;
        exclusive = pToClone.exclusive;
        physicMaterial = pToClone.physicMaterial;
        return this;
    }

    private void Awake()
    {
        //exclusive = true;
        //if (physicMaterial == null) Destroy(this);
    }

    public override void Apply()
    {
        Collider[] cols = GetComponents<Collider>();

        foreach (Collider col in cols)
        {
            col.material = physicMaterial;
        }
    }

    protected override void Update()
    { 
        
    }
}
