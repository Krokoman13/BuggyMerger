using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky : MObjectMaterialPropperty
{
    Rigidbody rb;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        rb.isKinematic = true;
    }
}
