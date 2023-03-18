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

    public enum OnActivationType {Drop = 0, Place = 1, Throw = 2, Fire = 3}
    public OnActivationType type = OnActivationType.Drop;

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
            case OnActivationType.Drop:
                Drop(mObject);
                break;
            case OnActivationType.Place:
                activatePlace(mObject, groundDetectMask, 3f);
                break;
            case OnActivationType.Throw:
                activateThrow(mObject, (mObject.transform.forward + throwAngle).normalized * throwForce);
                break;
            case OnActivationType.Fire:
                activateFire(shootSpeed);
                break;
        }
    }

    private void activateFire(float force)
    {
        if (ammo == null) return;

        Rigidbody toShoot = Instantiate(ammo);
        toShoot.gameObject.SetActive(true);
        toShoot.transform.position = mObject.transform.position + (mObject.transform.forward / 2f);
        toShoot.transform.rotation = mObject.transform.rotation;
        toShoot.velocity = (toShoot.transform.forward * force);
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

        Vector3 rotation = pMObject.transform.eulerAngles;
        rotation.x = 0f;
        rotation.z = 0f;
        pMObject.transform.rotation = Quaternion.Euler(rotation);

        pMObject.transform.position = point;
        Drop(pMObject);
    }

    public void Load(MObject mObject)
    {
        Drop(mObject);
        MObject oldAmmo = ammo.GetComponent<MObject>();
        ammo = Instantiate(mObject, transform).GetComponent<Rigidbody>();
        ammo.transform.localPosition = Vector3.zero;
        ammo.gameObject.SetActive(false);
        Destroy(oldAmmo.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        //if (mObject == null) return;
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, transform.position + (transform.forward + throwAngle).normalized * throwForce);
    }

    internal static MObjectActivation AddComponentClone(GameObject target, MObjectActivation toClone)
    {
        MObjectActivation cloned = target.AddComponent<MObjectActivation>();
        cloned.type = toClone.type;
        cloned.ammo = toClone.ammo;

        cloned.shootSpeed = toClone.shootSpeed;
        cloned.throwAngle = toClone.throwAngle;
        cloned.throwForce = toClone.throwForce;

        return cloned;
    }
}