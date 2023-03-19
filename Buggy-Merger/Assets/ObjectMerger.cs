using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MObjectActivation;

public class ObjectMerger : MonoBehaviour
{
    public static MObject Merge(MObject one, MObject two)
    {
        if (one == null || two == null) return null;

        two.enabled = false;
        MObject outP = Instantiate(two);
        two.enabled = true;

        if (one.modelPrio > outP.modelPrio)
        {
            Destroy(outP.model.gameObject);
            //outP.model = Instantiate(one.model, outP.transform);
            outP.model.localPosition = one.transform.localPosition;
        }

        if (outP.activation.type == OnActivationType.Fire || outP.activation.mustLoad)
        {
            outP.activation.Load(one);
        }
        else
        {
            if (one.activation.mustLoad) outP.activation.mustLoad = true;

            if (two.activation.type < one.activation.type)
            {
                MObjectActivation newActivation = outP.activation;
                MObjectActivation oneActivation = one.activation;

                newActivation.type = oneActivation.type;
                newActivation.ammo = oneActivation.ammo;

                newActivation.shootSpeed = oneActivation.shootSpeed;
                newActivation.throwAngle = oneActivation.throwAngle;
                newActivation.throwForce = oneActivation.throwForce;

                if (oneActivation.type == OnActivationType.Fire || oneActivation.mustLoad) newActivation.Load(two);
            }

            foreach (MObjectPropperty propperty in one.properties)
            {
                outP.Add(MObjectPropperty.AddClonedProperty(outP.gameObject, propperty));
            }
        }

        outP.enabled = true;
        return outP;
    }
}
