using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Topo originalTopo;
    [SerializeField] private TextMesh scoreLabel;

    [Header("Configuración de la Cuadrícula")]
    public int gridRows = 2;
    public int gridCols = 4;
    public float header = 2f;
    public float margin = 1f;

    [Header("Temporizador")]

    public float tiempoPartida = 120f;
    private bool juegoTerminado = false;

    [Header("Objetos Aleatorios")]
    public GameObject[] objetosAleatoriosPrefabs;
    public float tiempoMinSpawn = 2f;
    public float tiempoMaxSpawn = 5f;

    private float limiteX;
    private float limiteY;

    private int _score = 0;

    void Start()
    {
        // Calculamos el tamaño de la pantalla según la cámara
        float totalheight = Camera.main.orthographicSize * 2;
        float totalwidth = totalheight * Camera.main.aspect;
        
        limiteX = (totalwidth/2) - margin;
        limiteY = (totalheight/2) - margin;

        GenerarCuadricula(totalwidth, totalheight);
        StartCoroutine(GenerarObjetos());
    }

    void Update()
    {
        if(!juegoTerminado)
        {
            tiempoPartida -= Time.deltaTime;

            if (scoreLabel!= null)
            {
                scoreLabel.text = "Tiempo: " + Mathf.Ceil(tiempoPartida).ToString();
            }

            if (tiempoPartida<=0)
            {
                FinDePartida();
            }
        }    
    }

    private void FinDePartida()
    {
        juegoTerminado = true;
        tiempoPartida = 0;

        Debug.Log("Tiempo agotado. FIN DE PARTIDA");
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
        float radioObjeto = 0.7f; // Tamaño del área libre que necesita el objeto

        // Bucle que intenta buscar una posición vacía varias veces
        while (!posicionValida && intentos < maxIntentos)
        {
            float posX = Random.Range(-limiteX, limiteX);
            float posY = Random.Range(-limiteY, limiteY);
            Vector2 posicionAleatoria = new Vector2(posX, posY);

            // Traza un círculo imaginario y detecta si choca con algún Collider2D (ej. los tubos)
            Collider2D colision = Physics2D.OverlapCircle(posicionAleatoria, radioObjeto);

            if (colision == null)
            {
                // Si colision devuelve null, significa que ese espacio está limpio
                posicionFinal = new Vector3(posX, posY, 0f);
                posicionValida = true;
            }

            intentos++;
        }

        // Solo instanciamos el objeto si realmente hemos encontrado un hueco libre
        if (posicionValida)
        {
            Instantiate(prefab, posicionFinal, Quaternion.identity);
        }
    }


    private void GenerarCuadricula(float totalwidth, float totalheight)
    {
        // Calculamos el espacio disponible para cada topo
        float spacewidth = (totalwidth - 2 * margin) / gridCols;
        float spaceheight = (totalheight - header - margin) / gridRows;

        int topoID = 0; // Un identificador único para cada agujero (opcional, pero útil)

        for (int j = 0; j < gridRows; j++)
        {
            for (int i = 0; i < gridCols; i++)
            {
                // Instanciamos el topo
                Topo topo = Instantiate(originalTopo);

                // Calculamos su posición en la cuadrícula
                float posX = (i + 0.5f) * spacewidth - totalwidth / 2 + margin;
                float posY = (j + 0.5f) * spaceheight - totalheight / 2 + margin;
                
                topo.transform.position = new Vector3(posX, posY, originalTopo.transform.position.z);
                
                // Nombramos el objeto en la jerarquía para mantener el orden
                topo.name = $"Topo_{topoID}";
                
                // Si tienes una función en tu script Topo para inicializarlo, la llamarías aquí.
                // Ejemplo: topo.Inicializar(topoID);

                topoID++;
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}