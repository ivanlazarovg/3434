using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CumMeterDisplay : MonoBehaviour
{
    public void DisplayPointsGained()
    {
        StartCoroutine(PopAndShrink());
    }

    private IEnumerator PopAndShrink()
    {
        yield return null;
    }
}
