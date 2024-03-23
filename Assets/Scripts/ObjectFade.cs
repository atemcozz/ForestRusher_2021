using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFade : MonoBehaviour
{
  [SerializeField]  float factor = 0.05f;
   [SerializeField] float time = 0.01f;
   Collider _collider;
   void Start(){
       _collider = GetComponent<Collider>();
   }
    public void Destroy(){
        _collider.enabled = false;
        StartCoroutine(StartDestroying());
    }
    public IEnumerator StartDestroying(){
        while(transform.localScale.x>=0){
            transform.localScale -= new Vector3(factor,factor,factor);
            yield return new WaitForSeconds(time);
        }
    }

}
