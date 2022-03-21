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
    private PlayerController playerController;
    private SpawnManager spawnManager;
    private DataPersistantManager dataPersistantManager;
    private GameObject dataPersistantManagerGameObject;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI wavePopUpText;
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI townHPText;
    public TextMeshProUGUI playerLevelText;
    public GameObject menuCanvas;
    private bool pauseToggle;


    // Start is called before the first frame update
    void Start()
    {
        
        playerController = FindObjectOfType<PlayerController>();
        spawnManager = FindObjectOfType<SpawnManager>();
        dataPersistantManager = FindObjectOfType<DataPersistantManager>();
        dataPersistantManagerGameObject = dataPersistantManager.GetComponent<GameObject>();
        playerLevelText.text = "Lvl: " + dataPersistantManager.playerLevel;
        playerHPText.text = "HP: " + dataPersistantManager.playerHP;
        townHPText.text = "Town Resistance: " + dataPersistantManager.townHP;
        waveText.text = "Wave: " + dataPersistantManager.wave;
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
        wavePopUpText.text = "Wave " + dataPersistantManager.wave;
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
