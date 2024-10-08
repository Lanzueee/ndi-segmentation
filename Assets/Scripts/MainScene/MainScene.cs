using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    //Main Scene
    public GameObject loginPanel, signupPanel;

    //Scene Management
    public RectTransform startScene, imageTrackerScene, homeScene;
    public Camera mainCamera;

    void Start()
    {
        OpenStart();

    }
    void Update()
    {
        
    }
    public void OpenLogIn() 
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);

    }
    public void LogIn() 
    {

    }
    public void OpenSignUp() 
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(true);

    }
    public void SignUp() 
    {

    }
    public void OpenStart()
    {
        startScene.anchoredPosition = Vector2.zero;
        imageTrackerScene.anchoredPosition = new Vector2(3000, 0);
        homeScene.anchoredPosition = new Vector2(3000, 0);

    }
    public void OpenImageTracker()
    {
        startScene.anchoredPosition = new Vector2(3000, 0);
        imageTrackerScene.anchoredPosition = Vector2.zero;
        homeScene.anchoredPosition = new Vector2(3000, 0);

    }
    public void OpenHome()
    {
        startScene.anchoredPosition = new Vector2(3000, 0);
        imageTrackerScene.anchoredPosition = new Vector2(3000, 0);
        homeScene.anchoredPosition = Vector2.zero;

    }
}
