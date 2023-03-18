using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Utils
{
    public static IEnumerator DelayedFunction(Action action, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }
}
