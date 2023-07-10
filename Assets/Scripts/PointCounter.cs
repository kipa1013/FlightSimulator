using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PointCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointCanvas;
    private int points;

    public int Points
    {
        get { return points; }
    }

    private int highscore;

    public int Highscore
    {
        get { return highscore; }
    }

    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        highscore = 0;
        Collider collider = GetComponent<Collider>();
        Debug.Log("Started");
    }


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Pomts+1");
        GameObject targetObject = other.gameObject;
        Target target = targetObject.GetComponent<Target>();
        if (target == null)
        {
            return;
        }

        points += target.PointsAdded;
        targetObject.SetActive(false);
        pointCanvas.text = "Points:" + points;
        Debug.Log("Points: " + points);
    }

    // Update is called once per frame
    void Update()
    {
    }
}