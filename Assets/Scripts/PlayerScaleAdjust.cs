using UnityEngine;

public class PlayerScaleAdjust : MonoBehaviour
{
    [SerializeField] AnimationCurve _scaleCurve;
    [SerializeField] Transform minY;
    [SerializeField] Transform maxY;
    [SerializeField] Transform player; // player to scale

    [SerializeField] float minScale, maxScale;

    void Update()
    {
        adjustPlayerScale();
    }

    void adjustPlayerScale()
    {
        // 1️⃣ Get how far the player is between minY and maxY (as a normalized 0–1 value)
        float t = Mathf.InverseLerp(minY.position.y, maxY.position.y, player.position.y);


        float scaleValue = Mathf.Lerp(minScale, maxScale, _scaleCurve.Evaluate(t));

        // 3️⃣ Apply it to player’s scale
        player.localScale = Vector3.one * scaleValue;
    }
}
