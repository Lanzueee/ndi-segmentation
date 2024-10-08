using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class stepCounter : MonoBehaviour
{
    public TextMeshProUGUI stepCountText, statusText, caloriesText, goalsText, startText;
    public GameObject goalPanel;
    [SerializeField] private UnityEngine.UI.Image progressBar;
    [SerializeField] private TMP_InputField goalsField;

    private float stepThreshold = 1.2f;
    private bool isStep = false;
    private int stepCount = 0;
    private bool isCounting = false;
    private const float caloriePerStep = 0.06f;
    private float calorieCount = 0;
    void Update()
    {
        if (isCounting)
        {
            if (Input.acceleration != Vector3.zero)
            {
                Vector3 acceleration = Input.acceleration;
                float magnitude = acceleration.magnitude;

                if (magnitude > stepThreshold)
                {
                    if (!isStep)
                    {
                        stepCount++;
                        float progress = (float)stepCount / (float)Convert.ToInt32(goalsText.text);
                        progressBar.fillAmount = (float)progress;
                        isStep = true;
                        UpdateStepCountText();

                        calorieCount = (float)stepCount * (float)caloriePerStep;
                        caloriesText.text = calorieCount.ToString();

                    }
                }
                else
                {
                    isStep = false;

                }
            }
        }
        if (string.IsNullOrEmpty(goalsText.text))
        {
            goalsText.text = "0";

        }
        calorieCount++;

    }
    public void OpenGoals() 
    {
        if (!goalPanel.gameObject.activeSelf)
        {
            goalPanel.gameObject.SetActive(true);

        }
        else if (goalPanel.gameObject.activeSelf) 
        {
            goalPanel.gameObject.SetActive(false);
            progressBar.fillAmount -= 1;

        }
    }
    public void StartCounting()
    {
        startText.text = "Start";
        if (isCounting == false) 
        {
            isCounting = true;
            statusText.text = "Counting";
            startText.text = "Stop";


        }
        else if (isCounting == true) 
        {
            isCounting = false;
            statusText.text = "Stopped";
            startText.text = "Start";

        }
    }
    public void ResetCount()
    {
        isCounting = false;
        statusText.text = "Waiting";

        stepCount = 0;
        stepCountText.text = stepCount.ToString();
        goalsText.text = "--";
        progressBar.fillAmount = 0;

    }
    void UpdateStepCountText()
    {
        stepCountText.text = stepCount.ToString();

    }
    public void setGoals() 
    {
        goalsText.text = goalsField.text;
        OpenGoals();

    }
}
