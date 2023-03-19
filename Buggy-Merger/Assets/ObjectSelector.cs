using FPSepController;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ObjectSelector : PlayerComponent
{
    [SerializeField] MObject current = null;
    public float range = 1;
    public float frequency = 1;
    [SerializeField] List<string> tags;
    [SerializeField] LayerMask mask;

    MObject inLeft;
    MObject inRight;

    [SerializeField] Transform handLeft;
    [SerializeField] Transform handRight;

    [SerializeField] UnityEvent onSeeObject = null;
    [SerializeField] UnityEvent onLostObject = null;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (inLeft != null)
            {
                inLeft.Activate();
                if (inLeft.transform.parent != handLeft) inLeft = null;
            }
            else if (current != null)
            {
                equipLeftHand(current);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (inRight != null)
            {
                inRight.Activate();
                if (inRight.transform.parent != handRight) inRight = null;
            }
            else if (current != null)
            {
                equipRightHand(current);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            MObjectActivation.Drop(inLeft);
            MObjectActivation.Drop(inRight);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            MObject oldLeft = inLeft;
            MObject oldRight = inRight;

            equipLeftHand(oldRight);
            equipRightHand(oldLeft);
        }

        if (Input.GetMouseButtonDown(2) && inLeft != null && inRight != null)
        {
            MObject newObject = ObjectMerger.Merge(inLeft, inRight);
            Destroy(inLeft.gameObject);
            Destroy(inRight.gameObject);
            equipLeftHand(newObject);
        }

        if (Time.frameCount % (1f/frequency) != 0)
        {
            return;
        }

        RaycastHit hit;
        Camera cam = Camera.main;

        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width/2f, Screen.height / 2f, 0f));

        if (Physics.Raycast(ray, out hit, range, mask))
        {
            Transform objectHit = hit.transform;

            if (tags.Contains(objectHit.tag))
            {
                MObject obj = objectHit.GetComponent<MObject>();

                if (obj != null)
                {
                    select(obj);
                    return;
                }
            }
        }

        deselct();
    }

    private void select(MObject pCurrent)
    {
        deselct();

        if (pCurrent == null) return;
        current = pCurrent;

        onSeeObject?.Invoke();
        //Outline outline = pCurrent.outline;
        //outline.enabled = true;
    }

    private void deselct()
    {
        if (current == null) return;

        onLostObject?.Invoke();
        //Outline outline = current.outline;
        //outline.enabled = false;

        current = null;
    }

    private void equipLeftHand(MObject mObject)
    {
        if (mObject == null) return;

        inLeft = mObject;

        mObject.transform.SetParent(handLeft);
        lockObject(mObject);
    }

    private void equipRightHand(MObject mObject)
    {
        if (mObject == null) return;

        inRight = mObject;

        mObject.transform.SetParent(handRight);
        lockObject(mObject);
    }

    private static void lockObject(MObject mObject)
    {
        Rigidbody rb = mObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        mObject.transform.localPosition = Vector3.zero;
        mObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        mObject.transform.localScale = Vector3.one;
    }


}
