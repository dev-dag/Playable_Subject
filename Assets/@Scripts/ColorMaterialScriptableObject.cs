using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorMaterial", menuName = "ScriptableObjects/ColorMaterial", order = 1)]
public class ColorMaterialScriptableObject : ScriptableObject
{
    [SerializeField] private List<ColorMaterial> colorMaterials;

    /// <summary>
    /// 컬러 머터리얼을 검색하는 함수
    /// </summary>
    public ColorMaterial? GetColorMaterial(ObjectColor color)
    {
        foreach (var colorMaterial in colorMaterials)
        {
            if (colorMaterial.color == color)
            {
                return colorMaterial;
            }
        }

        return null;
    }

    /// <summary>
    /// 컬러 머터리얼을 검색하는 함수
    /// </summary>
    public ColorMaterial? GetColorMaterial(Material material)
    {
        foreach (var colorMaterial in colorMaterials)
        {
            if (colorMaterial.material == material)
            {
                return colorMaterial;
            }
        }

        return null;
    }
}