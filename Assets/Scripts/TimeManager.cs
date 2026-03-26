using System;
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
        text.text = TimeSpan.FromSeconds(currentTime).ToString(@"mm\:ss\:ff");

        if (CumPointsManager.Instance.score >= CumPointsManager.Instance.maxScore)
        {
            Win();
        }

        text.gameObject.GetComponent<RectTransform>().localScale = new Vector3((Mathf.Abs(Mathf.Sin(Time.time) ) + 0.5f) * 2, 1, 1);
    }

    private void Win()
    {
        PlayerPrefs.SetFloat("time", currentTime);
        SceneManager.LoadScene("WinScreen");
    }
}
