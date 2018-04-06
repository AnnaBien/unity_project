using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoinRotation : MonoBehaviour {

    public Text coins;
    public static int  coinsPerLevel { set; get; }
    private static bool stop = false;

    private void OnTriggerEnter(Collider other) {
        coinsPerLevel += 1;
        GameManager.Instance.currency += 1;
        GameManager.Instance.Save();
        coins.text = "Coins: "+ GameManager.Instance.currency.ToString();
        Destroy(this.gameObject);
    }
    private void Start() {
        coins.text = "Coins: " + GameManager.Instance.currency.ToString();
        coinsPerLevel = 0;
    }
    
	void Update () {
        if(stop==false)
            this.transform.Rotate(2, 0, 0);
        
    }

    public static void changeStop(bool x) {
        if (x == true)
            stop = !stop;
        else
            stop = false;
    }
}
