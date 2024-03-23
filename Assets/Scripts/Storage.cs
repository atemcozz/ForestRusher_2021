using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Storage : MonoBehaviour
{
   private static Storage _instance;
    static Data data;
    public UnityAction OnMoneyChanged;
    public static Storage Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }

            // Do not modify _instance here. It will be assigned in awake
            return new GameObject("Storage Instance").AddComponent<Storage>();
        }
    }

     class Data{
        public int money = 0;
    }
    
    void Awake()
    {
        // Only one instance of SoundManager at a time!
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        LoadState();
    }
    
    public void AddMoney(){
        data.money++;
        OnMoneyChanged?.Invoke();
        SaveState();
    } 
    public void SubtractMoney(int value){
        data.money-=value;
        if(data.money<0) data.money = 0;
        SaveState();
        OnMoneyChanged?.Invoke();
    }
    public int GetMoneyAmount(){
        return data.money;
    }
    public void SaveState(){
        string storedData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SaveData", storedData);
    }
    public void LoadState(){
        if(PlayerPrefs.HasKey("SaveData")){
            data = JsonUtility.FromJson<Data>(PlayerPrefs.GetString("SaveData"));
        }
        else{
            data = new Data();
        }
    }
}
