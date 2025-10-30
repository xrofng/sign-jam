using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "AllDecorations", menuName = "SignGame/All Decorations")]
public class AllDecorationsSO : ScriptableObject
{
    [Tooltip("All DecorationSO assets found in the project.")]
    [ListDrawerSettings(ShowPaging = true, DraggableItems = false)]
    public List<DecorationSO> Decorations = new List<DecorationSO>();

    public List<string> Names = new List<string>();
    public List<HouseColorSO> HouseColors = new List<HouseColorSO>();

    public DecorationSO GetRandomDecoration()
    {
        return Decorations[Random.Range(0, Decorations.Count)];
    }

    public string GetRandomName()
    {
        return Names[Random.Range(0, Names.Count)];
    }

    public string GetRandomHouseColorId()
    {
        return HouseColors[Random.Range(0, HouseColors.Count)].PaletteId;
    }


#if UNITY_EDITOR
    /// <summary>
    /// Finds all DecorationSO assets in the project that have a BasePrefab and Textures assigned.
    /// </summary>
    [Button("Find All Valid Decorations", ButtonSizes.Large)]
    private void FindAllDecorations()
    {
        Decorations.Clear();

        // Search all assets of type DecorationSO in the project
        string[] guids = AssetDatabase.FindAssets("t:DecorationSO");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            DecorationSO deco = AssetDatabase.LoadAssetAtPath<DecorationSO>(path);

            if (deco != null && deco.BasePrefab != null && deco.Textures != null && deco.Textures.Length > 0)
            {
                Decorations.Add(deco);
            }
        }

        // Sort alphabetically by DecorationID (optional)
        Decorations = Decorations.OrderBy(d => d.DecorationID).ToList();

        EditorUtility.SetDirty(this);
        Debug.Log($"[AllDecorationsSO] Found {Decorations.Count} valid DecorationSO assets.");
    }
#endif
}
