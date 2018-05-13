using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueCombat : MonoBehaviour {

    public Slider Health;

    public void Awake() {

        var health = GameObject.FindGameObjectWithTag("BlueHealth");
        Health = health.GetComponent<Slider>();

    }


    private void OnTriggerEnter(Collider other) {

        if(other.gameObject.tag == "Red"){     

            Health.value -= 20;
            Debug.Log("Blue was Hit");
            Debug.Log(Health);

        }

    }

}
