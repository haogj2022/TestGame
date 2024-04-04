using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject userConfirmMenu;
    public GameObject deathMessage;
    public GameObject reviveButton;
    public AudioManager audioManager;
    public TMP_Text numOfGems;
    private bool canPause;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        canPause = false;
        mainMenu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            audioManager.Blip();
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            canPause = false;
        }
    }

    public void PlayGame()
    {
        audioManager.Blip();
        Time.timeScale = 1;
        canPause = true;

        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        deathMessage.SetActive(false);
    }

    public void Options()
    {
        audioManager.Blip();
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void QuitGame()
    {
        audioManager.Blip();
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        userConfirmMenu.SetActive(true);
    }

    public void CancelQuit()
    {
        audioManager.Blip();
        Time.timeScale = 1;
        canPause = true;
        userConfirmMenu.SetActive(false);
    }

    public void ConfirmQuit()
    {
        audioManager.Blip();
        Application.Quit();
    }

    public void DeathMessage()
    {
        canPause = false;

        if (GameManager.Instance.numOfHearts <= 0)
        {
            deathMessage.SetActive(true);

            if (GameManager.Instance.numOfGems <= 0)
            {
                reviveButton.SetActive(false);
            }
        }        
    }

    public void Revive()
    {
        Time.timeScale = 1;
        GameManager.Instance.numOfGems -= 1;
        GameManager.Instance.numOfHearts = 1;
    }

    public void StartOver()
    {
        SceneManager.LoadScene(0);
    }
}
