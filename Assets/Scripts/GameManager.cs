using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    private int numTentativas;          //Armazena as tentativas validas
    private int maxNumTentativas;       //Numero max de tentativas
    int score = 0;

    public GameObject letra;            //prefab da letra no Game
    public GameObject centro;           //objeto de texto que indica centro

    private string palavraOculta = "";  // Palavra a ser adivinhada (LAB1 PARTE A)

    //private string[] palavrasOcultas = new string[] { "carro", "elefante", "futebol" }; //array de palavras ocultas
    
    private int tamanhoPalavraOculta;   //tamanho da palavra oculta
    char[] letrasOcultas;               //letras da palavra oculta
    bool[] letrasDescoberas;            //indicador de quais letras foram descobertas

    
    
    // Start is called before the first frame update
    void Start()
    {
        centro = GameObject.Find("centroDaTela");

        InitGame();
        InitLetras();
        numTentativas = 0;
        maxNumTentativas = 10;
        UpdateNumTentativas();
        UpdateScore();
      
    }


    // Update is called once per frame
    void Update()
    {
        checkTeclado();
    }

    void InitLetras()
    {
        int numLetras = tamanhoPalavraOculta;
        for(int i=0; i < numLetras; i++)
        {
            Vector3 novaPosicao;
            novaPosicao = new Vector3(centro.transform.position.x + ((i-numLetras/2.0f)*80), centro.transform.position.y, centro.transform.position.z);
            GameObject l = (GameObject)Instantiate(letra, novaPosicao, Quaternion.identity);
            l.name = "letra" + (1 + i);         //nomeia na hierarquia a GameObject com letra(i+1), i=1..numLetras
            l.transform.SetParent(GameObject.Find("Canvas").transform);     //filho do gameObject no canvas
        }
    }

    void InitGame()
    {
        //palavraOculta = "Elefante";                         //define palavra a ser descoberta
        //int numeroAleatorio = Random.Range(0, palavrasOcultas.Length); //sorteamos um numero aleatorio dentro do numero de palavras do array
        //palavraOculta=palavrasOcultas[numeroAleatorio];     // selecionamos uma palavra sorteada

        palavraOculta = PegaUmaPalavraDoArquivo();
        tamanhoPalavraOculta = palavraOculta.Length;        //define o numero de letras da palavra oculta
        palavraOculta = palavraOculta.ToUpper();            //transforma a palavra em maiuscula
        letrasOcultas = new char[tamanhoPalavraOculta];     //intanciamos o array de chars das letras da palavra
        letrasDescoberas = new bool[tamanhoPalavraOculta];  //instanciamos o array bool do indicador de letras certas
        letrasOcultas = palavraOculta.ToCharArray();        // copia-se a palavra no array de letras

    }

    void checkTeclado()
    {
        if (Input.anyKeyDown)
        {
            char letraTeclada = Input.inputString.ToCharArray()[0];
            int letraTecladaComoInt = System.Convert.ToInt32(letraTeclada);

            if(letraTecladaComoInt >=97 && letraTecladaComoInt <= 122)
            {
                numTentativas++;
                UpdateNumTentativas();
                if(numTentativas > maxNumTentativas)
                {
                    SceneManager.LoadScene("lab1_forca");                         // a pessoa perdeu e foi enforcada
                }
                for(int i = 0; i <= tamanhoPalavraOculta;i++)
                {
                    if (!letrasDescoberas[i])
                    {
                        letraTeclada = System.Char.ToUpper(letraTeclada);
                        if (letrasOcultas[i] == letraTeclada)
                        {
                            letrasDescoberas[i] = true;
                            GameObject.Find("letra" + (i + 1)).GetComponent<Text>().text = letraTeclada.ToString();
                            score = PlayerPrefs.GetInt("score");
                            score++;
                            PlayerPrefs.SetInt("score", score);
                            UpdateScore();
                            VerificaSePalavraDescoberta();
                        }
                    }
                }
            }
        }
    }

    void UpdateNumTentativas()
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text=numTentativas + " | " + maxNumTentativas;  //valor do num de tentativas na tela

    }

    void UpdateScore()
    {
        GameObject.Find("scoreUI").GetComponent<Text>().text = "Score " + score;    //Valor do score na tela
    }

    void VerificaSePalavraDescoberta()
    {
        bool condicao = true;                                                       // inicializa variavel 
        for(int i = 0; i < tamanhoPalavraOculta; i++)                               // testa usando o tamanho da palavra
        {
            condicao = condicao && letrasDescoberas[i];                             // verifica se todas as letras foram descobertas
        }
        if (condicao)                                                               // a pessoa acertou a palavra e sobreviveu
        {
            PlayerPrefs.SetString("ultimaPalavraOculta", palavraOculta);            // indica qual era a palavra
            SceneManager.LoadScene("lab1_salvo");                                   // faz load da cena em que o jogador sobrevive
        }
    }
    
    string PegaUmaPalavraDoArquivo()
    {
        TextAsset t1 = (TextAsset)Resources.Load("palavras", typeof(TextAsset));     // leitura de arquivo
        string s = t1.text;                                                          // seleciona o texto do asset e coloca na variavel s
        string[] palavras = s.Split(' ');                                            // separa o conjunto de palavras em um vetor, usando o espaço como separador
        int palavraAleatoria = Random.Range(0, palavras.Length + 1);                 // sorteamos uma palavra aleatória 
        return (palavras[palavraAleatoria]);                                         // retorna uma das palavras sorteadas
    }

}
