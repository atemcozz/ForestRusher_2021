using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class M_Cell : MonoBehaviour
{
    [SerializeField] GameObject[] numberObjects;
    [SerializeField] GameObject mineObject, flagObject;
    [SerializeField] Transform treeRoot;
   [SerializeField] Vector2Int cellPosition;

    bool isMine;
    public bool IsMine => isMine;
    bool isGenerated =false;
    bool isFlagged;
    public bool IsFlagged => isFlagged;
    bool isRandomised = false;
    public bool IsGenerated => isGenerated;
    public bool IsRandomised => isRandomised;
    
    MineManager manager;
    Camera _camera;
     void OnEnable(){
     MineManager.Instance.OnLose += UncoverMine;
   }
   void OnDisable(){
        MineManager.Instance.OnLose -= UncoverMine;
   }
   void UncoverMine(){
       if(isMine){
           LoadCell(out bool i);
           DisableFlag();
       }
   }
    void Start()
    {
       
        _camera = Camera.main;
        manager = MineManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)){
            //MineManager.Instance.OpenEmptyCells(cellPosition);
            
           // LoadCell(Random.Range(0,9));
        }
        
         

    }
    public void Randomize(bool isFirst){
        if(!isRandomised){
            if(isFirst) isMine = false;
            else isMine = Random.value < MineManager.Instance.MineSpawnChance;

            if(isMine) manager.IncreaseMinesCount();
            isRandomised = true;
        }
        
    }
    public void LoadCell(out bool _isMine){
       
       
        foreach(Transform tree in treeRoot){
           tree.GetComponent<M_Tree>().Destroy();
        }
        if(isMine){
            mineObject.SetActive(true);
            _isMine = true;
        }
        else{
            numberObjects[CalculateAdjacents(cellPosition.x,cellPosition.y)].SetActive(true);
            _isMine = false;
           
        }
        DisableFlag();
        isGenerated = true; 
    }
   public void SetFlag(){
       flagObject.SetActive(true);
       isFlagged = true;
   }
   public void DisableFlag(){
      
        flagObject.SetActive(false);
        isFlagged = false;
   }
    public int CalculateAdjacents(int x,int y){
 
         int count = 0;
    for(int h = -1; h<=1;h++){    
        for(int w = -1; w<=1;w++){
            if(x+w>=0 && y+h>=0 && x+w<MineManager.Instance.FieldSize.x && y+h<MineManager.Instance.FieldSize.y){
                if(MineManager.Instance.MineAt(x+w,y+h)) ++count;
            }
        
        }
    }

    return count;
    }
    
    public void SetPositon(Vector2Int pos){
        cellPosition = pos;
    }
    public Vector2Int GetPositon(){
        return cellPosition;
    }
}
