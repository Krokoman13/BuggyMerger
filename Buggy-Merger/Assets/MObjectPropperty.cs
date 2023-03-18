using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MObjectPropperty : MonoBehaviour
{
    [NonSerialized] public bool exclusive = false;
    public int priority = 1;

    public static MObjectPropperty AddClonedProperty<T>(GameObject pTarget, T pToClone) where T: MObjectPropperty
    {
        T cloned = pTarget.AddComponent(pToClone.GetType()) as T;

        switch (cloned)
        {
            case PhysicsPropperty pP:
                return pP.OverrideBy(pToClone as PhysicsPropperty); ;
        }

        return null;
    }

    public virtual void Apply()
    {

    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    public virtual void OnDestroy()
    {

    }
}
