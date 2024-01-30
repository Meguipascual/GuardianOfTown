using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _peopleText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TextMeshProUGUI _devText;
    [SerializeField] private GameObject _rankInputPanel;
    [SerializeField] private GameObject _rankDataPanel;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _finishButton;
    private DataPersistantManager _persistantManager;
    private int _currentScore;
    private int _rankingCount;
    private int _rankingCountMax;
    private List<RankingData> _rankings;
    private RankingData _currentRanking;

    // Start is called before the first frame update
    void Start()
    {
        _rankingCountMax = 10;
        _rankings = new List<RankingData>();
        
        if (PlayerPrefs.HasKey("0Name"))
        {
            Debug.Log("That key Exists");
            LoadRanking();
        }
        else
        {
            Debug.Log("That key Doesn't Exists");
        }

        if (DataPersistantManager.Instance != null)
        {
            ChangeAndShowDevText($"Exp Total: {DataPersistantManager.Instance.SavedTotalPlayerExp}");
            _persistantManager = DataPersistantManager.Instance;
            CalculateScore();
        }
        else
        {
            _currentScore = 0;
        }
    }

    private void CalculateScore()
    {
        var expPoints = _persistantManager.SavedTotalPlayerExp;
        var lpPoints = _persistantManager.SavedPlayerLevelPoints * 1000;
        var hpPoints = _persistantManager.SavedPlayerHP * 100;
        var defPoints = _persistantManager.SavedPlayerDefense * 120;
        var criRatePoints = _persistantManager.SavedPlayerCriticalRate * 95;
        var criDMGPoints = _persistantManager.SavedPlayerCriticalDamage * 80;
        var speedPoints =(int) _persistantManager.SavedPlayerSpeed * 80;
        var atkPoints = _persistantManager.SavedPlayerAttack * 50;
        var townPoints = _persistantManager.SavedTownHpShields.Count;

        _currentScore = (townPoints * (expPoints + lpPoints + hpPoints + defPoints + criDMGPoints + criRatePoints + speedPoints + atkPoints)) / 10;
    }

    public void CloseWarningPanel()
    {
        if (_rankDataPanel.gameObject.activeSelf)
        {
            _finishButton.Select();
        }
        else
        {
            _confirmButton.Select();
        }
    }

    public void SavePlayerInRank()
    {
        var playerScore = _currentScore;//here we need to put our Score calculated.
        var playerName = _inputField.text;
        if(playerName.Equals(null) || playerName.Equals(""))
        {
            playerName = "Player";
        }
        _currentRanking.Name = playerName;
        _currentRanking.Score = playerScore;

        if (_rankings != null) 
        {
            _rankings.Add(_currentRanking);
            SortRanking();
        }
        else
        {
            _rankings.Add(_currentRanking);
        }

        SaveRankingData();
    }

    private void SortRanking()
    {
        _rankings = _rankings.OrderByDescending(x => x.Score).ToList();
    }

    public void LoadRanking()
    {
        _rankings.Clear();
        if(PlayerPrefs.GetInt("RankingCount") <= 10)
        {
            _rankingCount = PlayerPrefs.GetInt("RankingCount");
            Debug.Log($"RankingSize: {_rankingCount}");
        }
        else
        {
            _rankingCount = _rankingCountMax;
        }
        

        for (int i = 0; i < _rankingCount; i++)
        {
            var name = PlayerPrefs.GetString($"{i.ToString()}Name");
            var score = PlayerPrefs.GetInt($"{i.ToString()}Score");
            RankingData rankingData = new RankingData(name,score);
            _rankings.Add(rankingData);
        } 
    }

    public void SaveRankingData()
    {
        
        var count = 0;
        foreach (var value in _rankings)
        {
            Debug.Log($"{count.ToString()}Name String: {value.Name} Value: {value.Score}");
            PlayerPrefs.SetString($"{count.ToString()}Name", value.Name);
            PlayerPrefs.SetInt($"{count.ToString()}Score", value.Score);
            count++;
        }
        PlayerPrefs.SetInt("RankingCount", count);
    }

    public void ShowRankingData()
    {
        var peopleText = "";
        var score = "";
        var count = 0;

        foreach (var value in _rankings)
        {
            if (count >= 10) { continue; }
            count++;
            peopleText += $"{count}. {value.Name}\n";
            score += $"{value.Score}\n";
        }

        _peopleText.text = peopleText;
        _scoreText.text = score;
    }

    public void EraseSavedData()
    {
        for (int i = 0; i < _rankingCountMax; i++) 
        {
            PlayerPrefs.DeleteKey($"{i}Name");
            PlayerPrefs.DeleteKey($"{i}Score");
        }
        PlayerPrefs.SetInt("RankingCount", 0);
        _rankingCount = 0;
    }

    public void FinishButton()
    {
        Time.timeScale = 1;
        Destroy(PermanentPowerUpsSettings.Instance.gameObject);
        Destroy(DataPersistantManager.Instance.gameObject);
        Destroy(PermanentPowerUpManager.Instance.gameObject);
        Destroy(FindObjectOfType<DontDestroyOnLoad>());
        PermanentPowerUpsSettings.Instance.PowerUpIcons.Clear();
        SceneManager.LoadScene(Tags.Menu);
    }

    public void ChangeAndShowDevText(string text)
    {
        _devText.text = text;
        StartCoroutine(ShowDevText());
    }

    private IEnumerator ShowDevText()
    {
        yield return new WaitForSeconds(.5f);
        _devText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        _devText.gameObject.SetActive(false);
    }
}
