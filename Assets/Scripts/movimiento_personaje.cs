using UnityEngine;

public class movimiento_personaje : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad = 5f;

    private Rigidbody2D rb;
    private Vector2 movimiento;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); 
    }

    void Update()
    {
        movimiento.x = Input.GetAxisRaw("Horizontal");
        movimiento.y = Input.GetAxisRaw("Vertical");

        anim.SetFloat("Velocidad", movimiento.sqrMagnitude);

        if (movimiento != Vector2.zero)
        {
            anim.SetFloat("MovX", movimiento.x);
            anim.SetFloat("MovY", movimiento.y);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movimiento.normalized * velocidad * Time.fixedDeltaTime);
    }
}