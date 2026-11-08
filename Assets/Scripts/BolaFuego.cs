using UnityEngine;

public class BolaFuego : MonoBehaviour
{
    public float tiempoDeVida = 3f; // Se destruye sola tras 3 segundos si no choca

    void Start()
    {
        Destroy(gameObject, tiempoDeVida);
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        // Si choca con un Tubo, se destruye la bola
        if (otro.GetComponent<Topo>() != null)
        {
            Destroy(gameObject);
        }
    }
}