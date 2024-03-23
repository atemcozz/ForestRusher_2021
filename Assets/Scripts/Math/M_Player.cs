using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class M_Player : MonoBehaviour
{
    bool isMoving = false;
    [SerializeField] float movingSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float checkingRayDistance;
    [SerializeField] LayerMask sampleLayers;
    [SerializeField] CommonUI commonUI;
    [SerializeField] Transform playerMesh;
   public UnityAction OnMovingStarted;
   public UnityAction<int> OnSuccessfulMove;
   public UnityAction<List<bool>> OnMovingEnded;
   public UnityAction OnLose;
   Animator _animator;
   float timer;
   [SerializeField] bool timerEnabled = true;
   [SerializeField] float timerStartTime = 5f;
   public float Timer => timer;
   public float TimerStartTime => timerStartTime;
   bool timerIsActive = true;
   
   int score;
   [SerializeField] float coinMultiplier = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        timer = timerStartTime;
        // Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position,-Vector3.right * checkingRayDistance,Color.red);
        Debug.DrawRay(transform.position,Vector3.forward * checkingRayDistance,Color.red);
        Debug.DrawRay(transform.position,Vector3.right * checkingRayDistance,Color.red);
        if(Input.GetKeyDown(KeyCode.W)){
            StartCoroutine(Move(Vector3.forward));
        }
        if(Input.GetKeyDown(KeyCode.A)){
            StartCoroutine(Move(Vector3.left));
        }
        if(Input.GetKeyDown(KeyCode.D)){
             StartCoroutine(Move(Vector3.right));
        }
        if(timerIsActive && timerEnabled){
            UpdateTimer();
        }
       
    }
    void UpdateTimer(){ 
        if(timer<=0){
            Lose();
            timer = 0;
        } 
        else timer-=Time.deltaTime;
       
    }
    void ResetTimer(){
        timer = timerStartTime;
    }
    void CheckForTrees(){
         List<bool> treesBool = new List<bool>(){false,false,false};
       if(Physics.Raycast(transform.position,-Vector3.right, checkingRayDistance,sampleLayers)){
            treesBool[0] = true;
        }
        if(Physics.Raycast(transform.position,Vector3.forward, checkingRayDistance,sampleLayers)){
            treesBool[1] = true;
        }
        if(Physics.Raycast(transform.position,Vector3.right, checkingRayDistance,sampleLayers)){
            treesBool[2] = true;
        }
        
        OnMovingEnded?.Invoke(treesBool);
    }
    public void MovePlayer(float dir){
        switch(dir){
            case 0:
                StartCoroutine(Move(-Vector3.right));
                break;
            case 1:
                StartCoroutine(Move(Vector3.forward));
                break;
            case 2:
                StartCoroutine(Move(Vector3.right));
                break;
            default:
                throw new System.Exception("Wrong moving direction!");
                break;
        }
    }
    IEnumerator Move(Vector3 direction){
        if(!isMoving){
            isMoving = true;
            if(Physics.Raycast(transform.position,direction,out RaycastHit hit, checkingRayDistance,sampleLayers)){
                OnMovingStarted?.Invoke();
                OnSuccessfulMove?.Invoke(++score);
                _animator.Play("Attack");
                ResetTimer();
                hit.collider.GetComponent<LocationSample>().DestroyTrees();

               Vector3 samplePos =  hit.collider.GetComponent<LocationSample>().PlayerPos.position;
               Vector3 targetPos = new Vector3(samplePos.x,transform.position.y,samplePos.z);
  
                 while(Vector3.Distance(transform.position, targetPos ) >= 0.01f){
                     transform.position = Vector3.MoveTowards(transform.position,targetPos,movingSpeed * Time.deltaTime);
            
                    Quaternion toRotation = Quaternion.LookRotation(direction,Vector3.up);
                    playerMesh.rotation = Quaternion.RotateTowards(playerMesh.rotation,toRotation,rotationSpeed * Time.deltaTime);
                    //  playerMesh.rotation = toRotation;
                    yield return null;
                }
                transform.position = targetPos;
            }
        isMoving = false;
        CheckForTrees();
        }
        
    }
    public void Lose(){
        timerIsActive = false;
        OnLose?.Invoke();
        commonUI.ShowLoseMenu((int)(score*coinMultiplier));

    }
}
