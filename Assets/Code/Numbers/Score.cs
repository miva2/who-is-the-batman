using System;
using System.Collections.Generic;
using UnityEngine;

// Calculates and stores the main score
public class Score : MonoBehaviour
{
    [SerializeField]
    private double value;
    [SerializeField]
    private int exponent;

    private string scoreRepresentation;

    private Dictionary<int, string> numberNotation = new Dictionary<int, string>
    {
        { 0, "" },
        { 3, "k" },
        { 6, "M" },
        { 9, "G" },
        { 12, "T" },
        { 15, "P" },
        { 18, "E" },
        { 21, "Z" },
        { 24, "Y" },
        { 27, "R" },
        { 30, "Q" }
    };
    // how much the exponent increases in the dictionary
    private int step = 3;
    // max exponent in the dictionary
    private int max = 30;

    private void Update()
    {
        updateValue();
        calculateScoreRepresentation();
    }

    private void updateValue()
    {
        double threshold = Math.Pow(10, step) ;
        while (value >= threshold) // while loop in case updating multiple steps
        {
            // 679 k 
            // 1679 k => 1.679M
         // 12765 k ==> 12.765M   
         // 4.798.645 k ==> 4.798M
         value = value / threshold;
         exponent += step;

        }
    }
    
    private void calculateScoreRepresentation()
    {
        // okay to do this every frame?
        
        // int key = Math.DivRem(exponent, step, out int remainder);
        int remainder = exponent % step;
        int key = exponent - remainder;
        // Debug.Log($"divrem: exponent: {exponent}, step: {step}, key: {key}, remainder: {remainder}");

        //exponent % 3
        string letter = key > max? "∞" : numberNotation[key];
        string shownValue = value.ToString("0.##"); //TODO: calculate this
        scoreRepresentation = $"{shownValue} {letter}";
        Debug.Log($"scoreRepresentation: {scoreRepresentation}");
    }
}