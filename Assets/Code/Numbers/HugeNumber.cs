using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class HugeNumber
{
    [SerializeField]
    private string formattedNumber;
    [SerializeField]
    private string format = "0.##";

    [SerializeField]
    private double value = 0;
    
    [SerializeField]
    private int exponent = 1;
    
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
    [SerializeField] private int step = 3;
    // max exponent in the dictionary
    [SerializeField] private int max = 30;
    [SerializeField] private string maxExceededLetter = "∞";



    public HugeNumber( double value, int exponent ,string format = "0.##")
    {
        this.value = value;
        this.exponent = exponent;
        this.format = format;
        
        UpdateValue();
    }

    public HugeNumber Add(HugeNumber otherNumber)
    {
        int targetExp = Mathf.Max(exponent, otherNumber.exponent);
        HugeNumber convertedOther = ConvertToExp(otherNumber, targetExp);
        HugeNumber convertedThis = ConvertToExp(this, targetExp);

        double newValue = convertedThis.value + convertedOther.value;
        int newExponent = targetExp;
        return new HugeNumber(newValue, newExponent);
    }

    public HugeNumber Mult(double multiplier)
    {
        return new HugeNumber(value * multiplier, exponent);
    }

    public HugeNumber Subtract(HugeNumber otherNumber)
    {
        int targetExp = Mathf.Max(exponent, otherNumber.exponent);
        HugeNumber convertedOther = ConvertToExp(otherNumber, targetExp);
        HugeNumber convertedThis = ConvertToExp(this, targetExp);
        
        double newValue = convertedThis.value - convertedOther.value; // fix this
        int newExponent = targetExp;
        return new HugeNumber(newValue, newExponent);
    }
    
    public void UpdateValue()
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
    
    private string FormatNumber(string format)
    {
        // okay to do this every frame?
        int remainder = exponent % step;
        int key = exponent - remainder;

        string letter = key > max? maxExceededLetter : numberNotation[key];
        string shownValue = value.ToString(format); 
        return $"{shownValue} {letter}";
    }

    public static HugeNumber ConvertToExp(HugeNumber number, int targetExp)
    {
        // 357.86 exp 4
        // ==> 3.5786 exp 6
        
        int expDiff = targetExp - number.exponent; // can be negative if target is lower

        double newValue = number.value * Mathf.Pow(10, -expDiff);
        int newExponent = targetExp;

        return new HugeNumber(newValue, newExponent);
    }

    public override string ToString()
    {
        return $"{{val: {value} exp: {exponent}}}";
    }
}