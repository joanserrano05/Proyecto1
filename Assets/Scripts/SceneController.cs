using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    private int _score = 0;

    void Start()
    {
        // Calculamos el tamaño de la pantalla según la cámara
        float totalheight = Camera.main.orthographicSize * 2;
        float totalwidth = totalheight * Camera.main.aspect;
        
        GenerarCuadricula(totalwidth, totalheight);
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