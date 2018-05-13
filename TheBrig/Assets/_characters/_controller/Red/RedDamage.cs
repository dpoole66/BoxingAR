using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedDamage : MonoBehaviour {

    Animator m_Anim;
    public Slider health;
    public string Opponent;

    void Start() {

        m_Anim = GetComponent<Animator>();                                    

    }

    private void OnTriggerEnter(Collider other) {

        if (m_Anim && (other.gameObject.tag != Opponent))
        return;

        health.value -= 20;
        StartCoroutine(HeadHit());

        if (health.value <= 0)
        m_Anim.SetBool("Dead", true);
   
    }

    public IEnumerator HeadHit(){

        m_Anim.SetBool("HitHead", true);
        yield return new WaitForSeconds(1.0f);
        m_Anim.SetBool("HitHead", false);
        m_Anim.SetBool("Idle", true);

        yield break;

    }

}
