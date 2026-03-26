using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentTime;
    [SerializeField] TextMeshProUGUI bestTime;

    [SerializeField] Button b;

    void Start()
    {

        Cursor.lockState = CursorLockMode.None;
        b.onClick.AddListener(StartGame);

        float t = PlayerPrefs.GetFloat("time");
        float lastBestTime = PlayerPrefs.GetFloat("bestTime");

        currentTime.text = $"Current time:{TimeSpan.FromSeconds(t).ToString(@"mm\:ss\:ff")}";

        if (lastBestTime == 0)
        {
            bestTime.text = "First Game Record!";
            PlayerPrefs.SetFloat("bestTime", t);
        }
        else if (t < lastBestTime)
        {
            PlayerPrefs.SetFloat("bestTime", t);
            bestTime.text = $"New Record! Old Record: {TimeSpan.FromSeconds(lastBestTime).ToString(@"mm\:ss\:ff")}";
        }
        else 
        {
            bestTime.text = $"Record: {TimeSpan.FromSeconds(lastBestTime).ToString(@"mm\:ss\:ff")}";
        }
    }
    void StartGame()
    {
        SceneManager.LoadScene("RoomExploration");
    }
}
