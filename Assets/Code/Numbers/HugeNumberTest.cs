using System;
using System.Collections.Generic;
using UnityEngine;

// Just testing the hugenumber class
public class HugeNumberTest : MonoBehaviour
{

    

    private void Start()
    {
        // Test ConvertToExp
        // HugeNumber a = new HugeNumber(456, 3);
        // HugeNumber b = new HugeNumber(789, 6);
        //
        // HugeNumber a6 = HugeNumber.convertToExp(a, 6);
        // HugeNumber b4 = HugeNumber.convertToExp(b, 4);
        //
        // Debug.Log($"{a} ==> {a6}");
        // Debug.Log($"{b} ==> {b4}");
        
        // testing Adding
        HugeNumber a = new HugeNumber(456, 8);
        HugeNumber b = new HugeNumber(789, 6);
        HugeNumber sum = a.Add(b);
        
        Debug.Log($"{a} + {b} = {sum}");
        
        
        // testing mult
        HugeNumber c = new HugeNumber(560, 7);
        int mult = 2;
        HugeNumber multiplication = c.Mult(mult);
        
        Debug.Log($"{c} * {mult} = {multiplication}");
        
        
        
        // testing Subtract
        HugeNumber d = new HugeNumber(806, 8);
        HugeNumber e = new HugeNumber(405, 10);
        HugeNumber subtraction = d.Subtract(e);
        
        Debug.Log($"{d} - {e} = {subtraction}");
        
    }
}