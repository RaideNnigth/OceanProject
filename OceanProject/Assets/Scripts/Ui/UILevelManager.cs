using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelManager : MonoBehaviour
{   
    [Header("Depth Manager")]
    public Text DepthM;
    public Darkness Dark;
    public Transform PlayerTransform;
    private float depth;

    void Update()
    {
        DepthMeter();
    }
    void DepthMeter()
    {
        Dark.DepthCounter(depth);

        if (PlayerTransform.position.y > 0)
        {
            DepthM.text = "Depth: 0m";
        }
        else
        {
            depth = Mathf.Abs(PlayerTransform.position.y * 2);
            DepthM.text = "Depth: " + depth.ToString("0") + "m";
        }

    }
}
