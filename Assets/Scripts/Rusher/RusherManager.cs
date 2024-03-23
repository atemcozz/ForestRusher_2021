using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RusherManager : MonoBehaviour
{
    public static RusherManager Instance;
    [SerializeField] string obstacleResPath;
    [SerializeField] RusherPlayer player;
    public RusherPlayer Player =>player;
    GameObject[] obstaclePrefabs;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform destroyPoint;
  [Min(0)]  [SerializeField] float baseSpeed;
  [SerializeField] float speed;
  [SerializeField] float maxSpeed;
   [Min(0.2f)] [SerializeField] float baseSpawnRate;
   [SerializeField] int scoreStep;
   [SerializeField] float stepValue;
    public float ObstacleSpeed => speed;
    public float DestroyZ => destroyPoint.position.z;
    [SerializeField] GameObject[] sidePrefabs;
    [SerializeField] Transform[] sideSpanwers;
    public enum ObstacleType{
        Sword,Axe,Pickaxe
    }
    void Awake(){
        if(Instance == null){
            Instance = this;
        }
    }
    void Start()
    {
        obstaclePrefabs = Resources.LoadAll<GameObject>(obstacleResPath);
        speed = baseSpeed;
        StartCoroutine(SpawnObstacle());
    }
    GameObject GetRandomObstacle(){
        return obstaclePrefabs[Random.Range(0,obstaclePrefabs.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
           
    }
    public IEnumerator SpawnObstacle(){
        while(true){
            GameObject obj = GetRandomObstacle();
             RusherObstacle obstacle = Instantiate(obj,spawnPoint.position,obj.transform.rotation).GetComponent<RusherObstacle>();
             foreach(Transform spawner in sideSpanwers){
                 Instantiate(sidePrefabs[Random.Range(0,sidePrefabs.Length)],spawner.position,Quaternion.identity);
             }
            yield return new WaitForSeconds(baseSpawnRate/GetSpeedFactor());
        }
       
       // obstacle.OnDestroy += SpawnObstacle;
    }
    public float GetSpeedFactor(){
        return speed/baseSpeed;
    }
    public void UpdateSpeed(){
        if(player.Score % scoreStep == 0){
            speed = Mathf.Clamp(speed + stepValue,baseSpeed,maxSpeed);
        }
    }
    void OnEnable(){
        player.OnSuccessfulMove += UpdateSpeed;
    }
    void OnDisable(){
         player.OnSuccessfulMove -= UpdateSpeed;
    }

}
