using UnityEngine;
using System;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TMP_Text))]
[RequireComponent(typeof(Score))]
public class Timer : MonoBehaviour
{
    private static readonly WaitForSeconds waitForOneSecond = new(1);

    public event Action TimerEnd;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private int duration;

    private TMP_Text text;
    private Score score;
    private int time;

    private void OnEnable()
    {
        gameManager.GameEnded += StopTimer;
    }

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        score = GetComponent<Score>();
    }

    private void Start()
    {
        time = duration;
        text.text = time.ToString();
        StartCoroutine(CountdownHandling());
    }

    private void OnDisable()
    {
        gameManager.GameEnded -= StopTimer;
    }

    private void StopTimer(bool hasWon)
    {
        StopAllCoroutines();

        if (hasWon) score.DeclareScore(time * 10);
    }

    private IEnumerator CountdownHandling()
    {
        if (time <= 0)
        {
            time = 0;
            text.text = time.ToString();
            TimerEnd?.Invoke();
        }
        else
        {
            yield return waitForOneSecond;
            time--;
            text.text = time.ToString();
            StartCoroutine(CountdownHandling());
        }
    }
}
