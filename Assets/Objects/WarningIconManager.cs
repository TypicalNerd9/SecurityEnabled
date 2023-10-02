using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningIconManager : MonoBehaviour
{
    public GameObject WarningSprite;
    public GameObject Target;
    private bool outOfView = false;
    public Renderer rd;
    Vector2 OriginalPos;
    // Start is called before the first frame update
    void Start()
    {
        OriginalPos = WarningSprite.transform.position;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Player player = GameManager.instance.player;
        if (player) {
            if (!rd.isVisible)
            {
                outOfView = true;
                Vector2 direction = player.transform.position - transform.position;
                int layerMask = 1 << 7;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, layerMask);
               

                if (hit.collider != null)
                {
                    WarningSprite.transform.position = hit.point + (direction.normalized/4);
                }
            } else
            {
                if (outOfView)
                {
                    outOfView = false;
                    WarningSprite.transform.position = OriginalPos;
                }
            }
        }
    }
}
