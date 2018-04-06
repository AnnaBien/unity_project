using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationObject : MonoBehaviour {
    private bool touched = false;
    private Vector3 desiredPosition;


    public void OnCollisionEnter(Collision collision) {
        touched = true;
    }

    private void Update() {
        if (touched == true) 
            this.transform.Rotate(0, 0.5f, 0);
    }
}
