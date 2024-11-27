using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public void SinglePlayer()
    {
        HealthDataStore.resetAll();
        SceneManager.LoadScene("SinglePlayerView");
    }

    public void MultiPlayer()
    {
        #if UNITY_EDITOR
              EditorUtility.DisplayDialog("Sorry!", "This part of the game is not yet implemented.", "OK");
        #endif

    }

    public void Restart()
    {
        HealthDataStore.resetAll();
        SceneManager.LoadScene("SinglePlayerView");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
