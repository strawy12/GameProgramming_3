using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilClass
{
    private static Camera mainCam;
    public static Vector3 GetMouseWorldPosition()
    {
        if(mainCam == null) mainCam = Camera.main;

        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        return mousePos;
    }
}
