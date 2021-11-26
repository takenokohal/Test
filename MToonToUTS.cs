using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace VRM
{
    public static class MToonToUTS
    {
        private const string ShaderNameMToon = "VRM/MToon";

        private const string ShaderNameUTS = "Universal Render Pipeline/Toon";

        public static void Change(GameObject target)
        {
            var renderers = target.GetComponentsInChildren<SkinnedMeshRenderer>();


            foreach (var skinnedMeshRenderer in renderers)
            {
                var materials = skinnedMeshRenderer.materials;

                foreach (var material in materials)
                {
                    if (material.shader.name != ShaderNameMToon)
                        continue;


                    SetMaterial(material);
                }
            }
        }


        private static void SetMaterial(Material material)
        {
            Texture mainTex = material.GetTexture("_MainTex");
            Texture ShadeMap_1st = material.GetTexture("_ShadeTexture");
            Texture normalMap = material.GetTexture("_BumpMap");

            Texture matCap = material.GetTexture("_SphereAdd");
            Texture emissionMap = material.GetTexture("_EmissionMap");

            Color mainColor = material.GetColor("_Color");
            Color shadeColor = material.GetColor("_ShadeColor");

            Color emissionColor = material.GetColor("_EmissionColor");
            float outlineWidth = material.GetFloat("_OutlineWidth");
            Color outlineColor = material.GetColor("_OutlineColor");

            var isTransparent = material.GetTag("RenderType", false) == "Transparent";

            material.shader = Shader.Find(ShaderNameUTS);

            material.SetFloat("_ClippingMode", 1.0f);
            material.SetTexture("_MainTex", mainTex);
            material.SetTexture("_1st_ShadeMap", ShadeMap_1st);
            material.SetTexture("_NormalMap", normalMap);
            material.SetColor("_BaseColor", mainColor);
            material.SetColor("_1st_ShadeColor", shadeColor);


            material.SetTexture("_MatCap_Sampler", matCap);
            material.SetTexture("_Emissive_Tex", emissionMap);
            material.SetColor("_Emissive_Color", emissionColor);

            material.SetFloat("_Outline_Width", outlineWidth);
            material.SetColor("_Outline_Color", outlineColor);
            if (isTransparent)
            {
                material.SetFloat("_TransparentEnabled", 1);
                material.SetFloat("_IsBaseMapAlphaAsClippingMask", 1);
            }
        }
    }
}