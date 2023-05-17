using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarforceBtn : MonoBehaviour
{
    [SerializeField]
    private Building building;
    public void Awake()
    {
        transform.Find("button").GetComponent<Button>().onClick.AddListener(() =>
        {
            StarForceUI.Inst.Open(building);
        });
    }
}
