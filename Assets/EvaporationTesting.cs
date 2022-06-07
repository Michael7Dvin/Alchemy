using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    [SerializeField] private float x1 = -0.2927f;
    [SerializeField] private float x2 = 0.8742f;
    [SerializeField] private float x3 = 3.2522f;
    [SerializeField] private float x4 = 10.8372f;
    [SerializeField] private float x5 = 26.4639f;

    [SerializeField] private float x11 = 15f;
    [SerializeField] private float x22 = 30f;
    [SerializeField] private float x33 = 50f;
    [SerializeField] private float x44 = 75f;
    [SerializeField] private float x55 = 100f;

    private void Start()
    {
        //StartCoroutine(Temperature());
        //StartCoroutine(Evaporation());
        StartCoroutine(EFH());
    }

    IEnumerator Temperature()
    {
        Debug.Log(((0.0196f * Mathf.Pow(x1, 3)) - (0.8642f * Mathf.Pow(x1, 2)) + (12.2178f * x1) + 19.1307f));
        Debug.Log(((0.0196f * Mathf.Pow(x2, 3)) - (0.8642f * Mathf.Pow(x2, 2)) + (12.2178f * x2) + 19.1307f));
        Debug.Log(((0.0196f * Mathf.Pow(x3, 3)) - (0.8642f * Mathf.Pow(x3, 2)) + (12.2178f * x3) + 19.1307f));
        Debug.Log(((0.0196f * Mathf.Pow(x4, 3)) - (0.8642f * Mathf.Pow(x4, 2)) + (12.2178f * x4) + 19.1307f));
        Debug.Log(((0.0196f * Mathf.Pow(x5, 3)) - (0.8642f * Mathf.Pow(x5, 2)) + (12.2178f * x5) + 19.1307f));


        Debug.Log("////////////////////");
        Debug.Log("////////////////////");
        yield return new WaitForSeconds(1);
        StartCoroutine(Temperature());
    }

    IEnumerator Evaporation()
    {
        Debug.Log(((0.0001f * Mathf.Pow(x11, 3)) - (0.0032f * Mathf.Pow(x11, 2)) + (0.1227f * x11) - 0.8783f) / 3f);
        Debug.Log(((0.0001f * Mathf.Pow(x22, 3)) - (0.0032f * Mathf.Pow(x22, 2)) + (0.1227f * x22) - 0.8783f) / 3f);
        Debug.Log(((0.0001f * Mathf.Pow(x33, 3)) - (0.0032f * Mathf.Pow(x33, 2)) + (0.1227f * x33) - 0.8783f) / 3f);
        Debug.Log(((0.0001f * Mathf.Pow(x44, 3)) - (0.0032f * Mathf.Pow(x44, 2)) + (0.1227f * x44) - 0.8783f) / 3f);
        Debug.Log(((0.0001f * Mathf.Pow(x55, 3)) - (0.0032f * Mathf.Pow(x55, 2)) + (0.1227f * x55) - 0.8783f) / 3f);
        Debug.Log("////////////////////");
        Debug.Log("////////////////////");
        yield return new WaitForSeconds(1);
        StartCoroutine(Evaporation());
    }

    IEnumerator EFH()
    {
        float boil = ((0.0001f * Mathf.Pow(100, 3)) - (0.0032f * Mathf.Pow(100, 2)) + (0.1227f * 100) - 0.8783f) / 3f;
        float extra = ((0.0001f * Mathf.Pow(20, 3)) - (0.0032f * Mathf.Pow(20, 2)) + (0.1227f * 20) - 0.8783f) / 3f;
        float e120 = ((0.0001f * Mathf.Pow(120, 3)) - (0.0032f * Mathf.Pow(120, 2)) + (0.1227f * 120) - 0.8783f) / 3f;

        Debug.Log(boil);
        Debug.Log(extra);
        Debug.Log(e120 - boil);

        yield return new WaitForSeconds(1);
        StartCoroutine(EFH());
    }

}
