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

        if (two.activation != null)
        {
            if (two.activation == null || two.activation.type < one.activation.type)
            {
                if (two.activation != null) Destroy(outP.activation);
                outP.activation = ComponentUtils.CopyComponent(one.activation, outP.gameObject);
                outP.activation.SetObject(outP);
            }
            else if (two.activation.type == OnActivationType.Fire)
            {
                outP.activation.Load(one);
            }
        }

        foreach (MObjectPropperty propperty in one.properties)
        {
            outP.Add(MObjectPropperty.AddClonedProperty(outP.gameObject, propperty));
        }

        outP.enabled = true;
        return outP;
    }
}
