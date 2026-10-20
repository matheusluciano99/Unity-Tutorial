using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public BridgeBuilder construtor;
    public Carro carro;
    public float tempoLimite = 90f;
    public int carros = 3;
    public float alturaQueda = -7f;
    public string proximaFase = "Fase2";

    public TMP_Text textoTempo;
    public TMP_Text textoOrcamento;
    public TMP_Text textoCarros;
    public TMP_Text textoBotao;
    public TMP_Text textoResultado;
    public TMP_Text textoDerrota;
    public GameObject telaVitoria;
    public GameObject telaDerrota;
    public GameObject botaoVitoria;
    public GameObject botaoDerrota;

    public AudioSource som;
    public AudioClip somVitoria;
    public AudioClip somQueda;

    private float tempoRestante;
    private bool acabou;

    void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        tempoRestante = tempoLimite;
        telaVitoria.SetActive(false);
        telaDerrota.SetActive(false);
    }

    void Update()
    {
        if (acabou) return;

        tempoRestante -= Time.deltaTime;
        if (tempoRestante <= 0f)
        {
            tempoRestante = 0f;
            Perder("O tempo acabou!");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AlternarModo();
        }

        if (!construtor.construindo && carro.Altura() < alturaQueda)
        {
            Tocar(somQueda);
            carros--;
            if (carros == 0) Perder("Seus carros acabaram!");
            else VoltarConstrucao();
        }

        AtualizarHud();
    }

    public void AlternarModo()
    {
        // Tira o foco do botão para Espaço/A não acionarem ele de novo.
        EventSystem.current.SetSelectedGameObject(null);

        if (construtor.construindo)
        {
            construtor.Testar();
            carro.Ligar();
            textoBotao.text = "Construir";
        }
        else
        {
            VoltarConstrucao();
        }
    }

    void VoltarConstrucao()
    {
        carro.Voltar();
        construtor.Reconstruir();
        textoBotao.text = "Testar";
    }

    void AtualizarHud()
    {
        textoTempo.text = "Tempo: " + Mathf.CeilToInt(tempoRestante) + "s";
        textoOrcamento.text = "Orçamento: $" + (construtor.orcamento - construtor.gasto);
        textoCarros.text = "Carros: " + carros;
    }

    public void Tocar(AudioClip clip)
    {
        som.PlayOneShot(clip);
    }

    public void Venceu()
    {
        if (acabou) return;

        float tempoGasto = tempoLimite - tempoRestante;
        int pontos = construtor.orcamento - construtor.gasto + Mathf.CeilToInt(tempoRestante) * 10;
        textoResultado.text = "Tempo: " + tempoGasto.ToString("0.0") + "s\nPontos: " + pontos;
        Tocar(somVitoria);
        Fim(telaVitoria, botaoVitoria);
    }

    void Perder(string motivo)
    {
        textoDerrota.text = motivo;
        Fim(telaDerrota, botaoDerrota);
    }

    void Fim(GameObject tela, GameObject botao)
    {
        acabou = true;
        AtualizarHud();
        tela.SetActive(true);
        EventSystem.current.SetSelectedGameObject(botao);
        Time.timeScale = 0f;
    }

    public void ProximaFase()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(proximaFase);
    }

    public void JogarDeNovo()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VoltarAoMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
