using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.UI;

public class Fairy :MonoBehaviour
{

    private static Fairy instance;
    public static Fairy Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Fairy>();
            }
            return instance;
        }
    }

    bool canUse = true;
    float nearDistance = 5.0f;
    float speed = 3;
    UnityAction doAction;
    Vector3 targetPosition;
    private float counter;
    private float actionTime = 10.0f;

    private void Start()
    {
        targetPosition = Player.Instance.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (!canUse)
        {
            counter += Time.deltaTime;
        }
        if (counter >= actionTime)
        {

        }
    }

    private void Move()
    {
        if (canUse)
        {
            targetPosition = Player.Instance.transform.position;
        }

        if (Vector3.Distance(transform.position, targetPosition) < nearDistance)
        {
            return;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    public bool getCanUse()
    {

        return canUse;
    }

    public void setCanUse(bool c)
    {
        canUse = c;
    }

    public void setTargetPosition(Vector3 pos)
    {
        targetPosition = pos;
    }
}

