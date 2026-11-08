using UnityEngine;

public class objetos : MonoBehaviour
{
    public enum TipoObjeto {Fuego, Hielo, Estrella}
    public TipoObjeto tipoDeObjeto;

    private void OnTriggerEnter2D(Collider2D otro)
    {
        EfectosJugador jugador = otro.GetComponent<EfectosJugador>();

        if (jugador != null)
        {
            jugador.AplicarEfecto(tipoDeObjeto);
            Destroy(gameObject);
        }
    }
}
