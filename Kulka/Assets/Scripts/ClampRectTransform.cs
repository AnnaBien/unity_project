using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampRectTransform : MonoBehaviour {

    public float padding = 5.0f;
    public float elementSize = 128.0f;
    public float viewSize = 350.0f;

    private RectTransform rt;
    private int amountElements;
    private float contentSize;


	// Use this for initialization
	void Start () {
        rt = GetComponent<RectTransform>();	
	}
	
	// Update is called once per frame
	void Update () {
        amountElements = rt.childCount;
        contentSize = ((amountElements * (elementSize + padding)) - viewSize) * rt.localScale.x;

        if (rt.localPosition.x > padding)
            rt.localPosition = new Vector3(padding, rt.localPosition.y, rt.localPosition.z);

        else if(rt.localPosition.x < -contentSize)
            rt.localPosition = new Vector3(-contentSize, rt.localPosition.y, rt.localPosition.z);
    }
}
