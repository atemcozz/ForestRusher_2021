using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class CommonUI : MonoBehaviour
{
    /*
    public static GeneralUI Instance;
    void Awake(){
        if(Instance == null){
            Instance = this;
        }
    }*/
    [SerializeField] GameObject pauseMenu, winMenu,loseMenu,pauseButton;
    [SerializeField] Text coinsText;
    public UnityAction OnPause,OnUnpause;
    public void ShowPauseMenu(){
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
        OnPause?.Invoke();
    }
    public void ShowLoseMenu(int earnedCoins){
        Time.timeScale = 0f;
        loseMenu.SetActive(true);
        pauseButton.SetActive(false);
         coinsText.text = "+" + earnedCoins.ToString();
    }
    public void ShowWinMenu(int earnedCoins){
        Time.timeScale = 0f;
        winMenu.SetActive(true);
        pauseButton.SetActive(false);
        coinsText.text = "+" + earnedCoins.ToString();
    }
    public void ClosePauseMenu(){
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        OnUnpause?.Invoke();
    }
    public void Exit(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void Restart(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
