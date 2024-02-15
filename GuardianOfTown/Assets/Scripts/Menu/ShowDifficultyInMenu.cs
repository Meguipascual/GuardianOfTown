using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowDifficultyInMenu : MonoBehaviour
{
    private TextMeshProUGUI _text;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        var difficulty = GameSettings.Instance.GetDifficulty();

        if (difficulty == 0)
        {
            _text.text = "Easy";
        }else if (difficulty == 1)
        {
            _text.text = "Normal";
        }else if(difficulty == 2)
        {
            _text.text = "Hard (H)";
        }
        else
        {
            _text.text = "Error";
        }
    }
}
