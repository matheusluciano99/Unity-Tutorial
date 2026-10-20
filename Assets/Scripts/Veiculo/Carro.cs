using UnityEngine;

public class Carro : MonoBehaviour
{
    public WheelJoint2D[] rodas;
    public float velocidade = 600f;

    private Rigidbody2D[] corpos;
    private Vector3[] posicoes;
    private Quaternion[] rotacoes;

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

    // Liga a física e acelera para a frente até o fim do teste.
    public void Ligar()
    {
        foreach (Rigidbody2D rb in corpos) rb.simulated = true;

        // Velocidade negativa no motor gira a roda no sentido horário, levando o carro para a direita.
        foreach (WheelJoint2D roda in rodas)
        {
            JointMotor2D motor = roda.motor;
            motor.motorSpeed = -velocidade;
            roda.motor = motor;
        }
    }

    // Congela o carro e devolve ele para a largada.
    public void Voltar()
    {
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
