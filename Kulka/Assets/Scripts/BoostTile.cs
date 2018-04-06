using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostTile : MonoBehaviour {

    private void OnCollisionEnter(Collision collision) {
        Motor.Instance.BoostTile();
    }
}
