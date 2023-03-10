using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera vCam;
    private float orthographicSize;
    private float targetOrthographicSize;
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float minSize = 10f;
    [SerializeField]
    private float maxSize = 30f;
    [SerializeField]
    private float zoomSpeed = 5f;
    private void Start()
    {
        orthographicSize = vCam.m_Lens.OrthographicSize;
        targetOrthographicSize = orthographicSize;
    }
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(x, y, 0).normalized;

        transform.Translate(dir * speed * Time.deltaTime);

        targetOrthographicSize += -Input.mouseScrollDelta.y;

        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minSize, maxSize);
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);
         
        vCam.m_Lens.OrthographicSize = orthographicSize;
    }
}
