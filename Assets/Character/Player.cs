using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float PlayerSpeed;
    public Animator animator;
    private Rigidbody2D rb;
    public GameObject Flashlight;
    private PolygonCollider2D FlashlightCollider;
    private Vector2 playerDirection;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Flashlight = transform.GetChild(0).gameObject;
        FlashlightCollider = Flashlight.GetComponent<PolygonCollider2D>();
        GameManager.instance.player = this;
    }

    // Update is called once per frame
    void Update()
    {
        float directionX = Input.GetAxisRaw("Horizontal");
        float directionY = Input.GetAxisRaw("Vertical");

        playerDirection = new Vector2(directionX, directionY);
        animator.SetFloat("Horizontal", directionX);
        animator.SetFloat("Vertical", directionY);
        animator.SetFloat("Speed", playerDirection.sqrMagnitude);
        playerDirection = playerDirection.normalized;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3 MouseDirectionVec = mouseWorldPos - Flashlight.transform.position;
        float FlashlightAngle = Mathf.Atan2(MouseDirectionVec.y, MouseDirectionVec.x) * Mathf.Rad2Deg - 90;
        Flashlight.transform.rotation = Quaternion.Euler(0, 0, FlashlightAngle);

        
    }
    void FixedUpdate()
    {
        rb.velocity = new Vector2(playerDirection.x * PlayerSpeed, playerDirection.y * PlayerSpeed);

       
    }


}
