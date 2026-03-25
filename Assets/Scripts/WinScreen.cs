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

        currentTime.text = $"Current time:{t.ToString("00:00")}";

        if (lastBestTime == 0)
        {
            bestTime.text = "First Game Record!";
            PlayerPrefs.SetFloat("bestTime", t);
        }
        else if (t < lastBestTime)
        {
            PlayerPrefs.SetFloat("bestTime", t);
            bestTime.text = $"New Record! Old Record: {lastBestTime.ToString("00:00")}";
        }
        else 
        {
            bestTime.text = $"Record: {lastBestTime.ToString("00:00")}";
        }
    }
    void StartGame()
    {
        SceneManager.LoadScene("RoomExploration");
    }
}
