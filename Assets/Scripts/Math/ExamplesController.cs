using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using UnityEngine.UI;

public class ExamplesController : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject[] answerButtons;
    [SerializeField] M_Player player;
    [SerializeField] Text exampleUIText;
    [SerializeField] int leastAnswer, greatestAnswer, leastTerm, greatestTerm;
  [SerializeField] int exampleLenght;
    string[] operands = new string[4]{"+","-","*","/"};
    string example;
    float answer;
    int correctButtonID;
   void OnEnable(){
       player.OnMovingStarted += DisableUI;
       player.OnMovingEnded += UpdateButtonsState;
   }
   void OnDisable(){
        player.OnMovingStarted -= DisableUI;
       player.OnMovingEnded -= UpdateButtonsState;
   }
   public void UpdateButtonsState( List<bool> buttonsState){
       EnableUI();
       for(int i = 0; i<3;i++){
         //  print(1);
           answerButtons[i].SetActive(buttonsState[i]);
       }
       UpdateExample();
       UpdateAnswers(buttonsState);
       
   }
   public void UpdateExample(){
       example = string.Empty;
       for(int i = 1; i<=exampleLenght;i++){
            if(i<exampleLenght){
                example +=(int)Random.Range(leastTerm,greatestTerm) + operands[Random.Range(0,operands.Length)];
            }
            else {
                example +=(int)Random.Range(leastTerm,greatestTerm);
            }
            
        }
        exampleUIText.text = example;
        DataTable table = new DataTable();
        answer =  System.Convert.ToSingle(table.Compute(example, null));
        //print(table.Compute("10/5*2", null));
       // answer = System.Convert.ToSingle(ExpressionEvaluator.Evaluate<int>(example, out int value));
       if(answer>greatestAnswer || answer<leastAnswer || (answer != Mathf.RoundToInt(answer))) UpdateExample();
   }
   public void UpdateAnswers(List<bool> buttonsState){


        List<int> availableButtonsID = new List<int>();
        for(int i = 0;i<3;i++){
            answerButtons[i].GetComponent<AnswerButton>().TextUI.text = (answer - (int)Random.Range(1,20)).ToString();
            if(buttonsState[i] == true) availableButtonsID.Add(i);
        }
        correctButtonID = availableButtonsID[Random.Range(0,availableButtonsID.Count)];
        // answerButtons[correctButtonID].GetComponent<AnswerButton>().TextUI.text =  "<color=blue>" + answer.ToString() + "</color>";
        // answerButtons[correctButtonID].GetComponent<AnswerButton>().TextUI.text = "<color=green>" + answer.ToString() + "</color>";
        answerButtons[correctButtonID].GetComponent<AnswerButton>().TextUI.text = answer.ToString();
      
   }
   public void OnAnswerButtonPressed(int buttonID){
       if(buttonID == correctButtonID){
           player.MovePlayer(buttonID);
       }
       else{
           player.Lose();
       }
   }
   public void DisableUI(){
    panel.SetActive(false);
   }
   public void EnableUI(){
       panel.SetActive(true);
   }
    // Start is called before the first frame update
    void Start()
    {
       // UpdateExample();
      // uiController = GetComponent<UIController>();
       UpdateButtonsState(new List<bool>(){true,true,true});
    }

    
}
