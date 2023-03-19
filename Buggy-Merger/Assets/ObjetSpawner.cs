using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetSpawner : MonoBehaviour
{
    [SerializeField] Transform mObjectPrefab;
    [SerializeField] float secDuration = 5;
    [SerializeField] Transform mine;

    bool spawning = false;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        if (mine != null || mObjectPrefab == null) return;
        spawning = false;

        mine = Instantiate(mObjectPrefab, transform);
        mine.localPosition = Vector3.zero;
        mine.rotation = Quaternion.Euler(Vector3.zero);

        Rigidbody rb;
        if (!mine.TryGetComponent<Rigidbody>(out rb)) return;
        rb.isKinematic = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (mine == null || mine.parent == transform) return;
        mine = null;
        if (!spawning) StartCoroutine(startSpawning());
    }

    IEnumerator startSpawning()
    {
        spawning = true;
        yield return new WaitForSeconds(secDuration);
        Spawn();
    }

    private void OnValidate()
    {
        Spawn();
    }
}
