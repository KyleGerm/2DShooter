using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{ 
  /// <summary>
  /// Changes to the Game scene
  /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene("MainGameScene");
    }
    /// <summary>
    /// Exits the application
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
