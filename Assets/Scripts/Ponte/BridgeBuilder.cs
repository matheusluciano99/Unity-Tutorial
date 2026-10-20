using System.Collections.Generic;
using UnityEngine;

public class BridgeBuilder : MonoBehaviour
{
    public Node prefabPonto;
    public Beam prefabViga;
    public Transform grupoPonte;
    public LineRenderer previa;
    public Transform cursor;
    public AudioClip somViga;
    public float tamanhoMaximo = 2f;
    public float grade = 0.5f;
    public int orcamento = 1000;
    public int custoPorUnidade = 100;

    public bool construindo = true;
    public int gasto;

    private List<Node> pontos = new List<Node>();
    private List<Beam> vigas = new List<Beam>();
    private List<Vector3> posicoesSalvas = new List<Vector3>();
    private Node inicio;
    private Vector2 posCursor;

    void Start()
    {
        pontos.AddRange(grupoPonte.GetComponentsInChildren<Node>());
        previa.enabled = false;
    }

    void Update()
    {
        cursor.gameObject.SetActive(construindo);
        if (!construindo) return;

        MoverCursor();
        Vector2 alvo = Arredondar(posCursor);

        if (Input.GetMouseButtonDown(1)) ApagarViga(posCursor);
        if (Input.GetMouseButtonDown(0)) inicio = PontoPerto(alvo);
        if (inicio == null) return;

        Vector2 fim = PontoFinal(posCursor);
        bool cabe = gasto + Custo(Vector2.Distance(inicio.transform.position, fim)) <= orcamento;
        previa.enabled = true;
        previa.startColor = previa.endColor = cabe ? new Color(1f, 1f, 1f, 0.6f) : new Color(1f, 0.2f, 0.2f, 0.6f);
        previa.SetPosition(0, inicio.transform.position);
        previa.SetPosition(1, fim);

        if (Input.GetMouseButtonUp(0))
        {
            if (cabe) CriarViga(inicio, fim);
            inicio = null;
            previa.enabled = false;
        }
    }

    void MoverCursor()
    {
        posCursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursor.position = Arredondar(posCursor);
    }

    Vector2 Arredondar(Vector2 p)
    {
        return new Vector2(Mathf.Round(p.x / grade) * grade, Mathf.Round(p.y / grade) * grade);
    }

    Vector2 PontoFinal(Vector2 mouse)
    {
        Vector2 origem = inicio.transform.position;
        return Arredondar(origem + Vector2.ClampMagnitude(mouse - origem, tamanhoMaximo));
    }

    int Custo(float comprimento)
    {
        return Mathf.CeilToInt(comprimento * custoPorUnidade);
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
        gasto += Custo(viga.transform.localScale.x);
        GameManager.instance.Tocar(somViga);
    }

    void ApagarViga(Vector2 posicao)
    {
        Collider2D toque = Physics2D.OverlapPoint(posicao, LayerMask.GetMask("Ponte"));
        if (toque == null) return;

        Beam viga = toque.GetComponent<Beam>();
        vigas.Remove(viga);
        gasto -= Custo(viga.transform.localScale.x);
        Destroy(viga.gameObject);
        ApagarSeSolto(viga.a);
        ApagarSeSolto(viga.b);
    }

    void ApagarSeSolto(Node p)
    {
        if (p.ancora) return;
        foreach (Beam v in vigas)
        {
            if (v.a == p || v.b == p) return;
        }
        pontos.Remove(p);
        Destroy(p.gameObject);
    }

    public void Testar()
    {
        construindo = false;
        inicio = null;
        previa.enabled = false;

        posicoesSalvas.Clear();
        foreach (Node p in pontos)
        {
            posicoesSalvas.Add(p.transform.position);
            if (!p.ancora) p.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
        foreach (Beam v in vigas)
        {
            v.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }

    // Devolve cada ponto à posição de antes do teste e remonta as vigas (inclusive as quebradas).
    public void Reconstruir()
    {
        for (int i = 0; i < pontos.Count; i++)
        {
            Parar(pontos[i].GetComponent<Rigidbody2D>());
            pontos[i].transform.SetPositionAndRotation(posicoesSalvas[i], Quaternion.identity);
        }
        foreach (Beam v in vigas)
        {
            Parar(v.GetComponent<Rigidbody2D>());
            v.Conectar(v.a, v.b);
        }
        construindo = true;
    }

    void Parar(Rigidbody2D rb)
    {
        if (rb.bodyType == RigidbodyType2D.Static) return;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
}
