using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MObjectActivation;

public class ObjectMerger : MonoBehaviour
{
    public static MObject Merge(MObject one, MObject two)
    {
        two.enabled = false;
        MObject outP = Instantiate(two);
        two.enabled = true;

        if (two.activation.type == OnActivationType.Fire)
        {
            outP.activation.Load(one);
        }
        else if (one.activation.type != OnActivationType.Fire && two.activation.type < one.activation.type)
        {
            Destroy(outP.activation);
            //outP.activation = ComponentUtils.CopyComponent(one.activation, outP.gameObject);
            outP.activation = MObjectActivation.AddComponentClone(outP.gameObject, one.activation);
            outP.activation.SetObject(outP);
        }

        foreach (MObjectPropperty propperty in one.properties)
        {
            outP.Add(MObjectPropperty.AddClonedProperty(outP.gameObject, propperty));
        }

        outP.enabled = true;
        return outP;
    }
}
