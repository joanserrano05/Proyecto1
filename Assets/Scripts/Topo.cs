using UnityEngine;

public class Topo : MonoBehaviour
{
    public void RecibirGolpe()
    {
        Debug.Log("¡Has golpeado al " + gameObject.name + "!");
        
        // Más adelante, aquí añadiremos:
        // 1. Sumar puntos al GameManager
        // 2. Reproducir sonido de golpe
        // 3. Esconder el topo de nuevo bajo tierra
    }
}