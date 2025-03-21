﻿using System.Collections.Generic;
using UnityEngine;

namespace GameWorld.Builders.Chunks
{
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshCollider))]
	public class ChunkMesh : MonoBehaviour
	{
		private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
		private static readonly int BaseTexture = Shader.PropertyToID("_BaseTexture");
		private Mesh _mesh;
		
		private MeshFilter _meshFilter;
		private MeshRenderer _meshRenderer;
		private MeshCollider _meshCollider;

		public void Init(Vector3[] vertices, List<int[]> triangleIndices, Vector2[] uvs, List<Color> grassColors, Texture tileTexture)
		{
			if (triangleIndices.Count != grassColors.Count)
			{
				Debug.LogWarning("Sub mesh count does not match color count");
				return;
			}
            
			_mesh ??= new Mesh();
			_mesh.Clear();
            
			_meshFilter.mesh = _mesh;
			_meshCollider.sharedMesh = _mesh;
			_mesh.vertices = vertices;
			_mesh.uv = uvs;
			_mesh.subMeshCount = triangleIndices.Count;
			
			for (int subMesh = 0; subMesh < triangleIndices.Count; subMesh++)
			{
				_mesh.SetTriangles(triangleIndices[subMesh], subMesh);
			}
			
			List<Material> materials = new();
			foreach (Color grassColor in grassColors)
			{
				Material material = new(Shader.Find("Shader Graphs/InteractionShaderGraph"));
				material.SetColor(BaseColor, grassColor);
				material.SetTexture(BaseTexture, tileTexture);
				materials.Add(material);
			}
			_meshRenderer.materials = materials.ToArray();
			
			_mesh.RecalculateNormals();
		}

		private void Awake()
		{
			_meshFilter = GetComponent<MeshFilter>();
			_meshRenderer = GetComponent<MeshRenderer>();
			_meshCollider = GetComponent<MeshCollider>();
		}
	}
}