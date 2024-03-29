using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class Pattern : MonoBehaviour
{
    [SerializeField] private AudioClip[] clickFX;
    [SerializeField] private AudioClip failedFX;
    [SerializeField] private List<Button> buttons;
    [SerializeField] private GameObject blockScreen, simon;
    [SerializeField] private TextMeshProUGUI highscoreText, scoreText;
    private AudioSource audioSource;
    private List<int> pattern, tempPattern;
    int highscore = 0, score = 0;

    private void Start()
    {
        //highscore = PlayerPrefs.GetInt("HighScore");
        pattern = new List<int>();
        tempPattern = new List<int>();
        audioSource = GetComponent<AudioSource>();
        //UpdateScores();

        //blockScreen.SetActive(true);
    }

    public void StartGame()
    {
        pattern.Clear();
        ResetPattern();

        UIController.Instance.ShowGame();
        highscore = PlayerPrefs.GetInt("HighScore");
        UpdateScores();
        blockScreen.SetActive(true);
        AddNewStep();
        StartCoroutine(ShowPattern());
    }

    public void AddNewStep()
    {
        pattern.Add(UnityEngine.Random.Range(0, buttons.Count));
        tempPattern.Add(pattern[pattern.Count - 1]);
    }

    public void ResetPattern()
    {
        tempPattern.Clear();
        tempPattern.AddRange(pattern);
    }

    public void UpdateScores()
    {
        highscoreText.text = $"Highscore: [{highscore}]";
        scoreText.text = $"Score: [{score}]";
    }

    public IEnumerator ShowPattern()
    {
        yield return new WaitForSeconds(1f);
        blockScreen.SetActive(true);

        foreach (int step in pattern)
        {
            SimonClicked(step);
            UIController.Instance.SimulateButtonClick(buttons[step]);
            yield return new WaitForSeconds(1f);
            UIController.Instance.SimulateButtonRelease(buttons[step]);
        }

        yield return new WaitForSeconds(1f);
        blockScreen.SetActive(false);
    }

    public void SimonClicked(int index)
    {
        audioSource.clip = clickFX[index];
        audioSource.Play();
    }

    public void UserClicked(int index)
    {
        audioSource.clip = clickFX[index];
        audioSource.Play();
        CheckUserInput(index);
    }

    public void CheckUserInput(int input)
    {
        bool failed = false;

        if (input != tempPattern[0])
            failed = true;
        else
            tempPattern.RemoveAt(0);

        if (failed)
        {
            if (score > highscore)
            {
                highscore = score;
                PlayerPrefs.SetInt("HighScore", highscore);
            }

            score = 0;
            Fail();
            return;
        }

        //correct pattern
        if (tempPattern.Count <= 0)
        {
            score++;

            AddNewStep();
            StartCoroutine(ShowPattern());

            ResetPattern();
            UpdateScores();
        }
    }

    private void Fail()
    {
        blockScreen.SetActive(true);
        audioSource.clip = failedFX;
        audioSource.Play();
        simon.transform.DOShakePosition(1f, 500f, 100, 90f).SetEase(Ease.OutQuad)
            .OnComplete(()=> 
            {
                UIController.Instance.ShowGameOver();
            });
    }
}