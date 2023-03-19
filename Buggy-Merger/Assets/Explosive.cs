using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Explosive : MonoBehaviour
{
    Rigidbody rb = null;
    [SerializeField] bool primed = false;

    public float power = 5f;
    public float durationSeconds = 5f;
    uint amount = 5u;

    MObjectActivation activation = null;

    public void Awake()
    {
        activation = GetComponent<MObjectActivation>();
        activation.mustLoad = true;
        activation.onActivate.AddListener(() => StartCoroutine(startExploding()));

        rb = GetComponent<Rigidbody>();
    }

    IEnumerator startExploding()
    {
        primed = true;
        yield return new WaitForSeconds(durationSeconds);
        Destroy(gameObject);
    }

    private void explode()
    {
        if (!primed || activation.ammo == null) return;

        for (int i = 0; i < amount; i++)
        {
            Rigidbody projectile = Instantiate(activation.ammo);
            projectile.gameObject.SetActive(true);
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            projectile.position = transform.position;
            direction = direction.normalized * power;
            projectile.velocity = direction;
            projectile.angularVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

            MObject mToShoot;
            projectile.TryGetComponent<MObject>(out mToShoot);
            if (mToShoot != null) mToShoot.activation.onActivate?.Invoke();
        }
    }

    public void OnDestroy()
    {
        explode();
    }
}
