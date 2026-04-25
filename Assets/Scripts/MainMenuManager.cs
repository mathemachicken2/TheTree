using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject optionsPanel;

    // Called by Play button
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // Called by Options button
    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    // Called by Close button
    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }
}