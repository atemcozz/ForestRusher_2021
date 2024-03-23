using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [SerializeField] Transform treeRoot;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(ProceduralManager.Instance.GetRandomTree(),transform.position,Quaternion.Euler(0f,Random.Range(0f,360f),0f),treeRoot);
        Destroy(gameObject);
    }
}
