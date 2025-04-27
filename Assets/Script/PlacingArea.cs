using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlacingArea : MonoBehaviour
{
    public Image background;
    public TextMeshProUGUI backgroundtext;

    void Start()
    {
        // Setze die Durchsichtigkeit auf 50%
        SetImageTransparency(0.5f);
    }

    public void SetImageTransparency(float alpha)
    {
        Color color = background.color; // Aktuellen Farbwert holen
        color.a = alpha; // Alpha-Wert setzen
        background.color = color; // Neuen Farbwert anwenden

        Color textColor = backgroundtext.color;
        textColor.a = alpha;
        backgroundtext.color = textColor;
    }
}
