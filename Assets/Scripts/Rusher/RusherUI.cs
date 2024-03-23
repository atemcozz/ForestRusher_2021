using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RusherUI : MonoBehaviour
{
    [SerializeField] RusherPlayer player;
    [SerializeField] Text scoreText;
    [SerializeField] GameObject toolPanel;
    void OnEnable(){
        player.OnSuccessfulMove += UpdateScoreText;
        player.OnLose+=HideIngameUI;
    }
    void OnDisable(){
        player.OnSuccessfulMove -= UpdateScoreText;
        player.OnLose-=HideIngameUI;
    }
    public void UpdateScoreText(){
        scoreText.text = player.Score.ToString();
    }
    public void HideIngameUI(){
        toolPanel.SetActive(false);
    }
}
