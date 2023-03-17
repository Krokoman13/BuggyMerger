using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Stopwatch : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI amountLeft;

    [SerializeField] float currentSeconds;

    public int getSeconds
    {
        get { return (int)(currentSeconds % 60); }
    }
    public int getMinutes
    {
        get { return ((int)(currentSeconds - getSeconds)) / 60; }
    }
    public int getMiliseconds
    {
        get
        {
            float rest = currentSeconds - getSeconds - (getMinutes * 60);
            return Mathf.RoundToInt(rest * 100);
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentSeconds += Time.deltaTime;

        amountLeft.color = Color.white;

        TimeSpan timeSpan = TimeSpan.FromSeconds(currentSeconds);

        if (currentSeconds > 60f)
        {

            amountLeft.text = timeSpan.ToString(@"mm\:ss"); ;
            return;
        }

        amountLeft.text = timeSpan.ToString(@"ss\:ff");
    }

    public void StartStopwatch()
    {
        enabled = true;
        currentSeconds = 0;
    }

    public void PauseStopwatch()
    {
        enabled = false;
    }

    public void ResumeStopwatch()
    {
        enabled = true;
    }

    public void DisplayTime(TextMeshProUGUI tmpGUI)
    {
        currentSeconds += Time.deltaTime;

        amountLeft.color = Color.white;

        TimeSpan timeSpan = TimeSpan.FromSeconds(currentSeconds);
        tmpGUI.text = amountLeft.text = timeSpan.ToString(@"mm\:ss\:ff");
    }
}
