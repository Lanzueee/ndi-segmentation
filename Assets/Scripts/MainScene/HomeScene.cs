using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScene : MonoBehaviour
{
    public GameObject stepCounter;
    private void Start()
    {
        stepCounter.SetActive(false);

    }
    public void OpenStepCounter() 
    {
        stepCounter.SetActive(true);

    }
}
