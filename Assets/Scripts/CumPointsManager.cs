
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

    public float GetScoreModifier() => scoreModifier;

    private static CumPointsManager instance;

    public static CumPointsManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindAnyObjectByType<CumPointsManager>();
            }
            return instance;
        }
    }

    void Start()
    {
        cumDisplay = FindAnyObjectByType<CumMeterDisplay>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            IncreasePoints(Random.Range(10, 50));
        }

        decayTimer += Time.deltaTime;

        if (decayTimer > 1)
        {
            DecayPoints();
        }
    }

    public void IncreasePoints(int pointsIncrease)
    {
        cumDisplay.DisplayPointsGained(pointsIncrease);

        if (pointsIncrease > decayThreshold)
        {
            decayTimer = 0;
        }

        cumDisplay.SetSliderValue(pointsIncrease * scoreModifier);

    }

    public void DecayPoints()
    {

        cumDisplay.SetSliderValue(-decayRate);

    }
}
