using UnityEngine;

public class Puntuacion : MonoBehaviour
{
    public static Puntuacion Instancia;

    [Header("Referencias")]
    [SerializeField] private TextMesh textoPuntos; 
    public SceneController sceneController;

    private float puntuacionActual = 0f;

    [Header("Configuración Puntos")]
    public float puntosGoomba = 1f;
    public float puntosKoopa = 3f;
    public float puntosPlanta = 5f;
    public float puntosBomba = -5f;

    private void Awake()
    {
        Instancia = this;
    }

    private void Start()
    {
        ActualizarTexto();
    }

    public void ProcesarEnemigo(Enemigos.TipoEnemigo enemigo)
    {
        switch (enemigo)
        {
            case Enemigos.TipoEnemigo.Goomba: puntuacionActual += puntosGoomba; break;
            case Enemigos.TipoEnemigo.Koopa: puntuacionActual += puntosKoopa; break;
            case Enemigos.TipoEnemigo.Planta: puntuacionActual += puntosPlanta; break;
            case Enemigos.TipoEnemigo.Bomba: puntuacionActual += puntosBomba; break;
            case Enemigos.TipoEnemigo.Bowser: 
                if (sceneController != null) sceneController.FinDePartida(); 
                break;
        }
        ActualizarTexto();
    }

    private void ActualizarTexto()
    {
        if (textoPuntos != null) textoPuntos.text = "Puntos: " + puntuacionActual;
    }
}