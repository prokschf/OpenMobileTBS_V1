using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {
    Vector3 touchStart;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;
	
    // Update is called once per frame
    void Update () {
        if(Input.GetMouseButtonDown(1)){
            touchStart = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                10));
        }
        if(Input.touchCount == 2){
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            zoom(difference * 0.01f);
        }else if (Input.GetMouseButton(1))
        {
            Vector3 direction = touchStart -
                                Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                                    10));
            Camera.main.transform.position += new Vector3(direction.x, 0, direction.z);
        }
        zoom(Input.mouseScrollDelta.y);
    }

    void zoom(float increment)
    {
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
            Mathf.Clamp(Camera.main.transform.position.y  - increment, zoomOutMin, zoomOutMax),
            Camera.main.transform.position.z);
    }
}