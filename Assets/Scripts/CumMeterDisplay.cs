using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CumMeterDisplay : MonoBehaviour
{
    public Slider cumSlider;
    public GameObject scoreTextPrefab;

    private float scoreDisplay;

    float increaseTimer = 0;

    float currentStartingValue;
    float currentEndingValue;

    public float textPopUpScale;

    public void DisplayPointsGained(int pointIncrease)
    {
        Vector3 positionRandomized = cumSlider.handleRect.position + new Vector3(0, Random.Range(15, 35), 0);
        GameObject textInstance = Instantiate(scoreTextPrefab, positionRandomized, Quaternion.identity, cumSlider.transform.parent);
        textInstance.GetComponent<TextMeshProUGUI>().text = pointIncrease.ToString();


        StartCoroutine(PopAndShrink(textInstance, positionRandomized));
    }

    private IEnumerator PopAndShrink(GameObject textInstance, Vector3 position)
    {
        textInstance.transform.localScale = Vector3.zero;

        float t = 0;

        while(t < 1)
        {
            t += Time.deltaTime * 2;
            textInstance.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(textPopUpScale, textPopUpScale, textPopUpScale), t);
            textInstance.transform.position = position;
            yield return null;
        }

        yield return new WaitForSeconds(0.7f);

        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 2;
            textInstance.transform.localScale = Vector3.Lerp(new Vector3(textPopUpScale, textPopUpScale, textPopUpScale), Vector3.zero, t);
            yield return null;
        }
        Destroy(textInstance);
    }

    public void SetSliderValue(float valueToAdd)
    {
        currentStartingValue = cumSlider.value;
        currentEndingValue = cumSlider.value + valueToAdd;

        increaseTimer = 0;
    }


    private void Update()
    {
        cumSlider.value = Mathf.Lerp(currentStartingValue, currentEndingValue, increaseTimer);

        increaseTimer += Time.deltaTime;
    }
}
