using MoreMountains.Tools;
using UnityEngine;

public class CursorController : MMSingleton<CursorController>
{
    public Texture2D DefaultCursor; // Assign your custom cursor texture in the Inspector
    public Vector2 hotSpot = Vector2.zero; // Defines the point within the texture that acts as the cursor's "point"

    void Start()
    {
        SetCursorTexture(DefaultCursor);
    }

    public void SetCursorTexture(Texture2D texture2D)
    {
        Cursor.SetCursor(texture2D, hotSpot, CursorMode.Auto); // Or CursorMode.ForceSoftware for software rendering
    }
}
