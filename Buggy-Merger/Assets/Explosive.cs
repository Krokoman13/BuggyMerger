using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Explosive : MObjectPropperty
{
    Rigidbody rb = null;
    [SerializeField] bool exploding = false;
    [SerializeField] bool primed = false;

    public float power = 5f;
    public float durationSeconds = 5f;
    uint amount = 5u;

    MObjectActivation activation = null;

    public override void Apply()
    {
        activation = GetComponent<MObjectActivation>();
        activation.mustLoad = true;

        rb = GetComponent<Rigidbody>();
    }

    protected override void Update()
    {
        if (exploding) return;

        if (!primed)
        {
            if (rb.isKinematic) primed = true;
            return;
        }

        if (primed && rb.isKinematic) return;

        StartCoroutine(startExploding());
    }

    IEnumerator startExploding()
    {
        exploding = true;
        yield return new WaitForSeconds(durationSeconds);
        Destroy(gameObject);
    }

    private void explode()
    {
        if (activation.ammo == null) return;

        for (int i = 0; i < amount; i++)
        {
            Rigidbody projectile = Instantiate(activation.ammo);
            projectile.gameObject.SetActive(true);
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            projectile.position = transform.position + direction;
            direction = direction.normalized * power;
            projectile.velocity = direction;
            projectile.angularVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }
    }

    public override void OnDestroy()
    {
        if (rb.isKinematic) return;
        explode();
    }
}
