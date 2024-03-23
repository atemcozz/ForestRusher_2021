using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class MineManager : MonoBehaviour
{
    public static MineManager Instance;
    void Awake(){
        if(Instance == null){
            Instance = this;
        }
    }
    Camera _camera;
    [SerializeField] LayerMask cellLayers;
    public UnityAction OnLose;
    public UnityAction OnHintActivated;
    public UnityAction OnWin;
    bool isLose = false;
    int minesCount;
    int safeCellsCount;
    int cellsOpened;
    [SerializeField]  Vector2Int fieldSize;
    public Vector2Int FieldSize => fieldSize;
    [SerializeField] GameObject cellPrefab;
   [Range(0,1)] [SerializeField]  float mineSpawnChance;
   public float MineSpawnChance => mineSpawnChance;
    [SerializeField] float distanceBetweenCells;
    bool firstCellOpened;
     M_Cell[,] cells;
     bool[,] visitedCells;
     [SerializeField] int safeZoneStep;
     int fingerID = 0;
    [SerializeField] float holdTime = 1f;
    float timeSinceTouch;
    bool isCellHeld = false;
    bool hintMode = false;
    Vector3 maxCameraPosition;
    [SerializeField] CameraDragger dragger;
       void Start()
    {
        /*
       System.Random rnd = new System.Random();
       int seed = rnd.Next(0,100000);
       Random.InitState(seed);
       print(seed);
       */
         if(PlayerPrefs.HasKey("fieldXsize")){
            fieldSize = new Vector2Int(PlayerPrefs.GetInt("fieldXsize"),PlayerPrefs.GetInt("fieldYsize"));
            mineSpawnChance = PlayerPrefs.GetFloat("mineSpawnChance");
        }
        cells = new M_Cell[fieldSize.x,fieldSize.y];
        visitedCells = new bool[fieldSize.x,fieldSize.y];
        _camera = Camera.main;
       
        GenerateField();
        dragger.SetMaxCameraPosition(new Vector3(fieldSize.x*distanceBetweenCells,0f,fieldSize.y*distanceBetweenCells ));
        
         #if UNITY_EDITOR
            fingerID = -1;
        #endif
        timeSinceTouch = 0f;
    }
    void GenerateField(){
        Vector3 currentPosition = Vector3.zero;
        for(int h = 0; h < fieldSize.y;h++){
          
            for(int w = 0; w < fieldSize.x;w++){
                M_Cell cell =  Instantiate(cellPrefab,currentPosition,Quaternion.identity).GetComponent<M_Cell>();
                cell.SetPositon(new Vector2Int(w,h));
                    cells[w,h] = cell;
                  currentPosition += Vector3.right * distanceBetweenCells;
            }
            currentPosition = new Vector3(0f,0f,currentPosition.z);
            currentPosition += Vector3.forward * distanceBetweenCells;
        }
    }
    public void EnableHint(){
        hintMode = true;
    }
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.B)){
           StartBot();
        }
        if(Input.GetKeyDown(KeyCode.R)){
            PlayerPrefs.DeleteAll();
           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
           // RebuildField();
        }
        if(Input.GetKeyDown(KeyCode.H)){
         //  OpenEmptyCells(new Vector2Int(0,0));
        GetUnopenedNearbyCells(GetNearbyCells(1,1,1));
        }
        if(Input.GetKeyDown(KeyCode.G)){
           
           for(int h = 0; h<fieldSize.y; h++){
               for(int w = 0; w<fieldSize.y; w++){
                OpenCell(w,h);
           }
           }
           UncoverMines();
           }
        #if UNITY_EDITOR || UNITY_STANDALONE_WIN
    
        if(Input.GetMouseButton(0) && !isCellHeld  && !hintMode && !IsPointerOverUIObject() && !isLose){
            CheckForPointerEnter(_camera.ScreenPointToRay(Input.mousePosition));
        }
        if(Input.GetMouseButtonUp(0) && !isLose && !IsPointerOverUIObject()){ 
            CheckForPointerExit(_camera.ScreenPointToRay(Input.mousePosition));
        }
        
        #endif
        #if UNITY_ANDROID && !UNITY_EDITOR
            if(Input.touchCount == 1 &&  Input.GetTouch(0).phase == TouchPhase.Moved){
                isCellHeld = true;
                timeSinceTouch = 0;
            }
        if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Stationary && !isCellHeld && !isLose && !hintMode && !IsPointerOverUIObject() ){
                CheckForPointerEnter(_camera.ScreenPointToRay(Input.GetTouch(0).position));
             }
            if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended && !isLose && !IsPointerOverUIObject()){
                CheckForPointerExit(_camera.ScreenPointToRay(Input.GetTouch(0).position));
             }
        #endif
    }
    void CheckForPointerEnter(Ray ray){
        
                 if (Physics.Raycast(ray, out RaycastHit hit,cellLayers)){M_Cell cell = hit.collider.GetComponent<M_Cell>();
                        timeSinceTouch+=Time.deltaTime;
                 
                 
                     // OpenCell(hit.collider.GetComponent<M_Cell>().GetPositon().x,hit.collider.GetComponent<M_Cell>().GetPositon().y);
                    if(timeSinceTouch>holdTime){

                        if(!cell.IsGenerated){
                        if(!cell.IsFlagged) {
                        cell.SetFlag();
                        //Handheld.Vibrate();
                       timeSinceTouch = 0;
                        isCellHeld = true;
                        
                        } 
                        else {
                        cell.DisableFlag();
                       // Handheld.Vibrate();
                       timeSinceTouch = 0;
                        isCellHeld = true;
                        }
                    }
                     
                 }
              

            }
    }
    void CheckForPointerExit(Ray ray){
            if (Physics.Raycast(ray, out RaycastHit hit,cellLayers)){
                M_Cell cell = hit.collider.GetComponent<M_Cell>();
                if(hintMode){
                    if(cell.IsMine)cell.SetFlag();
                    else OpenCell(cell.GetPositon().x,cell.GetPositon().y);
                    OnHintActivated?.Invoke();
                    hintMode = false;
                }
                else if(timeSinceTouch<=holdTime){
                    if(!isCellHeld){
                    OpenCell(hit.collider.GetComponent<M_Cell>().GetPositon().x,hit.collider.GetComponent<M_Cell>().GetPositon().y);
                    }
                    isCellHeld = false;
                    timeSinceTouch = 0;
                }
                else if(timeSinceTouch>=holdTime){
                    isCellHeld = false;
                    timeSinceTouch = 0;
                }
                

            }
   
    }
    public void UncoverMines(){
        OnLose?.Invoke();
    }
    public bool MineAt(int x, int y){
        if(cells[x,y].IsMine){
            return true;
        }
        else {
            return false;
        }
    }
    
    public  void OpenCell(int x,int y){
        if(x >=0 && y>=0 && x<fieldSize.x && y<fieldSize.y && !cells[x,y].IsGenerated){

             if(!firstCellOpened){
                    
                    for(int w = -safeZoneStep; w<=safeZoneStep; w++) for(int h = -safeZoneStep; h<=safeZoneStep; h++){
                        if(x+w>=0 && y+h>=0 && x+w < fieldSize.x && y+h < fieldSize.y ){
                            cells[x+w,y+h].Randomize(true);
                        }
                        
                    }
                    foreach(M_Cell cell in cells){
                        cell.Randomize(false);     
                        }         
                    print("Mines: " + minesCount);
                    safeCellsCount = fieldSize.x * fieldSize.y - minesCount;
                    print("Safe cells: " + safeCellsCount);
                     firstCellOpened = true;
                }
                if(!cells[x,y].IsFlagged){
                   // cells[x,y].DisableFlag();

                    cells[x,y].LoadCell(out bool _isMine);
                     if(_isMine){
                        isLose = true;
                        UncoverMines();
                     // Handheld.Vibrate();
                     
                      
                    }
                    else{
                        cellsOpened++;
                        if(cellsOpened==safeCellsCount){
                            OnWin?.Invoke();
                           // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                        }
                    }
                    if( cells[x,y].CalculateAdjacents(x,y) == 0){
                        RevealEmpty(x,y);
                    }
                        
                    }

                
            
        }
               
                     
    }
    public M_Cell[] GetUnopenedNearbyCells(List<M_Cell> nearbyCells){
        List<M_Cell> unCells = new List<M_Cell>();
        for(int i = 0; i<nearbyCells.Count;i++){
            if(!nearbyCells[i].IsGenerated){
                unCells.Add(nearbyCells[i]);
            }
        }
         for(int p = 0; p<unCells.Count;p++){
            print(unCells[p].GetPositon());
        }
        return unCells.ToArray();
    }
    public List<M_Cell> GetNearbyCells(int x, int y, int step){
        List<M_Cell> nearbyCells = new List<M_Cell>();
        
    for(int w = -step; w<=step; w++) for(int h = -step; h<=step; h++){
                        if(x+w>=0 && y+h>=0 && x+w < fieldSize.x && y+h < fieldSize.y){
                            if(!(x+w==x && y+h==y)){  
                                nearbyCells.Add(cells[x+w,y+h]);
                            }
                           
                          
                        }
                        
                    }
                   
                    return nearbyCells;
    }
    public void RevealEmpty(int x,int y){
       OpenCell(x+1,y);
       OpenCell(x-1,y);
       OpenCell(x,y+1);
       OpenCell(x,y-1);
       OpenCell(x+1,y+1);
       OpenCell(x-1,y-1);
       OpenCell(x-1,y+1);
       OpenCell(x+1,y-1);

    }
    public M_Cell GetCell(int x, int y){
        return cells[x,y];
    }
    public void IncreaseMinesCount(){
        minesCount++;
    }


    public void StartBot(){
        bool[,] flagOnPoint = new bool[fieldSize.x,fieldSize.y];
       
        int minesFound = 0; /*
        int rndX = Random.Range(0,fieldSize.x);
        int rndY = Random.Range(0,fieldSize.y); 
        OpenCell(rndX,rndY); */
       
           for(int w =0; w<fieldSize.x; w++) for(int h =0; h<fieldSize.y; h++){
            if(cells[w,h].CalculateAdjacents(w,h) == GetUnopenedNearbyCells(GetNearbyCells(w,h,1)).Length){
                foreach(M_Cell cell in GetUnopenedNearbyCells(GetNearbyCells(w,h,1))){
                    cell.SetFlag();
                   flagOnPoint[cell.GetPositon().x, cell.GetPositon().y] = true;
                  minesFound++;
                }
            }
            else{
                int flagsNearby = 0;
                foreach(M_Cell cell in GetUnopenedNearbyCells(GetNearbyCells(w,h,1))){
                   if(flagOnPoint[cell.GetPositon().x, cell.GetPositon().y]){
                       flagsNearby++;
                   }
                  
                }
                if(flagsNearby == cells[w,h].CalculateAdjacents(w,h)){
                    cells[w,h].LoadCell(out bool p);
                }
            }
            
        }
        
        
        
    }
    void RebuildField(){
        foreach(M_Cell cell in cells){
            Destroy(cell.gameObject);
            
        }
        cells = new M_Cell[fieldSize.x,fieldSize.y];
        firstCellOpened = false;
        minesCount = 0;
        GenerateField();
    }
    private bool IsPointerOverUIObject() {

    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
    eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
 
    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
    return results.Count > 0;
}
}
