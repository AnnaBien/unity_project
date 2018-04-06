using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHint : MonoBehaviour {
    public GameObject text;
    private float duration;

    public void Start() {
        text.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision) {
        text.SetActive(true);
        duration = Time.time;
    }

    private void Update() {
        if ((Time.time - duration) > 5)
            text.SetActive(false);
    }
}
