using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MaterialSwap
{
    public int materialIndex;
    public Material material1;
    public Material material2;
}

public class SwapMaterial : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    
    [Space(10)]
    
    [SerializeField] private List<MaterialSwap> materials;

    public void SwapMaterials(int materialIndex, bool reset)
    {
        Material[] mats = meshRenderer.materials;
        int meshIndex = materials[materialIndex].materialIndex;
        if (reset) mats[meshIndex] = materials[materialIndex].material1;
        else mats[materialIndex] = materials[materialIndex].material2;
        meshRenderer.materials = mats;
    }
    
    public void SwapAllMaterials(bool reset)
    {
        Material[] mats = meshRenderer.materials;
        foreach (var t in materials)
        {
            if (reset) mats[t.materialIndex] = t.material1;
            else mats[t.materialIndex] = t.material2;
        }
        meshRenderer.materials = mats;
    }

    public Material[] GetMaterials()
    {
        return meshRenderer.materials;
    }
}
