using UnityEngine;

public class AtaqueJugador : MonoBehaviour
{
    [Header("Configuración del Martillo")]
    public float radioDeGolpe = 1.5f;
    public KeyCode teclaAtaque = KeyCode.Space;

    [Header("Visuales")]
    public Animator animador; // Referencia para controlar la animación

    void Update()
    {
        if (Input.GetKeyDown(teclaAtaque))
        {
            DarMartillazo();
        }        
    }

    private void DarMartillazo()
    {
        // 1. Reproducir la animación visual del golpe
        if (animador != null)
        {
            animador.SetTrigger("Atacar");
        }

        // 2. Lógica de físicas (detectar a los topos)
        Collider2D[] objetoGolpeado = Physics2D.OverlapCircleAll(transform.position, radioDeGolpe);

        foreach (Collider2D objeto in objetoGolpeado)
        {
            Topo topoDetectado = objeto.GetComponent<Topo>();

            if (topoDetectado != null)
            {
                topoDetectado.RecibirGolpe();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeGolpe);
    }
}