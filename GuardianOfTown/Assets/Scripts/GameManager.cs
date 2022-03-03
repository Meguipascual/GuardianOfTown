using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private PlayerManager playerScript;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI townHPText;
    public TextMeshProUGUI playerLevelText;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerScript.IsDead)
        {
            gameOverText.gameObject.SetActive(true);
        }
    }
}
