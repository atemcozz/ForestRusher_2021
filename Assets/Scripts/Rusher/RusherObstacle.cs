using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class RusherObstacle : MonoBehaviour
{
    public UnityAction OnDestroy;
    public RusherManager.ObstacleType type;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.back * RusherManager.Instance.ObstacleSpeed * Time.deltaTime;
        if(transform.position.z<RusherManager.Instance.DestroyZ){
           // OnDestroy?.Invoke();
         
            Destroy(gameObject);
        }
    }
}
