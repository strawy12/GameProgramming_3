using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterManagement : MonoBehaviour
{
    private enum Disasters { Flood, VolcanicEruption, EarthQuake};
    private Disasters nextDisasters = Disasters.Flood;
    
    private void Start()
    {
        SetNextDisaster();
    }

    private void Update()
    {
        
    }

    private void SetNextDisaster()
    {
        int index = Random.Range(0, 3);
        switch(index)
        {
            case 0:
                nextDisasters = Disasters.Flood;
                break;
            case 1:
                nextDisasters = Disasters.VolcanicEruption;
                break;
            case 2:
                nextDisasters = Disasters.EarthQuake;
                break;
        }
    }
}
