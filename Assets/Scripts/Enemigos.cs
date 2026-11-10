using UnityEngine;

public class Enemigos : MonoBehaviour
{
    public enum TipoEnemigo { Goomba, Koopa, Planta, Bomba, Bowser }
    public TipoEnemigo tipoDeEnemigo;

    [Header("Configuración")]
    public float tiempoDeVida = 5f; 

    private void Start()
    {
        Destroy(gameObject, tiempoDeVida);
    }
}