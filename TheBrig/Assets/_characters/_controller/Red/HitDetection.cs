using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour {

    Animator m_Anim;

    private void OnTriggerEnter(Collider other) {

        Debug.Log("Hit");
        StartCoroutine(HitBody());

    }

    // Use this for initialization
    void Awake () {

        m_Anim = GetComponent<Animator>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator HitBody(){

        m_Anim.SetBool("HitBody", true);
        yield return new WaitForSeconds(0.75f);
        m_Anim.SetBool("HitBody", false);
        yield return null;

    }
}
