using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RusherPlayer : MonoBehaviour
{
    Animator animator;
    [SerializeField] float rayLenght;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] CommonUI ui;
    [SerializeField] float coinMultiplier = 0.2f;
    public UnityAction OnSuccessfulMove;
    public UnityAction OnLose;
    int score;
    public int Score => score;
    public bool ObstacleAvailable(out RusherObstacle obstacle)
    {
        if(Physics.Raycast(transform.position,transform.forward,out RaycastHit hit,rayLenght,obstacleLayer)){
            obstacle = hit.collider.GetComponent<RusherObstacle>();
            return true;
        }
        else {
            obstacle = null;
            return false;
        }
    }
    void Start(){
        animator = GetComponent<Animator>();
        animator.SetBool("isRunning",true);
        
    }
    void AddScore(int value = 1){
        score+=value;
    }
    public void DestroyObstacle(RusherManager.ObstacleType tool){
       //  animator.SetTrigger("Attack");
       animator.Play("Attack");
        if(ObstacleAvailable(out RusherObstacle obstacle)){
            if(tool == obstacle.type){
               obstacle.gameObject.GetComponent<ObjectFade>().Destroy();
                AddScore();
                OnSuccessfulMove?.Invoke();
            }
            else{
                 OnLose?.Invoke();
                ui.ShowLoseMenu((int)(score*coinMultiplier));
            }
        }
    }
    void OnTriggerEnter(Collider trigger){
        if(trigger.TryGetComponent(out RusherObstacle obstacle)){
           // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
           OnLose?.Invoke();
           ui.ShowLoseMenu((int)(score*coinMultiplier));
        }
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.A)) DestroyObstacle(RusherManager.ObstacleType.Sword);
        if(Input.GetKeyDown(KeyCode.S)) DestroyObstacle(RusherManager.ObstacleType.Axe);
        if(Input.GetKeyDown(KeyCode.D)) DestroyObstacle(RusherManager.ObstacleType.Pickaxe);
       
    }
}
