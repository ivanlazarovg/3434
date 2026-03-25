using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    float currentTime;

    [SerializeField]TextMeshProUGUI text;
    void Start()
    {
        currentTime = 0;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        text.text = currentTime.ToString("00:00");

        if (CumPointsManager.Instance.score >= CumPointsManager.Instance.maxScore)
        {
            Win();
        }
    }

    private void Win()
    {
        PlayerPrefs.SetFloat("time", currentTime);
        SceneManager.LoadScene("WinScreen");
    }
}
