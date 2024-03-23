using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField]float followingSpeed = 0.5f;
   [SerializeField] Transform _targetPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       // transform.Translate(new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime,0f, Input.GetAxis("Vertical") * speed * Time.deltaTime));   
        transform.position = Vector3.Lerp(transform.position,new Vector3(_targetPos.position.x,0f,_targetPos.position.z),followingSpeed);
    }
}
