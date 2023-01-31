using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sebastian
{
    public class MapDisplay : MonoBehaviour
    {
        public Renderer textureRender;
        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;

        public UnityEngine.UI.RawImage rawImage;

        public void DrawTexture(Texture2D texture)
        {
            textureRender.sharedMaterial.mainTexture = texture;
            textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
        }
        
        public void DrawMesh(MeshData meshData, Texture2D texture)
        {
            meshFilter.sharedMesh = meshData.CreateMesh();
            meshRenderer.sharedMaterial.mainTexture = texture;
        }

        public void DrawRawImage(Texture2D texture)
        {
            rawImage.texture = texture;
            rawImage.rectTransform.sizeDelta = new Vector3(texture.width, texture.height, 1);
        }
    }
}