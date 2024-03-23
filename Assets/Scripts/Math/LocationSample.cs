using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationSample : MonoBehaviour
{
    Camera _camera;
    bool isChecked = false;
    [SerializeField] Transform playerPos;
    public Transform PlayerPos=>playerPos;
    [SerializeField] GameObject treeRoot;
    Transform player;
  
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        player = ProceduralManager.Instance.Player.transform;
   
    }

    // Update is called once per frame
    void Update()
    {
        if(treeRoot!= null){
             if(Vector3.Distance(transform.position, new Vector3(player.transform.position.x,transform.position.y,player.transform.position.z))
             > ProceduralManager.Instance.DestructionDistance){
            treeRoot.SetActive(false);
        }
        else{
            treeRoot.SetActive(true);
        }
        }
    }
    public void DestroyTrees(){
        GetComponent<Collider>().enabled = false;
        foreach(Transform tree in treeRoot.transform){
            tree.GetComponent<M_Tree>().Destroy();
        }
        
    }
}
