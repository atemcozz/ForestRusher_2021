using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu, levelSelector;
    // Start is called before the first frame update
    void Start(){
        Application.targetFrameRate = 200;
    }
   public void OpenLevelSelector()
    {
        mainMenu.SetActive(false);
        levelSelector.SetActive(true);
    }
    public void OpenMainMenu()
    {
        mainMenu.SetActive(true);
        levelSelector.SetActive(false);
    }

    public void LoadGame(int index){
        SceneManager.LoadScene(index);
    }
}
