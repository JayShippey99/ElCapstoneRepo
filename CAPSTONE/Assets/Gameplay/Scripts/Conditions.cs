using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

static public class Conditions
{
    static public bool IsAlternating(string t)
    {
        // get first and second input in the string
        if (t.Length < 2) // meaning if there's only one char
        {
            return false;
        }

        char[] cArr = t.ToCharArray();
        char c1 = cArr[0];
        char c2 = cArr[1];

        /*
        if (c1 == c2) // add this if you want them to be different
        {
            return false;
        }
        */

        // loop through and for each other one check if it matches c1 
        for (int i = 0; i < cArr.Length; i++)
        {
            if (i % 2 == 0) // this means every other starting with the first
            {
                if (cArr[i] != c1) return false;
            }
            else
            {
                if (cArr[i] != c2) return false;
            }
        }

        return true;
    }
    static public bool IsInAscendingOrder(string t)
    {
        char[] cStr = t.ToCharArray();

        for (int i = 0; i < cStr.Length - 1; i++)
        {
            if (cStr[i] > cStr[i + 1])
            {
                return false;
            }
        }

        return true;
    }

    static public bool IsMathSymbol(char c)
    {
        if (c == '+') return true;
        if (c == '-') return true;
        if (c == '*') return true;
        if (c == '/') return true;
        return false;
    }
    static public bool IsThisChar(char yourC, char testC)
    {
        return yourC == testC;
    }

    static public bool IsNumber(char c)
    {
        return char.IsNumber(c);
    }

    static public bool IsLetter(char c)
    {
        return char.IsLetter(c);
    }

    static public bool IsCapitalLetter(char c)
    {
        return char.IsUpper(c);
    }

    static public bool IsSymbol(char c)
    {
        // symbol means not letter and not number and not space
        if (!char.IsLetter(c) && !char.IsNumber(c) && !char.IsWhiteSpace(c)) return true;

        return false;
    }

    static public bool IsThisIndexANumber(string str, int n)
    {
        char[] charstr = str.ToCharArray();
        return char.IsNumber(charstr[n]);
    }
    static public bool AreValuesMatching(char str1, string str2)
    {
        char[] c = str2.ToCharArray();
        return str1 == c[0];
    }

    static public bool IsHigherValueThan(char str1, string str2)
    {

        char[] c = str2.ToCharArray();
        //Debug.Log(c[0] + " ayooo");
        return str1 > c[0];
    }

    static public bool IsLowerValueThan(char str1, string str2)
    {
        char[] c = str2.ToCharArray();
        return str1 < c[0];
    }

    static public int HowManyNumbers(string str)
    {
        int count = 0;

        char[] charstr = str.ToCharArray();

        for (int i = 0; i < charstr.Length; i++)
        {
            char c = charstr[i];
            if (char.IsNumber(c)) count++;
        }

        return count;
    }
    static public int HowManyUpperCase(string str)
    {
        int count = 0;

        char[] charstr = str.ToCharArray();

        for (int i = 0; i < charstr.Length; i++)
        {
            char c = charstr[i];
            if (char.IsUpper(c)) count++;
        }

        return count;
    }
    static public int HowManyLowerCase(string str)
    {
        int count = 0;

        char[] charstr = str.ToCharArray();

        for (int i = 0; i < charstr.Length; i++)
        {
            char c = charstr[i];
            if (char.IsLower(c)) count++;
        }

        return count;
    }
    static public int HowManySymbols(string str)
    {
        int count = 0;

        char[] charstr = str.ToCharArray();

        for (int i = 0; i < charstr.Length; i++)
        {
            char c = charstr[i];
            if (!char.IsNumber(c) && !char.IsLetter(c)) count++;
        }

        return count;
    }

    static public bool HasThisChar(string str, string c)
    {
        return str.Contains(c);
    }

    static public bool HasThisString(string str, string strTarget)
    {
        return false;
    }

    static public bool IsAllLetters(string str)
    {
        return false;
    }

    static public bool IsAllNumbers(string str)
    {
        return false;
    }

    static public bool HasLetter(string str)
    {
        return str.Any(char.IsLetter);
    }

    static public bool HasUpper(string str)
    {
        return str.Any(char.IsUpper);
    }

    static public bool HasLower(string str)
    {
        return str.Any(char.IsLower);
    }

    static public bool HasNumber(string str)
    {
        return str.Any(char.IsNumber);
    }

    static public bool HasSymbol(string str) // Symbol is anything that's not letter or number for now
    {
        bool hasSymbol = false;

        char[] charstr = str.ToLower().ToCharArray();

        for (int i = 0; i < charstr.Length; i++)
        {
            if (!char.IsLetter(charstr[i]) && !char.IsNumber(charstr[i])) hasSymbol = true; // COULD I JUST ASK IF ITS NOT LETTERORDIGIT?
        }

        return hasSymbol;
    }

    static public bool IsItThisLong(string str, int n)
    {
        return str.Length == n;
    }

    static public bool IsLongerThan(string str, int n)
    {
        return str.Length > n;
    }

    static public bool IsShorterThan(string str, int n)
    {
        return str.Length < n;
    }
}

