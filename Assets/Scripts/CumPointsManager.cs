
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CumPointsManager : MonoBehaviour
{
    CumMeterDisplay cumDisplay;

    float decayTimer = 0;

    public int decayThreshold;

    public float decayRate;

    public float scoreModifier = 0.001f;

    private float score = 0;
    void Start()
    {
        cumDisplay = FindAnyObjectByType<CumMeterDisplay>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            IncreasePoints(Random.Range(100, 500));
        }

        decayTimer += Time.deltaTime;

        if (decayTimer > 1)
        {
            DecayPoints();
        }
    }

    public void IncreasePoints(int pointsIncrease)
    {
        cumDisplay.DisplayPointsGained();

        if (pointsIncrease > decayThreshold)
        {
            decayTimer = 0;
        }

        score += pointsIncrease * scoreModifier;

        cumDisplay.SetSliderValue(score);

    }

    public void DecayPoints()
    {
        cumDisplay.DisplayPointsGained();

        score -= decayRate * Time.deltaTime;

        cumDisplay.SetSliderValue(score);

    }
}
