using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mover : MonoBehaviour {

    Transform target;
    Transform m_Player;

    private void Awake() {

        m_Player = GetComponent<Transform>();
        target = null;

    }

    void Update () {

        if(Input.GetMouseButtonDown(0)){

                target.position = Input.mousePosition;
                Vector3.MoveTowards(m_Player.position, target.position, 1.0f);

        }
		
	}
}
