using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class RankingManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _peopleText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TextMeshProUGUI _devText;
    [SerializeField] private TextMeshProUGUI _devText1;
    private int _rankingCount;
    private int _rankingCountMax;
    private List<RankingData> _rankings;
    private RankingData _currentRanking;

    // Start is called before the first frame update
    void Start()
    {
        //_devText.text = $"Exp Total: {DataPersistantManager.Instance.SavedTotalPlayerExp}";
        _devText1.text = $"{SceneManager.GetActiveScene().name}";
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
    }

    public void SavePlayerInRank()
    {
        var playerScore = _rankingCount;//here we need to put our Score calculated.
        var playerName = _inputField.text;
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

    private void LoadRanking()
    {
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
        PlayerPrefs.DeleteAll();
    }

    public void FinishButton()
    {
        SceneManager.LoadScene(Tags.Menu);
    }
}
