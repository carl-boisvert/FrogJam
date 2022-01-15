using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SummaryController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dayText;
    [SerializeField] private TextMeshProUGUI _nextDayText;
    [SerializeField] private TextMeshProUGUI _whatsNext;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _dayPhraseText;
    [SerializeField] private List<PhraseForDay> _phrases;
    [SerializeField] private List<WhatsNextPhrases> _whatsNextPhrases;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _mainMenuButton;

    private void Start()
    {
        _continueButton.onClick.AddListener(OnContinueClicked);
        _mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        
        GameEvents.OnGameStartEvent += OnGameStartEvent;
    }

    private void OnGameStartEvent()
    {
        _continueButton.gameObject.SetActive(true);
        _mainMenuButton.gameObject.SetActive(false);
    }

    private void ReturnToMainMenu()
    {
        GameEvents.OnGoBackToMenuEvent();
    }

    private void OnContinueClicked()
    {
        GameEvents.OnGameContinueEvent();
    }

    public void SetCurrentDay(int day, int score, float happiness, bool isLastDay)
    {
        _dayText.text = $"Day {day} over";
        _scoreText.text = $"Score: {score}";

        List<PhraseForDay> phrases = _phrases.FindAll(p => happiness >= p.from && happiness <= p.to);
        if (phrases.Count > 0)
        {
            _dayPhraseText.text = phrases[Random.Range(0, phrases.Count)].phrase;
        }

        if (!isLastDay)
        {
            _nextDayText.text = $"Day {day + 1}";
            string text = "";
            if (day <= _whatsNextPhrases.Count)
            {
                WhatsNextPhrases next = _whatsNextPhrases.Find(n => n.day == day + 1);
                foreach (var phrase in next.phrases)
                {
                    text += $"\n {phrase}";
                }
            }

            _whatsNext.SetText(text);
        }
        else
        {
            _nextDayText.text = "";
            _whatsNext.text = "";
            _continueButton.gameObject.SetActive(false);
            _mainMenuButton.gameObject.SetActive(true);
            
        }
    }
}

[Serializable]
public class PhraseForDay
{
    public float from;
    public float to;
    public string phrase;
}

[Serializable]
public class WhatsNextPhrases
{
    public int day;
    public List<String> phrases;
}
