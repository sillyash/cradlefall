using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class BlurryFlicker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private SpriteRenderer rend;
    private int R, G, B;
    public Color colorfull;


    // Update is called once per frame
    void Update()
    {
        R = UnityEngine.Random.Range(0, 255);
        G = UnityEngine.Random.Range(0, 255);
        B = UnityEngine.Random.Range(0, 255);
        colorfull = Color(R, G, B, 255);
        rend = GetComponent<SpriteRenderer>();
        rend.color = colorfull;

    }
}
