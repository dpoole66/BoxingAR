using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ropes : MonoBehaviour {

    public Animator m_Red;
    public Animator m_Blue;
    public Rigidbody m_R_Blue;

    private void OnTriggerEnter(Collider other) {
        
         if(other.CompareTag("Blue")) {

            m_R_Blue.isKinematic = false;

         }

    }

    private void OnTriggerExit(Collider other) {

        if (other.CompareTag("Blue")) {

            m_R_Blue.isKinematic = true;

        }

    }
}
