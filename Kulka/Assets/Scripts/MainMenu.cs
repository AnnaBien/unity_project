using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelData {    //zapisywanie czasów przejścia dla każdego poziomu

    public LevelData(string levelName) {
        string data = PlayerPrefs.GetString(levelName); //oczytuje nie nazwe planszy tylko np 00&00&00

        if (data == "")         //jezeli nie bylo wczesniej grane - nie ma czasów przejścia planszy
            return;

        string[] allData = data.Split('&');     //odzielanie poprzez &
        BestTime = float.Parse(allData[0]);
        SilverTime = float.Parse(allData[1]);
        GoldTime = float.Parse(allData[2]);

        string minutes = ((int)(BestTime) / 60).ToString("00");
        string seconds = ((BestTime) % 60).ToString("00.00");

        BestTime2 = minutes + ":" + seconds;
    }
    public float BestTime { set; get; }
    public float SilverTime { set; get; }
    public float GoldTime { set; get; }
    public string BestTime2 { set; get; }
}


public class MainMenu : MonoBehaviour {

    public GameObject levelButtonPrefab;
    public GameObject levelButtonContainer;

    public GameObject shopButtonPrefab;
    public GameObject shopButtonContainer;

    //private Transform cameraTransform;          //W jakim kierunku jest zwrocona kamera
    public GameObject menu;
    public GameObject shop;
    public GameObject levels;

    //public GameObject image;
    //private GameObject[] playerSkins = new GameObject[16];

    public Text currencyText;
    private bool nextLevelLocked = false;

    public Material playerMaterial;
    private int[] costs = { 0, 50, 50, 50, 100, 100, 100, 100, 150, 150, 175, 175, 200, 200, 250, 300 };          //16 - ilosc skorek

    void Start () {
        menu.SetActive(true);
        shop.SetActive(false);
        levels.SetActive(false);

        ChangePlayerSkin(GameManager.Instance.currentSkinIndex);
        currencyText.text = "Coins : " + GameManager.Instance.currency.ToString();
        //cameraTransform = Camera.main.transform;
        
        //------------INICJOWANIE POZIOMÓW GRY---------------------------------------------------------------------------------------------
        Sprite[] thumbnails = Resources.LoadAll<Sprite>("Levels");      //Lista grafik dla kazdego poziomu
        bool ifTraining = true;
        foreach (Sprite thumbnail in thumbnails)                         //dla kazdej grafiki w liscie grafik
        {                                                               //Sprite - bitmapa 2D podpinana do celu bez zbędnych ceregieli
            GameObject container = Instantiate(levelButtonPrefab) as GameObject;    //kontenerem jest button
            container.GetComponent<Image>().sprite = thumbnail;                     //do kontenera wrzucamy obrazek
            container.transform.SetParent(levelButtonContainer.transform, false);   //utworzony obiekt (button z obrazkiem) podpinamy do rodzica
                                                                                    //zmiana pozycji obiektu: false - nie utrzymuj swojej pozycji tylko pojdz na pozycje swojego rodzica
            LevelData level = new LevelData(thumbnail.name);            //thumbnail.name = (np) 1_Training
            container.transform.GetChild(0).GetChild(0).GetComponent<Text>().text =(level.BestTime != 0.0f) ? level.BestTime2 : "Play";
            if (ifTraining == true) {
                container.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = (level.BestTime != 0.0f) ? level.BestTime2 : "Training";
                ifTraining = false;
            }

            container.transform.GetChild(1).GetComponent<Image>().enabled = nextLevelLocked;
            container.GetComponent<Button>().interactable = !nextLevelLocked;

            if (nextLevelLocked == true)    //następny level jest zablokowany
                container.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Locked";

            if (level.BestTime == 0.0f) {   //jesli ten level nie ma czasu przejscia to następny będzie zablokowany
                nextLevelLocked = true;
            }

            string sceneName = thumbnail.name;
            container.GetComponent<Button>().onClick.AddListener(() => LoadLevel(sceneName));   //wywolaj funkcje i przekaz nazwe levelu
        }
        //------------INICJOWANIE SKÓREK W SKLEPIE-----------------------------------------------------------------------------------------
        int textureIndex = 0;

        Sprite[] skins = Resources.LoadAll<Sprite>("Player");      
        foreach (Sprite skin in skins)        //inicjowanie wszystkich tekstur                       
        {                                                               
            GameObject container = Instantiate(shopButtonPrefab) as GameObject;   
            container.GetComponent<Image>().sprite = skin;                     
            container.transform.SetParent(shopButtonContainer.transform, false);
            container.transform.GetChild(1).GetComponent<Image>().gameObject.SetActive(false);
            

            int index = textureIndex;
            container.GetComponent<Button>().onClick.AddListener(() => ChangePlayerSkin(index));        //metoda wywolywana po kliknieciu na skorke
            container.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = costs[index].ToString();
            //container.transform.GetComponentInChildren<Text>();
            if ((GameManager.Instance.skinAvailability & (1 << index)) == 1 << index) {
                container.transform.GetChild(0).gameObject.SetActive(false);        //wlaczenie skorki jesl zostala wczesniej kupiona
               // currentPlayerSkinMatched.transform.GetChild(1).GetComponent<Image>().gameObject.SetActive(true);
            }
           // playerSkins[textureIndex] = container;
           // if(GameManager.Instance.currentSkinIndex == textureIndex)
           //     playerSkins[textureIndex].transform.GetChild(1).GetComponent<Image>().gameObject.SetActive(true);

            textureIndex++;
        }
        //---koniec Start--------------------------------------------------------------------------------------------------------------------------------
    }
    
    private void LoadLevel( string sceneName)   //zaladuj wybrany level
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
        CoinRotation.changeStop(false);     //Niewazne jaki byl stan wlacz obrot pieniazkow
    }

    //----------------------------------------obsługa przycisków w menu------------------------------------------------   
    public void ToShop() {
        menu.SetActive(!menu.activeSelf);
        shop.SetActive(!shop.activeSelf);
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void ToLevels() {
        menu.SetActive(!menu.activeSelf);
        levels.SetActive(!levels.activeSelf);
    }

    public void ToMenu() {
        menu.SetActive(!menu.activeSelf);
        if (levels.activeSelf == true)
            levels.SetActive(!levels.activeSelf);
        else
            shop.SetActive(!shop.activeSelf);
    }
        
    //-----------------------------------------------------------------------------------------------------------------  

    private void ChangePlayerSkin(int index) {                   //Sprawdz czy skin jest dostepny w skinAvailability
        if ((GameManager.Instance.skinAvailability & (1 << index)) == 1 << index) {      //x << y (x po przesunięciu wszystkich jego bitów o y pozycji w lewo/w prawo )
            float x = (index % 4) * 0.25f;                                               // x << 1 (mnozenie x przez 2)   x >> 1 (dzielenie x przez 2)
            float y = ((int)index / 4) * 0.25f;                                          // x & y - iloczyn bitowy (mnozenie odpowiadajacych sobie bitow)           
                                                                                         
            y = 0.75f - y; //aby indeksowanie szlo od gory
            playerMaterial.SetTextureOffset("_MainTex", new Vector2(x, y));
            
            GameManager.Instance.currentSkinIndex = index;
            GameManager.Instance.Save();
            //playerSkins[index].GetComponent<Image>().gameObject.SetActive(true);
        }
        else {      //skorka nie jest dostepna, czy chcesz ja kupic?
            int cost = costs[index];
            if(GameManager.Instance.currency >= cost) {     //jesli Cie stac
                GameManager.Instance.currency -= cost;      //kup (odejmowanie kosztow)
                currencyText.text = "Coins : " + GameManager.Instance.currency.ToString();
                GameManager.Instance.skinAvailability += 1 << index;    //dodawanie kupionej skorki do skinAvailability
                GameManager.Instance.Save();                //zapisanie nowych danych
                shopButtonContainer.transform.GetChild(index).GetChild(0).gameObject.SetActive(false);      //wylaczenie obiektu
                ChangePlayerSkin(index);
            }
        }
    }
}
