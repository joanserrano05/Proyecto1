using UnityEngine;

public class AtaqueJugador : MonoBehaviour
{
    [Header("Configuración del Martillo")]
    public float radioDeGolpe = 1.5f;
    public KeyCode teclaAtaque = KeyCode.Space;

    private Animator animador; 

    void Start()
    {
        animador = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(teclaAtaque))
        {
            DarMartillazo();
        }        
    }

    private void DarMartillazo()
    {
        if (animador != null)
        {
            animador.SetTrigger("Atacar");
        }

        Collider2D[] objetoGolpeado = Physics2D.OverlapCircleAll(transform.position, radioDeGolpe);

        foreach (Collider2D objeto in objetoGolpeado)
        {
            Topo topoDetectado = objeto.GetComponent<Topo>();
            if (topoDetectado != null) topoDetectado.RecibirGolpe();

            Enemigos enemigoGolpeado = objeto.GetComponent<Enemigos>();
            if (enemigoGolpeado != null)
            {
                Puntuacion.Instancia.ProcesarEnemigo(enemigoGolpeado.tipoDeEnemigo);
                Destroy(enemigoGolpeado.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeGolpe);
    }
}