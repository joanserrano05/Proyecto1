using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Topo originalTopo;

    [Header("Áreas de Juego (Ajustar en el Editor)")]
    public Vector2 tamanoCuadricula = new Vector2(14f, 7f); // NUEVO: Para separar los tubos (Recuadro Verde)
    public Vector2 tamanoMapa = new Vector2(12f, 7f);       // Para los objetos aleatorios (Recuadro Amarillo)
    public Vector2 centroMapa = new Vector2(0f, 0f);        // Mueve ambas zonas a la vez

    [Header("Configuración de la Cuadrícula")]
    public int gridRows = 3;
    public int gridCols = 3;

    [Header("Temporizador")]
    public float tiempoPartida = 120f;
    private bool juegoTerminado = false;

    [Header("Objetos Aleatorios (Estrella, Flores)")]
    public GameObject[] objetosAleatoriosPrefabs;
    public float tiempoMinSpawn = 2f;
    public float tiempoMaxSpawn = 5f;

    [Header("Enemigos en Tubos")]
    public GameObject[] enemigosPrefabs;
    public float tiempoMinSpawnEnemigo = 1f;
    public float tiempoMaxSpawnEnemigo = 3f;

    private float limiteX;
    private float limiteY;
    
    private List<Topo> listaTubos = new List<Topo>();

    void Start()
    {
        // Los límites de colisión se siguen basando en la zona de objetos aleatorios
        limiteX = tamanoMapa.x / 2f;
        limiteY = tamanoMapa.y / 2f;

        GenerarCuadricula();
        
        StartCoroutine(GenerarObjetos());
        StartCoroutine(GenerarEnemigosEnTubos());
    }

    void Update()
    {
        if(!juegoTerminado)
        {
            tiempoPartida -= Time.deltaTime;

            if (tiempoPartida <= 0)
            {
                FinDePartida();
            }
        }    
    }

    public void FinDePartida()
    {
        juegoTerminado = true;
        tiempoPartida = 0;
        Debug.Log("Tiempo agotado. FIN DE PARTIDA");
    }

    private IEnumerator GenerarEnemigosEnTubos()
    {
        while (!juegoTerminado)
        {
            float tiempoEspera = Random.Range(tiempoMinSpawnEnemigo, tiempoMaxSpawnEnemigo);
            yield return new WaitForSeconds(tiempoEspera);

            if (!juegoTerminado && enemigosPrefabs.Length > 0 && listaTubos.Count > 0)
            {
                int indiceTubo = Random.Range(0, listaTubos.Count);
                Topo tuboElegido = listaTubos[indiceTubo];

                int indiceEnemigo = Random.Range(0, enemigosPrefabs.Length);
                GameObject prefabEnemigo = enemigosPrefabs[indiceEnemigo];

                Instantiate(prefabEnemigo, tuboElegido.transform.position, Quaternion.identity);
            }
        }
    }

    private IEnumerator GenerarObjetos()
    {
        while (!juegoTerminado)
        {
            float tiempoEspera = Random.Range(tiempoMinSpawn, tiempoMaxSpawn);
            yield return new WaitForSeconds(tiempoEspera);
            if (!juegoTerminado && objetosAleatoriosPrefabs.Length > 0)
            {
                GenerarObjetosEnMapa();
            }
        }
    }

    private void GenerarObjetosEnMapa()
    {
        int indiceAleatorio = Random.Range(0, objetosAleatoriosPrefabs.Length);
        GameObject prefab = objetosAleatoriosPrefabs[indiceAleatorio];

        Vector3 posicionFinal = Vector3.zero;
        bool posicionValida = false;
        
        int intentos = 0;
        int maxIntentos = 15; 
        float radioObjeto = 0.7f; 

        while (!posicionValida && intentos < maxIntentos)
        {
            float posX = centroMapa.x + Random.Range(-limiteX, limiteX);
            float posY = centroMapa.y + Random.Range(-limiteY, limiteY);
            Vector2 posicionAleatoria = new Vector2(posX, posY);

            Collider2D colision = Physics2D.OverlapCircle(posicionAleatoria, radioObjeto);

            if (colision == null)
            {
                posicionFinal = new Vector3(posX, posY, 0f);
                posicionValida = true;
            }

            intentos++;
        }

        if (posicionValida)
        {
            Instantiate(prefab, posicionFinal, Quaternion.identity);
        }
    }

    private void GenerarCuadricula()
    {
        // AHORA USAMOS tamanoCuadricula PARA CALCULAR LA SEPARACIÓN DE LOS TUBOS
        float spacewidth = tamanoCuadricula.x / gridCols;
        float spaceheight = tamanoCuadricula.y / gridRows;
        int topoID = 0; 

        for (int j = 0; j < gridRows; j++)
        {
            for (int i = 0; i < gridCols; i++)
            {
                if (i == 1 && j == 1) continue; 

                Topo topo = Instantiate(originalTopo);
                
                float posX = centroMapa.x - (tamanoCuadricula.x / 2f) + (i + 0.5f) * spacewidth;
                float posY = centroMapa.y - (tamanoCuadricula.y / 2f) + (j + 0.5f) * spaceheight;
                
                topo.transform.position = new Vector3(posX, posY, originalTopo.transform.position.z);
                topo.name = $"Topo_{topoID}";
                
                listaTubos.Add(topo);
                topoID++;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Recuadro Amarillo: Área segura para que aparezcan las flores y estrellas
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(centroMapa, tamanoMapa);

        // Recuadro Verde: Área donde se distribuyen los tubos
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(centroMapa, tamanoCuadricula);
    }
}