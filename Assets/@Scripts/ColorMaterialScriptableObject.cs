using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 컬러와 머터리얼의 매핑 데이터를 제공하는 클래스
/// </summary>
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