using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshColliderMaker : MonoBehaviour
{
    public MeshFilter mesh;
    public MeshCollider meshCol;

    private void Awake()
    {
        if (Application.isEditor) return;

        TryGetComponent<MeshFilter>(out mesh);
        if (mesh == null) Destroy(this);


    }
}
