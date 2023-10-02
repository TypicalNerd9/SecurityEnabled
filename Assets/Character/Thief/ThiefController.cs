using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;

public class ThiefController : MonoBehaviour
{

    public GameObject Target;
    public float CaughtTime = 5f;
    public TextMeshProUGUI CountdownObj;
    public Animator animator;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEnd = false;

    private Seeker seeker;
    private Rigidbody2D rb;
    private bool stuck = false;
    private bool running = false;
    private bool waiting = false;
    private float actualSpeed = 200f;
    private float ticksSinceSeen = 0;
    private bool seen = false;
    private bool inside = false;
    private float caughtTimer = 0;
    private bool evading = false;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        actualSpeed = speed;
        transform.GetChild(0).gameObject.SetActive(false);
        CountdownObj.gameObject.SetActive(false);
        MoveToRandomValuable();

    }

    public void MoveToRandomValuable()
    {
        GameObject[] Stealables = GameObject.FindGameObjectsWithTag("Stealable");
        if (Stealables.Length > 0)
        {
            Target = Stealables[Random.Range(0, Stealables.Length)];
            if (Target && seeker.IsDone())
                seeker.StartPath(rb.position, Target.transform.position, OnPathComplete);
        }
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
            stuck = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEnd = true;
            if (!running)
            {
                StealableHandler TargetSH = Target.GetComponent<StealableHandler>();
                if (TargetSH)
                {
                    TargetSH.Steal(this);
                }
            } else
            {
                if (seen)
                {
                    MoveToRandomPos();
                }
            }
            return;
        } else
        {
            reachedEnd = false;
        }
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * actualSpeed * Time.deltaTime;

        rb.AddForce(force);


        if (rb.velocity.sqrMagnitude < 0.00000001f)
        {
            if (!stuck)
            {
                stuck = true;
                StartCoroutine(StuckTimer(currentWaypoint));
            }
           
        }
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
            stuck = false;
        }
    }

    private IEnumerator StuckTimer(int LastCurrWaypoint)
    {
        yield return new WaitForSeconds(5);
        if (currentWaypoint != LastCurrWaypoint && rb.velocity.magnitude > 0.00000001f) stuck = false;
        else MoveToRandomValuable();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
       
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.gameObject.name == "Flashlight")
        {
            RaycastHit2D hit = Physics2D.Raycast(collision.transform.position, transform.position - collision.transform.position);
            if (hit.collider != null)
            {
                if (hit.collider.name == "Thief")
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    CountdownObj.gameObject.SetActive(true);
                    if (!running)
                    {
                        AudioSource music = collision.transform.parent.GetChild(3).GetComponent<AudioSource>();
                        AudioSource fastMusic = collision.transform.parent.GetChild(4).GetComponent<AudioSource>();
                        music.Stop();
                        fastMusic.Play();
                        StealableHandler TargetSH = Target.GetComponent<StealableHandler>();
                        if (TargetSH)
                        {
                            TargetSH.stealing = false;
                        }
                        running = true;
                        actualSpeed = speed * 3;
                        MoveToRandomPos();
                    }
                    if (!seen)
                    {
                        seen = true;
                        ticksSinceSeen = 0;
                    }
                }
            }

        } else if (collision.gameObject.name == "SecCameraCollision")
        {
            RaycastHit2D hit = Physics2D.Raycast(collision.transform.position, transform.position - collision.transform.position);
            if (hit.collider != null)
            {
                if (hit.collider.name == "Thief")
                {
                    GameObject Indicator = collision.transform.parent.GetChild(3).gameObject;
                    if (!Indicator.activeSelf) StartCoroutine(IndicatorTimer(Indicator));
                    transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Flashlight")
        {
            transform.GetChild(0).gameObject.SetActive(false);
            CountdownObj.gameObject.SetActive(false);
            inside = false;
            seen = false;
            caughtTimer = 0;
            AudioSource music = collision.transform.parent.GetChild(3).GetComponent<AudioSource>();
            AudioSource fastMusic = collision.transform.parent.GetChild(4).GetComponent<AudioSource>();
            fastMusic.Stop();
            music.Play();
        }
        else if (collision.gameObject.name == "SecCameraCollision")
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private IEnumerator StopFastMusic(float time)
    {
        yield return new WaitForSeconds(time);
    }
    private void Update()
    {
        if (running && !seen) ticksSinceSeen += Time.deltaTime;

        if (ticksSinceSeen > 5)
        {
            running = false;
            MoveToRandomValuable();
            ticksSinceSeen = 0;
            actualSpeed = speed;
        }

        if (seen) caughtTimer += Time.deltaTime;

        if (caughtTimer > CaughtTime)
        {
            GameManager.instance.EndGame(true);
        } else
        {
            CountdownObj.text = (CaughtTime - caughtTimer).ToString("0");
            /*if (caughtTimer > CaughtTime/2)
            {
                if (!evading)
                {
                    evading = true;
                    StartCoroutine(EvasiveManuevers());
                }
            }*/
        }


        
    }

    public void MoveToRandomPos()
    {
        Vector2 point = Random.insideUnitCircle * 5f;
        point += rb.position;
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, point, OnPathComplete);
        }
    }

    private IEnumerator IndicatorTimer(GameObject Indicator)
    {
        Indicator.SetActive(true);
        yield return new WaitForSeconds(5);
        Indicator.SetActive(false);
    }

    /*private IEnumerator EvasiveManuevers()
    {
        Debug.Log("EVADE");
        MoveToRandomPos();
        yield return new WaitForSeconds(2);
        evading = false;
    }*/
}
