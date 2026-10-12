using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject telaFim;

    private int restantes;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        restantes = GameObject.FindGameObjectsWithTag("Coletavel").Length;
        telaFim.SetActive(false);
    }

    public void Coletou()
    {
        restantes--;
        if (restantes == 0)
        {
            telaFim.SetActive(true);
            Time.timeScale = 0f;
        }
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
