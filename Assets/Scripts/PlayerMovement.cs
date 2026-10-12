using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(x, y);
        movement.Normalize();

        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    // Detecta colisão com objetos coletáveis
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Coletavel")) // Verifica se o objeto colidido tem a tag "Coletavel"
        {
            audioSource.Play(); // Toca o som de coleta
            Destroy(other.gameObject); // Destroi o objeto coletável
        }
    }
}
