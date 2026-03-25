using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button b;
    private void Start()
    {
        b.onClick.AddListener(StartGame);
    }

    void StartGame() 
    {
        SceneManager.LoadScene("RoomExploration");
    }
}
