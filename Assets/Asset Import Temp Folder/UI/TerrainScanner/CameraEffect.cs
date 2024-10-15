using UnityEngine;

namespace TerrainScanner
{
    [ExecuteInEditMode]
    public class CameraEffect : MonoBehaviour
    {
        public Material material;
        public Material[] additionalMaterials;
        private Camera cameraForDepth;

        private void OnEnable()
        {
            cameraForDepth = GetComponent<Camera>();
            cameraForDepth.depthTextureMode = DepthTextureMode.Depth;
        }

        void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            material.SetMatrix("_ViewProjectInverse", (cameraForDepth.projectionMatrix * cameraForDepth.worldToCameraMatrix).inverse);
            Graphics.Blit(src, dest, material);

            if (additionalMaterials == null) { return; }
            for (int i = 0; i < additionalMaterials.Length; i++)
            {
                additionalMaterials[i].SetMatrix("_ViewProjectInverse", (cameraForDepth.projectionMatrix * cameraForDepth.worldToCameraMatrix).inverse);
                Graphics.Blit(src, dest, additionalMaterials[i]);
            }
        }
    }
}
