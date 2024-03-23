using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CameraDragger : MonoBehaviour
{
     Camera _camera;
    int fingerID = 0;
    bool isDragging = false;
    Vector3 dragPoint;
    Vector3 difference, origin;
    [SerializeField] float minOutZoom,maxOutZoom;
    [SerializeField] LayerMask cellLayers;
    [SerializeField] Vector3 minCameraPos, maxCameraPos;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        #if UNITY_EDITOR
            fingerID = -1;
        #endif
        dragPoint = new Vector3(0f,0f,_camera.transform.position.z);

    }

    // Update is called once per frame
    void LateUpdate()
    {

            #if UNITY_EDITOR || UNITY_STANDALONE_WIN
             if((Input.GetMouseButton(0) || Input.GetMouseButton(2)) && !EventSystem.current.IsPointerOverGameObject(fingerID)){
                 difference = _camera.ScreenToWorldPoint(Input.mousePosition) - _camera.transform.position;
                 if(!isDragging ){
                    // dragPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
                      isDragging = true;
                      origin = _camera.ScreenToWorldPoint(Input.mousePosition);
                     
                      
                      
                 }
                 
             _camera.transform.position = dragPoint;
                   
             }
             #endif
             
             #if UNITY_ANDROID && !UNITY_EDITOR
                if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved && !EventSystem.current.IsPointerOverGameObject(fingerID)){
                    difference = _camera.ScreenToWorldPoint(Input.GetTouch(0).position) - _camera.transform.position;
                    if(!isDragging){
                    // dragPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
                      isDragging = true;
                      origin = _camera.ScreenToWorldPoint(Input.GetTouch(0).position);
                     
                      
                      
                    }
                _camera.transform.position = dragPoint;
                   
                }
             #endif
             else{
                 isDragging = false;
             }
             if(isDragging){
                 _camera.transform.position = new Vector3(Mathf.Clamp((origin-difference).x, minCameraPos.x,maxCameraPos.x),minCameraPos.y,Mathf.Clamp((origin-difference).z, minCameraPos.z,maxCameraPos.z)) ;
             }
            if(Input.touchCount == 2 && !EventSystem.current.IsPointerOverGameObject(fingerID)){
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
                float prevMagnitude= (touchZeroPrevPos- touchOnePrevPos).magnitude;
                float currentMagnitude= (touchZero.position- touchOne.position).magnitude;
                float zoomDifference = currentMagnitude - prevMagnitude;
                Zoom(zoomDifference/100f);
            }
             
         Zoom(Input.mouseScrollDelta.y);
         
    }
    void Zoom(float factor){
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize-factor,minOutZoom, maxOutZoom);
    }
    public void SetMaxCameraPosition(Vector3 pos){
        maxCameraPos = pos;
    }
}
