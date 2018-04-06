using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBox : MonoBehaviour {

    private void OnCollisionEnter(Collision collision) {
        LevelManager.Instance.Victory();
    }

}
