using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CumMeterDisplay : MonoBehaviour
{
    public Slider cumSlider;
    public GameObject scoreTextPrefab;
    public GameObject doubleScoreText;

    private float scoreDisplay;

    float increaseTimer = 0;

    float currentStartingValue;
    float currentEndingValue;

    public float textPopUpScale;

    [Header("Shake")]
    Vector3 startPos;
    float shivertimer = 0;

    float shiverMultiplier1;
    float shiverMultiplier2;

    public float shiverRangeDown = 4;
    public float shiverRangeUp = 30;

    private void Start()
    {
        startPos = cumSlider.transform.position;
    }

    public void DisplayPointsGained(int pointIncrease)
    {
        Vector3 positionRandomized = cumSlider.handleRect.position + new Vector3(0, Random.Range(25, 45), 0);
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
            t += Time.deltaTime * (1+(1-t)) * 2;
            textInstance.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(textPopUpScale, textPopUpScale, textPopUpScale) * 2, t);
            textInstance.transform.position = position;
            yield return null;
        }
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 5;
            textInstance.transform.localScale = Vector3.Lerp(new Vector3(textPopUpScale, textPopUpScale, textPopUpScale) * 2, new Vector3(textPopUpScale, textPopUpScale, textPopUpScale), t);
            textInstance.transform.position = position;
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * (1 + (1-t) * 2);
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
        if (PowerUpManager.Instance.isLubed)
        {
            Shiver();
            doubleScoreText.SetActive(true);
        }
        else
        {
            cumSlider.transform.position = startPos;
            doubleScoreText.SetActive(false);
        }
       
        cumSlider.value = Mathf.Lerp(currentStartingValue, currentEndingValue, increaseTimer);
        increaseTimer += Time.deltaTime;
    }

    void Shiver()
    {
        cumSlider.transform.position = startPos + new Vector3(Mathf.Sin(Time.time * shiverMultiplier1), Mathf.Sin(Time.time * shiverMultiplier2), 0) * 2f;

        shivertimer += Time.deltaTime;

        if (shivertimer > 0.2f)
        {
            shiverMultiplier1 = Random.Range(shiverRangeDown, shiverRangeUp);
            shiverMultiplier2 = Random.Range(shiverRangeDown, shiverRangeUp);
            int randomcheck = Random.Range(1, 3);
            int randomcheck2 = Random.Range(1, 3);

            if (randomcheck == 1)
            {
                shiverMultiplier1 = shiverMultiplier1;
            }
            else
            {
                shiverMultiplier1 = -shiverMultiplier1;
            }

            if (randomcheck2 == 2)
            {
                shiverMultiplier2 = shiverMultiplier2;
            }
            else
            {
                shiverMultiplier2 = -shiverMultiplier2;
            }

            shivertimer = 0;


        }
    }
}
