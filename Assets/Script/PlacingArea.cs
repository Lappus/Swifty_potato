using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Visual adjustments of the active placement area background
public class PlacingArea : MonoBehaviour
{
    public Image background;
    public TextMeshProUGUI backgroundtext;

    void Start()
    {
        // set the transparency of the background to 50%
        SetImageTransparency(0.5f);
    }

    public void SetImageTransparency(float alpha)
    {
        // background
        Color color = background.color; // current colour value 
        color.a = alpha; // alpha value
        background.color = color; // new colour value

        // and the same for the text
        Color textColor = backgroundtext.color;
        textColor.a = alpha;
        backgroundtext.color = textColor;
    }
}
