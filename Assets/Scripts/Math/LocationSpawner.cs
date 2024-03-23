using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationSpawner : MonoBehaviour
{
    [SerializeField] LayerMask triggerLayers;
    Camera _camera;
    Transform player;
    void Start(){
        _camera = Camera.main;
         player = ProceduralManager.Instance.Player.transform;
    }
    void Update(){
        if(Vector3.Distance(transform.position, 
        new Vector3(player.transform.position.x,transform.position.y,player.transform.position.z)) 
        < ProceduralManager.Instance.GenerationDistance){
            Spawn();
        }
    }
    void Spawn(){
        if(!Physics.Raycast(transform.position,transform.forward,3f,triggerLayers)){
            Instantiate(ProceduralManager.Instance.GetRandomLocationSample(), transform.position,transform.rotation);
        }
       Destroy(gameObject);
    }
}
