using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumController : MonoBehaviour {

    private Vector3 movePoint;
    public float speed = 2.0f;                       
    public GameObject particle;
    private Transform m_Player;

    private void Start() {

        particle = GetComponent<GameObject>();

    }

    // Update is called once per frame
    void Update () {

        if((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))  {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out hit)) {

                Instantiate(particle, hit.point, transform.rotation);
                m_Player.position = Vector3.Lerp(gameObject.transform.position, movePoint, 1 / (speed * (Vector3.Distance(gameObject.transform.position, movePoint))));

            }

        }
      
	}
}
