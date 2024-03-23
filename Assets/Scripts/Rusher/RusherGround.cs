using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RusherGround : MonoBehaviour
{
    [SerializeField] float factor;
    Material groundMaterial;
    float currentScroll = 0;

    // Start is called before the first frame update
    void Start()
    {
        groundMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        currentScroll += Time.deltaTime * factor * RusherManager.Instance.GetSpeedFactor();
       // groundMaterial.SetTextureOffset("_MainTex",new Vector2(groundMaterial.mainTextureOffset.x,-Time.time * RusherManager.Instance.GetSpeedFactor() * factor));
        groundMaterial.mainTextureOffset = new Vector2(0,currentScroll);
    }
}
