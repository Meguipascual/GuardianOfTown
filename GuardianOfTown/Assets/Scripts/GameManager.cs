using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private PlayerController playerController;
    private SpawnManager spawnManager;
    private DataPersistantManager dataPersistantManager;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI townHPText;
    public TextMeshProUGUI playerLevelText;
    

    // Start is called before the first frame update
    void Start()
    {
        
        playerController = FindObjectOfType<PlayerController>();
        spawnManager = FindObjectOfType<SpawnManager>();
        dataPersistantManager = FindObjectOfType<DataPersistantManager>();
        playerLevelText.text = "Lvl: " + dataPersistantManager.playerLevel;
        playerHPText.text = "HP: " + dataPersistantManager.playerHP;
        townHPText.text = "Town Resistance: " + dataPersistantManager.townHP;
        waveText.text = "Wave: " + dataPersistantManager.wave;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.IsDead)
        {
            gameOverText.gameObject.SetActive(true);
        }
    }

    
}
