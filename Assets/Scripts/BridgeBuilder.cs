using System.Collections.Generic;
using UnityEngine;

public class BridgeBuilder : MonoBehaviour
{
    public Node prefabPonto;
    public Beam prefabViga;
    public Transform grupoPonte;
    public LineRenderer previa;
    public float tamanhoMaximo = 2f;
    public float grade = 0.5f;

    private List<Node> pontos = new List<Node>();
    private List<Beam> vigas = new List<Beam>();
    private Node inicio;
    private bool testando;

    void Start()
    {
        pontos.AddRange(grupoPonte.GetComponentsInChildren<Node>());
        previa.enabled = false;
    }

    void Update()
    {
        if (testando) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Testar();
            return;
        }

        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            inicio = PontoPerto(mouse);
        }

        if (inicio == null) return;

        Vector2 fim = PontoFinal(mouse);
        previa.enabled = true;
        previa.SetPosition(0, inicio.transform.position);
        previa.SetPosition(1, fim);

        if (Input.GetMouseButtonUp(0))
        {
            CriarViga(inicio, fim);
            inicio = null;
            previa.enabled = false;
        }
    }

    Vector2 PontoFinal(Vector2 mouse)
    {
        Vector2 origem = inicio.transform.position;
        Vector2 alvo = origem + Vector2.ClampMagnitude(mouse - origem, tamanhoMaximo);
        alvo.x = Mathf.Round(alvo.x / grade) * grade;
        alvo.y = Mathf.Round(alvo.y / grade) * grade;
        return alvo;
    }

    Node PontoPerto(Vector2 posicao)
    {
        foreach (Node p in pontos)
        {
            if (Vector2.Distance(p.transform.position, posicao) < 0.3f) return p;
        }
        return null;
    }

    bool VigaExiste(Node a, Node b)
    {
        foreach (Beam v in vigas)
        {
            if ((v.a == a && v.b == b) || (v.a == b && v.b == a)) return true;
        }
        return false;
    }

    void CriarViga(Node a, Vector2 fim)
    {
        Node b = PontoPerto(fim);
        if (b == a || (b != null && VigaExiste(a, b))) return;

        if (b == null)
        {
            b = Instantiate(prefabPonto, fim, Quaternion.identity, grupoPonte);
            pontos.Add(b);
        }

        Beam viga = Instantiate(prefabViga, grupoPonte);
        viga.Conectar(a, b);
        vigas.Add(viga);
    }

    void Testar()
    {
        testando = true;
        previa.enabled = false;

        foreach (Node p in pontos)
        {
            if (!p.ancora) p.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
        foreach (Beam v in vigas)
        {
            v.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
