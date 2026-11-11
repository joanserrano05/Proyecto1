using UnityEngine;
using System.Collections;

public class EfectosJugador : MonoBehaviour
{
    private movimiento_personaje movimiento;
    private SpriteRenderer spriteRenderer;
    private float velocidad;
    
    // 1. SOLUCIÓN: Declaramos las variables que faltaban
    private AtaqueJugador martillo;
    private float tiempoDeEspera; 

    [Header("Estado Actual")]
    public bool esInmune = false;
    public bool tieneFuego = false;

    [Header("Configuración Estrella")]
    public float multiplicadorVelocidad = 1.5f;
    public float duracionEstrella = 5f;

    [Header("Configuración Hielo")]
    public float duracionHielo = 2f;

    [Header("Configuración Fuego")]
    public float duracionFuego = 10f;
    public GameObject prefabBolaFuego;
    public float velocidadBolaFuego = 8f;
    
    void Start()
    {
        movimiento = GetComponent<movimiento_personaje>();
        martillo = GetComponent<AtaqueJugador>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        tiempoDeEspera = martillo.tiempoDeEspera;
        velocidad = movimiento.velocidad;
    }

    void Update()
    {
        if (tieneFuego && Input.GetMouseButtonDown(0))
        {
            DispararFuego();
        }
    }
    
    public void AplicarEfecto(objetos.TipoObjeto tipo) 
    {
        switch (tipo)
        {
            case objetos.TipoObjeto.Estrella:
                StartCoroutine(Estrella());
                break;
            case objetos.TipoObjeto.Hielo:
                if(!esInmune) StartCoroutine(Hielo());
                break;
            case objetos.TipoObjeto.Fuego:
                StartCoroutine(Fuego());
                break;
        }
    }

    private IEnumerator Estrella()
    {
        esInmune = true;
        movimiento.velocidad = velocidad * multiplicadorVelocidad;
        martillo.tiempoDeEspera = tiempoDeEspera / multiplicadorVelocidad;
        spriteRenderer.color = Color.yellow; 

        yield return new WaitForSeconds(duracionEstrella);

        esInmune = false;
        movimiento.velocidad = velocidad;
        // 2. SOLUCIÓN: Devolvemos la velocidad del martillo a la normalidad
        martillo.tiempoDeEspera = tiempoDeEspera; 
        spriteRenderer.color = Color.white; 
    }

    private IEnumerator Hielo()
    {
        movimiento.velocidad = 0f;
        spriteRenderer.color = Color.cyan;

        yield return new WaitForSeconds(duracionHielo);

        movimiento.velocidad = velocidad;
        spriteRenderer.color = Color.white;
    }

    private IEnumerator Fuego()
    {
        tieneFuego =  true;
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(duracionFuego);

        tieneFuego = false;
        spriteRenderer.color = Color.white;
    }

   private void DispararFuego()
    {
        if (prefabBolaFuego == null) return;

        Vector3 posicionRaton = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicionRaton.z = 0f;
        Vector2 direccion = (posicionRaton - transform.position).normalized;

        GameObject bola = Instantiate(prefabBolaFuego, transform.position, Quaternion.identity);
        Rigidbody2D rbBola = bola.GetComponent<Rigidbody2D>();
        
        if (rbBola != null)
        {
            rbBola.linearVelocity = direccion * velocidadBolaFuego;
        }
    }
}