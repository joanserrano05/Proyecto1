using UnityEngine;
using System.Collections; // IMPORTANTE: Necesario para usar IEnumerator

public class AtaqueJugador : MonoBehaviour
{
    [Header("Configuración del Martillo")]
    public float radioDeGolpe = 1.5f;
    public KeyCode teclaAtaque = KeyCode.Space;
    public float tiempoDeEspera = 1f; // El tiempo de pausa tras atacar

    private Animator animador; 
    private movimiento_personaje scriptMovimiento; 
    private bool puedeAtacar = true; // Controla que no spamee el botón

    void Start()
    {
        animador = GetComponent<Animator>();
        scriptMovimiento = GetComponent<movimiento_personaje>();
    }

    void Update()
    {
        // Solo ataca si pulsas Espacio Y el temporizador ha terminado
        if (Input.GetKeyDown(teclaAtaque) && puedeAtacar)
        {
            StartCoroutine(RutinaDeAtaque());
        }        
    }

    // Esta es la Corrutina: Permite hacer pausas en el tiempo
    private IEnumerator RutinaDeAtaque()
    {
        puedeAtacar = false; // Bloqueamos el ataque
        
        // OPCIONAL: Guardamos la velocidad normal y paramos al personaje
        float velocidadOriginal = 5f;
        if (scriptMovimiento != null)
        {
            velocidadOriginal = scriptMovimiento.velocidad;
            scriptMovimiento.velocidad = 0f;
        }

        // Ejecutamos el golpe y las físicas
        DarMartillazo();

        // Esperamos 1 segundo exacto
        yield return new WaitForSeconds(tiempoDeEspera);

        // Devolvemos la velocidad a la normalidad
        if (scriptMovimiento != null)
        {
            scriptMovimiento.velocidad = velocidadOriginal;
        }

        puedeAtacar = true; // Desbloqueamos el ataque para el siguiente golpe
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