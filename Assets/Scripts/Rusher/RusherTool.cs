using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RusherTool : MonoBehaviour
{
    [SerializeField] RusherManager.ObstacleType type;

    public void UseTool()
    {
        RusherManager.Instance.Player.DestroyObstacle(type);
    }
}
