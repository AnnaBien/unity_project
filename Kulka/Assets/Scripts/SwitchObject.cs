using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchObject : MonoBehaviour {
    public Transform target;

    private void OnCollisionEnter(Collision col) {
        if (target != null) { 
            Destroy(target.gameObject);
            this.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 0.1f, this.gameObject.transform.position.z);
        }
    }
/*
    private void moveSwitch() {
        Vector3 down = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 0.1f, this.gameObject.transform.position.z);
        while(this.transform.position != down) { 
            float step = speed * Time.deltaTime;
            this.transform.position = Vector3.MoveTowards(transform.position, down, 0.001f);
        }
    }
*/
}
