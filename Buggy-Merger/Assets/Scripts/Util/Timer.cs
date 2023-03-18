using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI amountLeft;

    [SerializeField] float currentSeconds;

    public UnityEvent onTimerOver = null;

    int getSeconds 
    {
        get { return (int) (currentSeconds % 60);  }
    }
    int getMinutes
    {
        get { return ((int)(currentSeconds - getSeconds))/60; }
    }
    int getMiliseconds
    { 
        get { 
            float rest = currentSeconds - getSeconds - (getMinutes * 60);
            return Mathf.RoundToInt(rest * 100);
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentSeconds -= Time.deltaTime;

        if (currentSeconds < 0)
        {
            FinishTimer();
        }

        TimeSpan timeSpan = TimeSpan.FromSeconds(currentSeconds);

        if (currentSeconds > 60f)
        {
            amountLeft.color = Color.white;
            amountLeft.text = timeSpan.ToString(@"mm\:ss"); ;
            return;
        }

        amountLeft.text = timeSpan.ToString(@"ss\:ff");

        if (currentSeconds < 20f)
        {
            amountLeft.color = Color.red;
        }

    }

    public void FinishTimer()
    {
        currentSeconds = 0;
        onTimerOver?.Invoke();
        enabled = false;
    }

    public void StartTimer(float seconds)
    { 
        enabled = true;
        currentSeconds = seconds;
    }

    public void PauseTimer()
    {
        enabled = false;
    }

    public void ResumeTimer()
    {
        enabled = true;
    }
}
