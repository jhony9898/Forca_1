using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("score", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMundoGame()
    {
        SceneManager.LoadScene("Lab1");  //load cena 1
    }
    public void StartMenu()
    {
        SceneManager.LoadScene("lab1_start"); //load cena Menu
    }

    public void StartCreditos()
    {
        SceneManager.LoadScene("lab1_creditos"); //load cena créditos
    }
}
