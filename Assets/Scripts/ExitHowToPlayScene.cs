using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitHowToPlayScene : MonoBehaviour
{
    Button ExitButton;
    void Start()
    {
        GameObject ExitButtonObject = GameObject.Find("ExitButton");

        this.ExitButton = ExitButtonObject.GetComponent<Button>();

        this.ExitButton.onClick.AddListener(OnExitButtonClicked);
    }


    void OnExitButtonClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("IntroScene");
    }
}
