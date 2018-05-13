using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedCombat : MonoBehaviour {

    Animator m_Anim;
    public Slider health;
    private GameObject m_Blue;
    private BlueController m_Controller;

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == "Blue") {

            health.value -= 20;
            Debug.Log("Blue was Hit");
            m_Controller.BodyHit();

        }

    }

    // Use this for initialization
    void Awake() {

        m_Anim = GetComponent<Animator>();
        m_Blue = GameObject.FindGameObjectWithTag("Blue");
        m_Controller = m_Blue.GetComponent<BlueController>();

    }

}
