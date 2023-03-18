using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilIenumerator : MonoBehaviour
{
    public static IEnumerator DelayAction(Action pAction, float pSeconds)
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(pSeconds);

        pAction();
    }
}
