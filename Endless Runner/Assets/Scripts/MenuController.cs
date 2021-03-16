using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Button PauseButton;
    [SerializeField] private Button UnpauseButton;

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void PauseGame()
    {
        PauseButton.gameObject.SetActive(false);
        Time.timeScale = 0;
        UnpauseButton.gameObject.SetActive(true);
    }

    public void UnpauseGame()
    {
        UnpauseButton.gameObject.SetActive(false);
        Time.timeScale = 1;
        PauseButton.gameObject.SetActive(true);
    }
}
