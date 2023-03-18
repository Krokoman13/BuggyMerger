using FPSepController;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectSelector : PlayerComponent
{
    [SerializeField] MObject current = null;
    public float range = 1;
    public float frequency = 1;
    [SerializeField] List<string> tags;

    MObject inLeft;
    MObject inRight;

    [SerializeField] Transform handLeft;
    [SerializeField] Transform handRight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (current != null)
            {

                equipLeftHand(current);
            }
            else if (inLeft != null)
            {
                inLeft.Activate();
                if (inLeft.transform.parent != handLeft) inLeft = null;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (inRight != null)
            {
                inRight.Activate();
                if (inRight.transform.parent != handRight) inRight = null;
            }
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

        if (Physics.Raycast(ray, out hit, range))
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

        Outline outline = pCurrent.outline;
        outline.enabled = true;
    }

    private void deselct()
    {
        if (current == null) return;

        Outline outline = current.outline;
        outline.enabled = false;

        current = null;
    }

    private void equipLeftHand(MObject mObject)
    {
        if (mObject == null) return;

        if (inLeft != null)
        {
            equipRightHand(inLeft);
        }

        inLeft = mObject;

        mObject.transform.SetParent(handLeft);
        lockObject(mObject);
    }

    private void equipRightHand(MObject mObject)
    {
        if (mObject == null) return;

        if (inRight != null)
        {
            MObjectActivation.Drop(inRight);
        }

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
