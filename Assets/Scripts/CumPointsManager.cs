
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CumPointsManager : MonoBehaviour
{
    CumMeterDisplay cumDisplay;

    float decayTimer = 0;

    public int decayThreshold;

    public float decayRate;
    public float decayDelay;

    public float scoreModifier = 0.001f;

    public float score = 0;
    public float maxScore = 0;

    public bool walshed;
    public float walshDecayMult;

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

    void Update()
    {
        decayTimer += Time.deltaTime;

        if (decayTimer > decayDelay)
        {
            DecayPoints();
        }
    }

    public void IncreasePoints(float pointsIncrease)
    {
        if (PowerUpManager.Instance.isLubed)
        {
            pointsIncrease *= 2;
        }

        cumDisplay.DisplayPointsGained(Mathf.FloorToInt(pointsIncrease));
        score += pointsIncrease;

        if (pointsIncrease > decayThreshold)
        {
            decayTimer = 0;
        }

        cumDisplay.SetSliderValue(score / maxScore);

    }

    public void DecayPoints()
    {
        if (walshed)
        {
            score -= decayRate * walshDecayMult * Time.deltaTime;

        }
        else 
        {
            score -= decayRate * Time.deltaTime;
        }

        if (score < 0)
        {
            score = 0;
        }
        cumDisplay.SetSliderValueInstant(score / maxScore);

    }
}
