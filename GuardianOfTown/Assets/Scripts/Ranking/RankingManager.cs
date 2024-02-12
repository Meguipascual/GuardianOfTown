using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    [SerializeField] private GameObject _peopleTextPrefab;
    [SerializeField] private GameObject _scoreTextPrefab;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TextMeshProUGUI _devText;
    [SerializeField] private GameObject _rankInputPanel;
    [SerializeField] private GameObject _rankDataPanel;
    [SerializeField] private GameObject _namesColumnText;
    [SerializeField] private GameObject _scoresColumnText;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _finishButton;
    private DataPersistantManager _persistantManager;
    private int _currentScore;
    private int _rankingCount;
    private int _rankingCountMax;
    private List<RankingData> _rankings;
    private RankingData _currentRanking;
    private bool _isCurrentRank; 

    // Start is called before the first frame update
    void Start()
    {
        _rankingCountMax = 10;
        _rankings = new List<RankingData>();

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName(Tags.Menu))
        {
            LoadRanking();
            ShowRankingData();
            return;
        }

        if (PlayerPrefs.HasKey("0Name"))
        {
            if (DataPersistantManager.Instance != null)
            {
                _persistantManager = DataPersistantManager.Instance;
                ChangeAndShowDevText($"Exp Total: {_persistantManager.SavedTotalPlayerExp}");
                CalculateScore();
            }
            else 
            { 
                _currentScore = 0;
            }

            LoadRanking();
            ShowRankingData();
            Debug.Log("That key Exists");
            
            if (GameSettings.Instance.IsEasyModeActive || (_currentScore <= _rankings[_rankingCount-1].Score && _rankingCount == _rankingCountMax))
            {
                _rankInputPanel.SetActive(false);
                _rankDataPanel.SetActive(true);
                return;
            }
        }
        else
        {
            Debug.Log("That key Doesn't Exists");
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

        if (GameSettings.Instance.IsEasyModeActive) 
        { 
            _currentScore = 0; 
            return; 
        } 
        else if(GameSettings.Instance.IsNormalModeActive)
        {
            _currentScore = (townPoints * (expPoints + lpPoints + hpPoints + defPoints + criDMGPoints + criRatePoints + speedPoints + atkPoints)) / 10;
        }
        else if (GameSettings.Instance.IsHardModeActive)
        {
            _currentScore = (townPoints * (expPoints + lpPoints + hpPoints + defPoints + criDMGPoints + criRatePoints + speedPoints + atkPoints)) / 10;
            _currentScore *= 2;
        }
        else
        {
            Debug.Log($"Something Gone wrong, none difficulty activated");
        }
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
        if(PlayerPrefs.GetInt("RankingCount") < _rankingCountMax)
        {
            _rankingCount = PlayerPrefs.GetInt("RankingCount");
            Debug.Log($"RankingSize: {_rankingCount}");
        }
        else
        {
            _rankingCount = _rankingCountMax;
            Debug.Log($"RankingSize: {_rankingCount}");
        }
        
        for (int i = 0; i < _rankingCount; i++)
        {
            var name = PlayerPrefs.GetString($"{i}Name");
            var score = PlayerPrefs.GetInt($"{i}Score");
            RankingData rankingData = new RankingData(name,score);
            _rankings.Add(rankingData);
        } 
    }

    public void SaveRankingData()
    {
        
        var count = 0;
        foreach (var value in _rankings)
        {
            Debug.Log($"{count} Name String: {value.Name} Value: {value.Score}");
            PlayerPrefs.SetString($"{count}Name", value.Name);
            PlayerPrefs.SetInt($"{count}Score", value.Score);
            count++;
        }
        PlayerPrefs.SetInt("RankingCount", count);
    }

    public void ShowRankingData()
    {
        ClearPrefabs();
        var count = 0;
        _isCurrentRank = false;

        foreach (var value in _rankings)
        {
            if (count >= 10) 
            {
                PlayerPrefs.DeleteKey($"{count}Name");
                PlayerPrefs.DeleteKey($"{count}Score");
                count++;
                continue; 
            }

            if(_currentRanking.Name != null && _currentRanking.Name != "")
            {
                if (value.Equals(_currentRanking) && !_isCurrentRank)
                {
                    count++;
                    _isCurrentRank = true;
                    _peopleTextPrefab.GetComponent<TextMeshProUGUI>().text = $"{count}. {value.Name}\n";
                    _scoreTextPrefab.GetComponent<TextMeshProUGUI>().text = $"{value.Score}\n";
                    var name = Instantiate(_peopleTextPrefab, _namesColumnText.transform);
                    var score = Instantiate(_scoreTextPrefab, _scoresColumnText.transform);
                    name.GetComponent<TextMeshProUGUI>().color = Color.yellow;
                    score.GetComponent<TextMeshProUGUI>().color = Color.yellow;
                }
                else
                {
                    count++;
                    _peopleTextPrefab.GetComponent<TextMeshProUGUI>().text = $"{count}. {value.Name}\n";
                    _scoreTextPrefab.GetComponent<TextMeshProUGUI>().text = $"{value.Score}\n";
                    Instantiate(_peopleTextPrefab, _namesColumnText.transform);
                    Instantiate(_scoreTextPrefab, _scoresColumnText.transform);
                }
            }
            else
            {
                count++;
                _peopleTextPrefab.GetComponent<TextMeshProUGUI>().text = $"{count}. {value.Name}\n";
                _scoreTextPrefab.GetComponent<TextMeshProUGUI>().text = $"{value.Score}\n";
                Instantiate(_peopleTextPrefab, _namesColumnText.transform);
                Instantiate(_scoreTextPrefab, _scoresColumnText.transform);
            }
        }
    }

    private void  ClearPrefabs()
    {
        foreach (var prefab in _namesColumnText.GetComponentsInChildren<TextMeshProUGUI>())
        {
            Destroy(prefab.gameObject);
        }

        foreach (var prefab in _scoresColumnText.GetComponentsInChildren<TextMeshProUGUI>())
        {
            Destroy(prefab.gameObject);
        }
    }

    public void EraseSavedData()
    {
        ClearPrefabs();
        for (int i = 0; i < PlayerPrefs.GetInt("RankingCount"); i++) 
        {
            PlayerPrefs.DeleteKey($"{i}Name");
            PlayerPrefs.DeleteKey($"{i}Score");
        }
        PlayerPrefs.SetInt("RankingCount", 0);
        //PlayerPrefs.DeleteAll(); Only if you need to clean all the stored options and registers
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
        yield return new WaitForSeconds(5);
        _devText.gameObject.SetActive(false);
    }
}
