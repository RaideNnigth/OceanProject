using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Darkness : MonoBehaviour
{
    public Image image;
    float depth;
    void Start()
    {
        image = GetComponent<Image>();
        var tempColor = image.color;
        tempColor.a = 0f;
        image.color = tempColor;
    }

    public void DepthCounter(float deep)
    {
        depth = deep;
    }
    void Update()
    {
        image = GetComponent<Image>();
        var tempColor = image.color;
        tempColor.a = depth / 600;
        image.color = tempColor;
    }
}
