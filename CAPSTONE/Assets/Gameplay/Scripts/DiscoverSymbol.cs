using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscoverSymbol : MonoBehaviour
{
    public SymbolChanger sc; // aaaaa i hate this I don't think I need this
    public char c;

    Image img;
    void Start()
    {
        img = GetComponent<Image>();
        img.color = Color.clear;
    }

    public void Reveal()
    {
        img.color = Color.white;
    }
}
