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
        
        
        // testing bigger than
        HugeNumber bigger1 = new HugeNumber(806, 8);
        HugeNumber bigger2 = new HugeNumber(405, 10);
        bool isBiggerThan12 = bigger1.IsBiggerThan(bigger2);
        bool isBiggerThan21 = bigger2.IsBiggerThan(bigger1);
        
        Debug.Log($"{bigger1} > {bigger2} = {isBiggerThan12}");
        Debug.Log($"{bigger2} > {bigger1} = {isBiggerThan21}");
        
        // testing bigger than second case
        HugeNumber bigger3 = new HugeNumber(208, 3);
        HugeNumber bigger4 = new HugeNumber(985, 3);
        bool isBiggerThan34 = bigger3.IsBiggerThan(bigger4);
        bool isBiggerThan43 = bigger4.IsBiggerThan(bigger3);
        
        Debug.Log($"{bigger3} > {bigger4} = {isBiggerThan34}");
        Debug.Log($"{bigger4} > {bigger3} = {isBiggerThan43}");
        
        
        // testing smaller than
        HugeNumber smaller1 = new HugeNumber(806, 8);
        HugeNumber smaller2 = new HugeNumber(405, 10);
        bool isSmallerThan12 = smaller1.IsSmallerThan(smaller2);
        bool isSmallerThan21 = smaller2.IsSmallerThan(smaller1);
        
        Debug.Log($"{smaller1} < {smaller2} = {isSmallerThan12}");
        Debug.Log($"{smaller2} < {smaller1} = {isSmallerThan21}");
        
        // testing smaller than second case
        HugeNumber smaller3 = new HugeNumber(208, 3);
        HugeNumber smaller4 = new HugeNumber(985, 3);
        bool isSmallerThan34 = smaller3.IsSmallerThan(smaller4);
        bool isSmallerThan43 = smaller4.IsSmallerThan(smaller3);
        
        Debug.Log($"{smaller3} < {smaller4} = {isSmallerThan34}");
        Debug.Log($"{smaller4} < {smaller3} = {isSmallerThan43}");
        
        //test lerp
        HugeNumber num1 = new HugeNumber(100, 8);
        HugeNumber num2 = new HugeNumber(100, 9);
        
        HugeNumber lerp = HugeNumber.Lerp(num1, num2, 0.5f);
        Debug.Log($"lerp : {lerp}");
        
    }
}