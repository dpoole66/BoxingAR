using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitDetection : MonoBehaviour {

    Animator m_Anim;
    public Slider health;  

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == "BlueFist") {

            health.value -= 20;
            Debug.Log("Red Hit Detection to Body");
            BodyHit();

        }
    }

    // Use this for initialization
    void Awake () {

        m_Anim = GetComponent<Animator>();                   
		
	}

    public void BodyHit() {

        Debug.Log("Red Controller BodyHit");
        StartCoroutine(HitBody());
        return;

    }

    public IEnumerator HitBody() {

        Debug.Log("Red Controller HitBody");
        m_Anim.SetBool("HitBody", true);
        yield return new WaitForSeconds(0.75f);
        m_Anim.SetBool("HitBody", false);
        yield break;

    }

}
