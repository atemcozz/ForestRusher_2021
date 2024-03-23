using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainOffset : MonoBehaviour
{
    Camera _camera;
    Vector3 startCameraPos;
    Material material;
    // Start is called before the first frame update
    void Start()
    {
       _camera = Camera.main; 
       startCameraPos =_camera.transform.position;
       material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 offset = new Vector2(startCameraPos.x-transform.position.x,startCameraPos.z-transform.position.z);
        material.SetTextureOffset("_MainTex", offset);
    }
}
