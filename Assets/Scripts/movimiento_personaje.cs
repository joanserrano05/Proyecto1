using UnityEngine;

public class movimiento_personaje : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad =5f;

    private Rigidbody2D rb;
    private Vector2 movimiento;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movimiento.x = Input.GetAxisRaw("Horizontal");
        movimiento.y = Input.GetAxisRaw("Vertical");

        movimiento = movimiento.normalized;        
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movimiento * velocidad *Time.fixedDeltaTime);
    }
}
