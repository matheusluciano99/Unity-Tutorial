using UnityEngine;

public class Carro : MonoBehaviour
{
    public WheelJoint2D[] rodas;
    public float velocidade = 600f;

    private Rigidbody2D[] corpos;
    private Vector3[] posicoes;
    private Quaternion[] rotacoes;
    private bool dirigindo;

    void Awake()
    {
        corpos = GetComponentsInChildren<Rigidbody2D>();
        posicoes = new Vector3[corpos.Length];
        rotacoes = new Quaternion[corpos.Length];
        for (int i = 0; i < corpos.Length; i++)
        {
            posicoes[i] = corpos[i].transform.position;
            rotacoes[i] = corpos[i].transform.rotation;
        }
        Voltar();
    }

    void Update()
    {
        if (!dirigindo) return;

        // Velocidade negativa no motor gira a roda no sentido horário, levando o carro para a direita.
        float eixo = Input.GetAxis("Horizontal");
        foreach (WheelJoint2D roda in rodas)
        {
            JointMotor2D motor = roda.motor;
            motor.motorSpeed = -eixo * velocidade;
            roda.motor = motor;
        }
    }

    public void Ligar()
    {
        dirigindo = true;
        foreach (Rigidbody2D rb in corpos) rb.simulated = true;
    }

    // Congela o carro e devolve ele para a largada.
    public void Voltar()
    {
        dirigindo = false;
        for (int i = 0; i < corpos.Length; i++)
        {
            corpos[i].simulated = false;
            corpos[i].linearVelocity = Vector2.zero;
            corpos[i].angularVelocity = 0f;
            corpos[i].transform.SetPositionAndRotation(posicoes[i], rotacoes[i]);
        }
    }

    public float Altura()
    {
        return corpos[0].transform.position.y;
    }
}
