using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*Śmierć, zwycięstwo*/

public class LevelManager : MonoBehaviour {

    public GameObject pauseMenu;
    public GameObject winPanel;

    public VirtualJoystick moveJoystick;
    public Button boost;

    private static LevelManager instance;
    public static LevelManager Instance { get { return instance; }}

    private float startTime;
    public float silverTime;
    public float goldTime;

    public Transform respawnPoint;
    private GameObject player;

    public Text timerText;

    private bool victory = false;


    private void Start() {
        instance = this;
        pauseMenu.SetActive(false);
        winPanel.SetActive(false);
        startTime = Time.time;
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = respawnPoint.position;
    }

    public void TogglePauseMenu() {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;       //pauza
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        CoinRotation.changeStop(true);                      //zaleznie od stanu zmien go na przeciwny
        moveJoystick.enabled = !moveJoystick.isActiveAndEnabled;
        boost.enabled = !boost.isActiveAndEnabled;
    }
    public void ToMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void Victory() {
        float duration = Time.time - startTime;             //czas przechodzenia poziomu
        /*if (duration < goldTime)                            //nagrody za przejscie levelu
            GameManager.Instance.currency += 50;    //50
        else if (duration < silverTime)
            GameManager.Instance.currency += 25;    //25
        else
            GameManager.Instance.currency += 10;    //10*/

        GameManager.Instance.Save();
        string saveString = "";
        LevelData level = new LevelData(SceneManager.GetActiveScene().name);        //nazwa np 1_Training
        saveString += (level.BestTime > duration || level.BestTime == 0.0f) ? duration.ToString() : level.BestTime.ToString();
        saveString += '&';
        saveString += (duration < silverTime) ? duration.ToString() : silverTime.ToString();
        saveString += '&';
        saveString += (duration < goldTime) ? duration.ToString() : goldTime.ToString();
        PlayerPrefs.SetString(SceneManager.GetActiveScene().name, saveString);              //nazwa sceny nadpisana czasami przejscia sceny

        //SceneManager.LoadScene("MainMenu");     //powrót do menu
        winPanel.SetActive(true);
        victory = true;
    }

    private void Update() {
        if (victory == false) {
            string minutes = ((int)(Time.time - startTime) / 60).ToString("00");
            string seconds = ((Time.time - startTime) % 60).ToString("00.00");
            timerText.text = minutes + ":" + seconds;

            if (player.transform.position.y < -10.0f) { 
                PlayAgain();
                GameManager.Instance.currency -= CoinRotation.coinsPerLevel;        //odejmij zebrane pieniążki 
                GameManager.Instance.Save();
            }
        }   
     }
/*
    public void Death() {
       player.transform.position = respawnPoint.position;
        Rigidbody rigid = player.GetComponent<Rigidbody>();
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }*/

    public void PlayAgain()   //zaladuj wybrany level - dla ,,Play Again" z Pause Menu
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        CoinRotation.changeStop(false);     //Niewazne jaki byl stan wlacz obrot pieniazkow
    }


}
