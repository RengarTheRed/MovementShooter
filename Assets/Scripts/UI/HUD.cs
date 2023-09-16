using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //Interact & Timer text elements
    public TMP_Text _interactText;
    public TMP_Text _timerText;
    
    //Ammo UI Components
    public TMP_Text _ammoText;
    public Slider _ammoSlider;
    
    //HP UI Components
    public TMP_Text _hpText;
    public Slider _hpSlider;
    
    //UI Functions
    public Transform _pausePanel;
    
    //UI Buttons
    public Button _ButtonResume;
    public Button _ButtonRestart;
    public Button _ButtonQuit;
    public Button _ButtonPostToLeaderboard;

    [SerializeField] private Leaderboard _leaderboardScript;

    private void Start()
    {
        SetupButtonListeners();
        
        //PAUSING TO SHOW CURSOR AND ENSURE LEADERBOARD SETUP WORKS
    }

    private void Awake()
    {
        //Pause();
    }

    private void SetupButtonListeners()
    {
        _ButtonResume.onClick.AddListener(ButtonResume);
        _ButtonRestart.onClick.AddListener(ButtonRestart);
        _ButtonQuit.onClick.AddListener(ButtonQuit);
        
        _ButtonPostToLeaderboard.onClick.AddListener(ButtonPostLeaderboard);
    }

    public void SetupHUD(int maxHP, int maxAmmo)
    {
        //Ammo setup
        _ammoText.SetText(maxAmmo.ToString());
        _ammoSlider.maxValue = maxAmmo;
        _ammoSlider.value = maxAmmo;
        
        //HP setup
        _hpText.SetText(maxHP.ToString());
        _hpSlider.maxValue = maxHP;
        _hpSlider.value = maxHP;
    }
    public void UpdateInteractText(string newText)
    {
        _interactText.SetText(newText);
    }

    public void UpdateAmmo(int newAmmo)
    {
        _ammoText.SetText(newAmmo.ToString());
        _ammoSlider.value = newAmmo;
    }
    public void UpdateHP(int newHP)
    {
        _hpText.SetText(newHP.ToString());
        _hpSlider.value = newHP;
    }

    public void UpdateTimer(float newTime)
    {
        _timerText.SetText(newTime.ToString("F0"));
    }

    //UI Button Functions
    public void ButtonResume()
    {
        Pause();
    }
    public void ButtonRestart()
    {
        Pause();
        Restart();
    }
    public void ButtonQuit()
    {
        Quit();
    }
    private void ButtonPostLeaderboard()
    {
        _leaderboardScript.TryToPostScore();
    }

    //Function for pausing / resuming scene
    public void Pause()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _pausePanel.gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            _pausePanel.gameObject.SetActive(true);
        }
    }

    //Restart function, loads currently active scene
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Outright quits the application
    //Will be changed to quit to main menu instead eventually
    private void Quit()
    {
        Application.Quit();
    }
}