using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void RestartGame()
    {
        // Reload the current scene to restart the game (Incomplete, add the scene that you want to reload to)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f; // resume
    }

    public void ExitGame()
    {
        // Exit the application
        Application.Quit();
        // If running in the Unity editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void LoadMainMenu()
    {
        // Load the Main Menu scene
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with the exact name of your main menu scene
    }
}
