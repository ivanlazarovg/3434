using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CumPointsManager : MonoBehaviour
{
    CumMeterDisplay cumDisplay;
    void Start()
    {
        cumDisplay = FindAnyObjectByType<CumMeterDisplay>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreasePoints (int pointsIncrease)
    {
        cumDisplay.DisplayPointsGained();
    }
}
