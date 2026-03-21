using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class CircleTransition : Image
{
    public override Material materialForRendering
    {
        get
        {
            Material material = new Material(base.materialForRendering);    // Get current material attached to the image
            material.SetInt("_StencilComp", (int)CompareFunction.NotEqual); // 
            return material;
        }
    }
}
