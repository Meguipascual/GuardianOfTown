using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class RankingManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _peopleText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TMP_InputField _inputField;
    private int _rankingCount;
    private int _rankingCountMax;
    private Dictionary<string, int> _rankings;

    // Start is called before the first frame update
    void Start()
    {
        _rankingCountMax = 10;
        _rankings = new Dictionary<string, int>(); 
        if (PlayerPrefs.HasKey("0K"))
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
        if (_rankings != null) 
        {
            _rankings.Add(_inputField.textComponent.text, _rankingCount);
            SortRanking();
        }
        else
        {
            _rankings.Add(_inputField.textComponent.text, _rankingCount);
        }

        SaveRankingData();
    }

    private void SortRanking()
    {
        _rankings = _rankings.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
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
            _rankings.Add(PlayerPrefs.GetString($"{i.ToString()}K"), PlayerPrefs.GetInt($"{i.ToString()}V"));
        } 
    }

    public void SaveRankingData()
    {
        
        var count = 0;
        foreach (KeyValuePair<string, int> value in _rankings)
        {
            Debug.Log($"{count.ToString()}K String: {value.Key} Value: {value.Value}");
            PlayerPrefs.SetString($"{count.ToString()}K", value.Key);
            PlayerPrefs.SetInt($"{count.ToString()}V", value.Value);
            count++;
        }
        PlayerPrefs.SetInt("RankingCount", count);
    }

    public void ShowRankingData()
    {
        var peopleText = "";
        var score = "";
        var count = 0;

        foreach (KeyValuePair<string, int> values in _rankings)
        {
            count++;
            peopleText += $"{count}. {values.Key}\n";
            score += $"{values.Value}\n";
        }

        _peopleText.text = peopleText;
        _scoreText.text = score;
    }

    public void EraseSavedData()
    {
        PlayerPrefs.DeleteAll();
    }
}
