using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Tree : MonoBehaviour
{
   [SerializeField] Renderer _renderer;
  [SerializeField]  float factor = 0.05f;
   [SerializeField] float time = 0.01f;

 
    public void Destroy(){
        StartCoroutine(StartDestroying());
    }
    public IEnumerator StartDestroying(){
        while(transform.localScale.x>=0){
            transform.localScale -= new Vector3(factor,factor,factor);
            yield return new WaitForSeconds(time);
        }
    }

    void OnBecameVisible(){
        
         // transform.GetChild(0).gameObject.SetActive(false);
        
        
        
            _renderer.enabled = true;
           //transform.GetChild(0).gameObject.SetActive(true);
        
    }
    void OnBecameInvisible(){
        
         // transform.GetChild(0).gameObject.SetActive(false);
         _renderer.enabled = false;
        
        
 
           //transform.GetChild(0).gameObject.SetActive(true);
        
    }
}
