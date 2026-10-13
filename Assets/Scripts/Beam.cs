using UnityEngine;

public class Beam : MonoBehaviour
{
    public Node a;
    public Node b;

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

        HingeJoint2D[] juntas = GetComponents<HingeJoint2D>();
        Prender(juntas[0], inicio, -0.5f);
        Prender(juntas[1], fim, 0.5f);
    }

    void Prender(HingeJoint2D junta, Node ponto, float ponta)
    {
        junta.autoConfigureConnectedAnchor = false;
        junta.anchor = new Vector2(ponta, 0f);
        junta.connectedBody = ponto.GetComponent<Rigidbody2D>();
        junta.connectedAnchor = Vector2.zero;
    }
}
