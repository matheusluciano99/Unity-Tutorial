using UnityEngine;

public class Bandeira : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.GetComponentInParent<Carro>() != null) GameManager.instance.Venceu();
    }
}
