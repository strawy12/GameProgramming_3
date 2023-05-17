using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrder : MonoBehaviour
{
    [SerializeField] private bool runOnce;
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void LateUpdate()
    {
        float precisionMulplier = 5f;
        spriteRenderer.sortingOrder = (int)(-(transform.position.y-transform.localPosition.y)* precisionMulplier);

        if (runOnce)
        {
            Destroy(this);
        }
    }
}
