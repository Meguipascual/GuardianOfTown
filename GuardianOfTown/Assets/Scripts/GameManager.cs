using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager SharedInstance;
    private PlayerController playerController;
    private SpawnManager spawnManager;
    private GameObject dataPersistantManagerGameObject;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI wavePopUpText;
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI townHPText;
    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI enemiesLeftText;
    public TextMeshProUGUI projectileText;
    public GameObject menuCanvas;
    private bool pauseToggle;


    // Start is called before the first frame update
    void Start()
    {
        SharedInstance = this;
        playerController = FindObjectOfType<PlayerController>();
        spawnManager = FindObjectOfType<SpawnManager>();
        dataPersistantManagerGameObject = DataPersistantManager.Instance.GetComponent<GameObject>();
        playerLevelText.text = "Lvl: " + DataPersistantManager.Instance.SavedPlayerLevel;
        playerHPText.text = "HP: " + DataPersistantManager.Instance.SavedPlayerHP;
        townHPText.text = "Town Resistance: " + DataPersistantManager.Instance.SavedTownHP;
        waveText.text = "Wave: " + DataPersistantManager.Instance.Wave;
        StartCoroutine(ShowWaveText());
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.IsDead)
        {
            gameOverText.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    IEnumerator ShowWaveText()
    {
        wavePopUpText.text = "Wave " + DataPersistantManager.Instance.Wave;
        wavePopUpText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        wavePopUpText.gameObject.SetActive(false);
        spawnManager.ControlWavesSpawn();
    }

    private void ToggleMenu()
    {
        if (pauseToggle)
        {
            Time.timeScale = 1;
            menuCanvas.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            menuCanvas.SetActive(true);
        }
        pauseToggle = !pauseToggle;
    }

    public void ReturnToMainMenuButton()
    {
        Time.timeScale = 1;
        Destroy(dataPersistantManagerGameObject);
        SceneManager.LoadScene(0);
    }
    
    public void ResumeButton()
    {
        Time.timeScale = 1;
        menuCanvas.SetActive(false);
        pauseToggle = !pauseToggle;
    }

    public void ExitButton()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit(); // original code to quit Unity player
        #endif
    }

}
