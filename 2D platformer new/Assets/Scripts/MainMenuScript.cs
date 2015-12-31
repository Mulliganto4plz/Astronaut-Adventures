using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    static public float sfxVolume = 0.2f;
    [SerializeField]
    public GameObject music;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        DontDestroyOnLoad(music);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void MusicVolume(float vol)
    {
        music.GetComponent<AudioSource>().volume = vol;
    }

    public void SfxVolume(float vol)
    {
        sfxVolume = vol;
    }

}