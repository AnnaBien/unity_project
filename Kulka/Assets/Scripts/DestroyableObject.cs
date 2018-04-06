using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour {

    public float forceRequired = 10.0f; //z taka sila trzeba uderzyc
    public GameObject burstPrefab;

    private void Start() {
        burstPrefab.SetActive(false);
    }
    private void OnCollisionEnter(Collision col) {
        if(col.impulse.magnitude > forceRequired) {
            burstPrefab.SetActive(true);
            Instantiate(burstPrefab, col.contacts[0].point, Quaternion.identity);
            Destroy(gameObject);
        }
    }
  
}
