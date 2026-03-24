using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CumMeterDisplay : MonoBehaviour
{
    public Slider cumSlider;

    public void DisplayPointsGained()
    {
        StartCoroutine(PopAndShrink());
    }

    private IEnumerator PopAndShrink()
    {
        yield return null;
    }

    public void SetSliderValue(float valueToSet)
    {
        cumSlider.value = valueToSet;
    }
}
