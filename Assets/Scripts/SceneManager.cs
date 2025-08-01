using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    Button StartButton;
    Button HowToPlayButton;
    Button ExitButton;
    void Start()
    {
        GameObject StartButtonObject = GameObject.Find("StartButton");
        GameObject HowToPlayButtonObject = GameObject.Find("HowToPlayButton");
        GameObject ExitButtonObject = GameObject.Find("ExitButton");

        this.StartButton = StartButtonObject.GetComponent<Button>();
        this.HowToPlayButton = HowToPlayButtonObject.GetComponent<Button>();
        this.ExitButton = ExitButtonObject.GetComponent<Button>();

        this.StartButton.onClick.AddListener(OnStartButtonClicked);
        this.HowToPlayButton.onClick.AddListener(OnHowToPlayButtonClicked);
        this.ExitButton.onClick.AddListener(OnExitButtonClicked);
    }

    void OnStartButtonClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
    void OnHowToPlayButtonClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("HowToPlayScene");
    }
    void OnExitButtonClicked()
    {
        Debug.Log("Button Clicked");
        Application.Quit();
        Debug.Log("Exit Called");
    }
}
