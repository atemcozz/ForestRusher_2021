using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MinesUI : MonoBehaviour
{
   [SerializeField] Button mineHintButton;
    [SerializeField] InputField widthField, heightField;
    [SerializeField] Slider mineSlider;
    [SerializeField] GameObject bottomPanel, winScreen;
    [SerializeField] GameObject losePanel;

    void Start(){

            MineManager.Instance.OnHintActivated += EnableHintButton;
             MineManager.Instance.OnWin += OpenWinMenu;
              MineManager.Instance.OnLose += ShowLoseMessage;
            if(PlayerPrefs.HasKey("fieldXsize")){
                    widthField.text = PlayerPrefs.GetInt("fieldXsize").ToString();
                heightField.text = PlayerPrefs.GetInt("fieldYsize").ToString();
                mineSlider.value = PlayerPrefs.GetFloat("mineSpawnChance");

            }
            else{
                 widthField.text ="10";
                heightField.text = "10";
                mineSlider.value = 0.1f;
            }
            

   }
   void OnDisable(){

            MineManager.Instance.OnHintActivated -= EnableHintButton;
            MineManager.Instance.OnWin -= OpenWinMenu;
            MineManager.Instance.OnLose -= ShowLoseMessage;

   }
    public void EnableHintButton(){
        mineHintButton.interactable =true;
    }
    public void DisableHintButton(){
        mineHintButton.interactable =false;
    }
    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void RestoreDefaults(){
        PlayerPrefs.DeleteAll();
        Restart();
    }
    public void ChangeFieldSettings(){
        if((widthField.text != string.Empty && int.Parse(widthField.text) >= 5) && (heightField.text != string.Empty && int.Parse(heightField.text) >= 5)){
              PlayerPrefs.SetInt("fieldXsize",int.Parse(widthField.text));
              PlayerPrefs.SetInt("fieldYsize",int.Parse(heightField.text));
              PlayerPrefs.SetFloat("mineSpawnChance",mineSlider.value);
               Restart();
        }
       
      
    }
    public void OpenWinMenu(){
        winScreen.SetActive(true);
        bottomPanel.SetActive(false);
    }
    void ShowLoseMessage(){
        losePanel.SetActive(true);
    }
    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    
}
