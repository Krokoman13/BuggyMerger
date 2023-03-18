using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MObject))]
public class MObjectActivation : MonoBehaviour
{
    private MObject mObject;

    public enum OnActivationType {Place = 0, Throw = 1, Fire = 2}
    public OnActivationType type = OnActivationType.Place;

    public Rigidbody defaultAmmoPrefab;
    public Rigidbody ammo;
    public float shootSpeed;
    
    public Vector3 throwAngle;
    public float throwForce;

    LayerMask groundDetectMask = 1;

    private void Awake()
    {
        mObject = GetComponent<MObject>();
        mObject.activation = this;
    }

    public void SetObject(MObject pMObject)
    {
        mObject = pMObject;
    }

    public void Activate()
    {
        switch (type)
        {
            case OnActivationType.Place:
                activatePlace(mObject, groundDetectMask, 3f);
                break;
            case OnActivationType.Throw:
                activateThrow(mObject, (mObject.transform.forward + throwAngle).normalized * throwForce);
                break;
            case OnActivationType.Fire:
                activateFire();
                break;
        }
    }

    private void activateFire()
    {
        Rigidbody toShootPrefab = ammo != null ? ammo : defaultAmmoPrefab;
        if (toShootPrefab == null) return;

        Rigidbody toShoot = Instantiate(toShootPrefab);
        toShoot.transform.position = mObject.transform.position;

    }

    public static void activateThrow(MObject pMObject, Vector3 pForce)
    {
        Rigidbody rb = pMObject.GetComponent<Rigidbody>();
        pMObject.transform.position += pMObject.transform.forward;
        Drop(pMObject);
        rb.velocity = pForce;
        rb.AddForceAtPosition(pForce * Random.Range(0f, 1f), pMObject.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)));
    }

    public static void Drop(MObject mObject)
    {
        Rigidbody rb = mObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
        }

        mObject.transform.parent = null;
        mObject.transform.localScale = Vector3.one;
    }

    public static void activatePlace(MObject pMObject, LayerMask pGroundLayerMask, float pRange )
    {
        RaycastHit hit;
        Camera cam = Camera.main;

        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        Vector3 point;

        if (Physics.Raycast(ray, out hit, pRange, pGroundLayerMask))
        {
            point = hit.point;
        }
        else if (Physics.Raycast(ray.origin + ray.direction, Vector3.down * pRange, out hit, 3, pGroundLayerMask))
        {
            point = hit.point;
        }
        else
        {
            point = ray.origin + (ray.direction * pRange) + (Vector3.down * pRange);
        }

        pMObject.transform.position = point;
        Drop(pMObject);
    }

    public MObject Load(MObject mObject)
    {
        MObject oldAmmo = ammo.GetComponent<MObject>();
        ammo = mObject.GetComponent<Rigidbody>();

        return oldAmmo;
    }

    private void OnDrawGizmosSelected()
    {
        //if (mObject == null) return;
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, transform.position + (transform.forward + throwAngle).normalized * throwForce);
    }
}