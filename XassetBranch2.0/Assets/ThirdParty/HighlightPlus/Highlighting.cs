
using UnityEngine;

public class Highlighting : HighlightPlus.HighlightEffect
{

    private void Awake()
    {
        StartFlash();
    }


    public void StartFlash()
    {
        highlighted = true;
        outline = 1;
        outlineWidth = 1;
        outlineQuality = HighlightPlus.QualityLevel.Highest;
        outlineVisibility = HighlightPlus.Visibility.AlwaysOnTop;
        outlineColor = new Color(52F / 255F, 228F / 255f, 1);
        overlayMinIntensity = 0;
        overlay = 0;
        glowWidth = 0f;
        glow = 0;
        outlineDownsampling = 1;
        overlayColor = Color.white;
        overlayAnimationSpeed = 0;
    }

    public void CancelFlash()
    {
        highlighted = false;
    }
}
