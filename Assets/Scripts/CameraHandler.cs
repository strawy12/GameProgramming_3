using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera vCam;
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3 (x, y, 0).normalized;
        float speed = 5f;

        transform.Translate(dir *speed* Time.deltaTime);
        vCam.m_Lens.OrthographicSize += Input.mouseScrollDelta.y;
    }
}
