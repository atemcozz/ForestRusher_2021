using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralManager : MonoBehaviour
{
    public static ProceduralManager Instance;
    GameObject[] mathLocationSamples, treePrefabs;
  
    private void Awake() {
        if(Instance==null) Instance = this;
        mathLocationSamples = Resources.LoadAll<GameObject>("m_LocationSamples");
        treePrefabs = Resources.LoadAll<GameObject>("TreePrefabs");
    }
    [SerializeField] Transform player;
    [SerializeField] float generationDistance = 20f;
    [SerializeField] float destructionDistance = 20f;
    public Transform Player => player;
    public float GenerationDistance => generationDistance; 
    public float DestructionDistance => destructionDistance; 
    public GameObject GetRandomLocationSample(){
        return mathLocationSamples[Random.Range(0,mathLocationSamples.Length)];
    }
   public GameObject GetRandomTree(){
        return treePrefabs[Random.Range(0,treePrefabs.Length)];
    }
}
