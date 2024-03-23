using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MathsUI : MonoBehaviour
{
    [SerializeField] GameObject examplePanel;
    [SerializeField] M_Player player;
    [SerializeField] Text scoreText;
  [SerializeField] CommonUI commonUI;
  [SerializeField] Image timerBar;


    void Start(){
        //commonUI = GetComponent<CommonUI>();
    }
    void Update(){
        timerBar.fillAmount = player.Timer/player.TimerStartTime;
    }
    void OnEnable(){
       player.OnSuccessfulMove += UpdateScoreUI;
       player.OnLose += HideExamplesUI;
       commonUI.OnPause+=HideExamplesUI;
       commonUI.OnUnpause+=ShowExamplesUI;


   }
   void OnDisable(){

        player.OnSuccessfulMove -= UpdateScoreUI;
        player.OnLose-=HideExamplesUI;
         commonUI.OnPause-=HideExamplesUI;
         commonUI.OnUnpause-=ShowExamplesUI;
    }
    void UpdateScoreUI(int value){
        scoreText.text = value.ToString();
    }
    void HideExamplesUI(){
        examplePanel.SetActive(false);
    }
    void ShowExamplesUI(){
        examplePanel.SetActive(true);
    }
}
