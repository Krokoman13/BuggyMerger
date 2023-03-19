using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class MObject : MonoBehaviour
{
    //public Outline outline;
    public MObjectActivation activation;

    public List<MObjectPropperty> properties;
    private Type getProppertyType;

    [NonSerialized] private Transform _model;

    public Transform model
    {
        get 
        {
            if (_model != null) return _model;
            _model = transform.GetChild(0);
            return _model;
        }
    }
    [HideInInspector] public UnityEvent onPickup = null;

    public int modelPrio;

    private void Awake()
    {
        //outline = GetComponent<Outline>();

        //if (outline == null) outline = gameObject.AddComponent<Outline>();

        //outline.OutlineColor = Color.green;
        //gameObject.layer = 6;
        //outline.OutlineWidth = 10;
        gameObject.tag = nameof(MObject);

        List< MObjectPropperty > mObjectPropperties = new List<MObjectPropperty>(GetComponents<MObjectPropperty>());
        mObjectPropperties.Sort(delegate (MObjectPropperty c1, MObjectPropperty c2) { return c1.priority.CompareTo(c2.priority); });

        foreach (MObjectPropperty propperty in mObjectPropperties)
        {
            Add(propperty);
        }

        onPickup = model.GetComponent<EventBehaviour>()?.toActivate;
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(UtilIenumerator.DelayAction(() => outline.enabled = false, 0.1f));

        foreach (MObjectPropperty propperty in properties)
        {
            propperty.Apply();
        }
    }

    public void Activate()
    {
        activation?.Activate();
    }

    public void Add(MObjectPropperty propperty)
    {
        if (propperty.exclusive)
        {
            MObjectPropperty current = getProprtyType(propperty.GetType());

            if (current != null && current != propperty)
            {
                if (propperty.priority < current.priority) return;
                properties.Remove(current);
                Destroy(current);
            }
        }

        properties.Add(propperty);
    }

    public MObjectPropperty getProprtyType(Type t)
    {
        foreach (MObjectPropperty propperty in properties)
        {
            if (propperty.GetType() == t) return propperty;
        }

        return null;
    }
}