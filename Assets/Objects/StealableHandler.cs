using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealableHandler : MonoBehaviour
{
    [System.Serializable]
    public struct Valuable
    {
        public GameObject ValObj;
        public double Worth;
    }


    public Valuable[] Valuables;
    public int WaitTime = 2;
    public bool stealing = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Steal(ThiefController thief)
    {
        if (tag == "Stealable" && !stealing)
        {
            stealing = true;
            StartCoroutine(StealCounter(WaitTime, thief));
        }
    }

    private IEnumerator StealCounter(float time, ThiefController thief)
    {
        StartCoroutine(IndicatorTimer(transform.GetChild(transform.childCount - 1).gameObject));
        foreach (Valuable val in Valuables)
        {
            if (stealing)
            {
                if (val.ValObj.activeSelf)
                {
                    yield return new WaitForSeconds(time);
                    if (stealing)
                    {
                        val.ValObj.SetActive(false);
                        GameManager.instance.AddWorth(val.Worth);
                    } else
                    {
                        break;
                    }
                }
            } else
            {
                break;
            }
        }
        if (stealing)
        {
            tag = "Untagged";
            stealing = false;
            thief.MoveToRandomValuable();
        }
    }

    private IEnumerator IndicatorTimer(GameObject Indicator)
    {
        Indicator.SetActive(true);
        yield return new WaitForSeconds(5);
        Indicator.SetActive(false);
    }
}
