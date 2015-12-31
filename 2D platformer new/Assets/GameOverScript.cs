using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour {

	public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
