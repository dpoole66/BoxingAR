using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour {

    public Text tCount;

    GameObject gO = null;
    Plane objPlane;
    Vector3 mO;

    Ray GenerateMouseRay(Vector3 touchPos){

        Vector3 mousePosFar = new Vector3(touchPos.x, touchPos.y, Camera.main.farClipPlane);
        Vector3 mousePosNear = new Vector3(touchPos.x, touchPos.y, Camera.main.nearClipPlane);

        Vector3 mousePosF = Camera.main.ScreenToViewportPoint(mousePosFar);
        Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);

        Ray mr = new Ray(mousePosN, mousePosF - mousePosN);
        return mr;

    }
    private void Update() {

        tCount.text = Input.touchCount.ToString();

        if(Input.touchCount> 0){
            if((Input.GetTouch(0).phase == TouchPhase.Began))   {

                Ray mouseRay = GenerateMouseRay(Input.GetTouch(0).position);
                RaycastHit hit;

                if(Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit)){

                    gO = hit.transform.gameObject;
                    objPlane = new Plane(Camera.main.transform.forward * -1, gO.transform.position);
                    //offset
                    Ray mRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    float rayDistance;
                    objPlane.Raycast(mRay, out rayDistance);
                    mO = gO.transform.position - mRay.GetPoint(rayDistance);

                }

            } else if(Input.GetTouch(0).phase == TouchPhase.Moved && gO){

                Ray mRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                float RayDistance;
                if(objPlane.Raycast(mRay, out RayDistance)){

                    gO.transform.position = mRay.GetPoint(RayDistance) + mO;

                }

            } else if(Input.GetTouch(0).phase == TouchPhase.Ended && gO){

                gO = null;

            }

        }

    }
}
