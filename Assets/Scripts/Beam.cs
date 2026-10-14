using UnityEngine;

public class Beam : MonoBehaviour
{
    public Node a;
    public Node b;
    public AudioClip somQuebra;

    private HingeJoint2D[] juntas;
    private SpriteRenderer visual;
    private Rigidbody2D corpo;
    private Color corNormal;

    void Awake()
    {
        juntas = GetComponents<HingeJoint2D>();
        visual = GetComponent<SpriteRenderer>();
        corpo = GetComponent<Rigidbody2D>();
        corNormal = visual.color;
    }

    // Durante o teste a viga avermelha conforme a força chega perto do limite de quebra.
    void Update()
    {
        if (corpo.bodyType != RigidbodyType2D.Dynamic) return;

        float esforco = 0f;
        foreach (HingeJoint2D j in juntas)
        {
            if (j.enabled) esforco = Mathf.Max(esforco, j.reactionForce.magnitude / j.breakForce);
        }
        visual.color = Color.Lerp(corNormal, Color.red, esforco);
    }

    public void Conectar(Node inicio, Node fim)
    {
        a = inicio;
        b = fim;

        Vector2 pa = inicio.transform.position;
        Vector2 pb = fim.transform.position;
        Vector2 direcao = pb - pa;

        transform.position = (pa + pb) / 2f;
        transform.right = direcao;
        transform.localScale = new Vector3(direcao.magnitude, 0.2f, 1f);
        visual.color = corNormal;

        Prender(juntas[0], inicio, -0.5f);
        Prender(juntas[1], fim, 0.5f);
    }

    void Prender(HingeJoint2D junta, Node ponto, float ponta)
    {
        junta.enabled = true;
        junta.autoConfigureConnectedAnchor = false;
        junta.anchor = new Vector2(ponta, 0f);
        junta.connectedBody = ponto.GetComponent<Rigidbody2D>();
        junta.connectedAnchor = Vector2.zero;
    }

    void OnJointBreak2D(Joint2D junta)
    {
        GameManager.instance.Tocar(somQuebra);
    }
}
