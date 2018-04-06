using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Ładowanie danych, zmienna zapisujaca ilosc pieniedzy*/

public class GameManager : MonoBehaviour {

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public int currentSkinIndex = 0;
    public int currency = 0;
    public int skinAvailability = 1;


    void Start() {

    }

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
        if (PlayerPrefs.HasKey("CurrentSkin")/*boolean*/) {          //Jesli juz wczesniej gralismy - zaladuj dane
            currentSkinIndex = PlayerPrefs.GetInt("CurrentSkin");
            currency = PlayerPrefs.GetInt("Currency");
            skinAvailability = PlayerPrefs.GetInt("SkinAvailability");
        }
        else {
            Save();
        }
    }

    public void Save() {
        PlayerPrefs.SetInt("CurrentSkin", currentSkinIndex);      // nadajemy ustawienia startowe
        PlayerPrefs.SetInt("Currency", currency);                 // zastapione przez inicjalizacje przy deklaracji
        PlayerPrefs.SetInt("SkinAvailability", skinAvailability);
    }

    public string coinCapture() {
        currency += 1;
        return currency.ToString();
    }

}