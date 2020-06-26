using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenLogic : MonoBehaviour
{
    float MaxOxygen = 30f;
    
    [SerializeField]
    float Oxygen;

    public Slider OxygenSlider;

    bool HeadinWater = false;
    void Start()
    {
        //Resets your Oxygen
        Oxygen = MaxOxygen;
    }

  
    void Update()
    {
        //Lose or get Oxygen
        if (HeadinWater)
        {
            if (Oxygen > 0)
            {
                Oxygen -= Time.deltaTime;
            }
           
        } else
        {
            if (Oxygen < MaxOxygen)
            {
                Oxygen += Time.deltaTime * 8;
            }
           
        }

        //Updates the Oxygen Meter
        OxygenSlider.maxValue = MaxOxygen;
        OxygenSlider.value = Oxygen;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Check if Head In Water
        if (other.CompareTag("Water"))
        {
            HeadinWater = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        //Check if Head Out of Water
        if (collision.CompareTag("Water"))
        {
            HeadinWater = false;
        }
    }
}
