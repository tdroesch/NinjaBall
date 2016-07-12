/// <summary>
/// Duplicate folder utility.
/// This Editor extension  will duplicate a folder and all it's contents
/// and then link meshes, materials, and some textures to duplicated prefab
/// based on relative path.
/// Only the _MainTex texture of a material will be duplicated.  All other
/// maps must be done manually.
/// 
/// Author: Tyler Roesch
/// Date: 10/24/15
/// </summary>

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

public class DuplicateFolderUtility : Editor {

	[MenuItem("Edit/Duplicate Folder with Relative Paths")]
	static void DuplicateFolderWithRelativePaths(){
		string sourceFolder;
		string cloneFolder;
		string sourcePath;
		string clonePath;
		List<string> cloneAssetPaths = new List<string>();

		var selected = Selection.activeObject;
		if (selected.GetType() != typeof(UnityEditor.DefaultAsset)){
			Debug.LogWarning("A folder was not selected.");
			return;
		}
		sourcePath = AssetDatabase.GetAssetPath(selected);

		sourceFolder = sourcePath.Substring(sourcePath.LastIndexOf('/')+1);
		cloneFolder = "_clone_" + sourceFolder;
		
		clonePath = sourcePath.Replace(sourceFolder, cloneFolder);
		AssetDatabase.CopyAsset(sourcePath, clonePath);
		AssetDatabase.Refresh();

		foreach (string s in AssetDatabase.GetAllAssetPaths()){
			if (s.StartsWith(clonePath)) {
				cloneAssetPaths.Add(s);
			}
		}

		foreach (string s in cloneAssetPaths){
			var mainAsset = AssetDatabase.LoadMainAssetAtPath(s);
			if (PrefabUtility.GetPrefabType(mainAsset) == PrefabType.Prefab){
				UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetRepresentationsAtPath(s);
				foreach (UnityEngine.Object asset in assets) {
					ReplaceComponentPaths(asset, sourcePath, clonePath);
				}
				ReplaceComponentPaths(mainAsset, sourcePath, clonePath);
			}
			if (s.EndsWith(".mat")){
				ReplaceMaterialTextures(mainAsset, sourcePath, clonePath);
			}
		}
		AssetDatabase.Refresh();
	}

	static void ReplaceComponentPaths(UnityEngine.Object asset, string sourcePath, string clonePath){
		GameObject go = asset as GameObject;
		MeshFilter mf = go.GetComponent<MeshFilter>();
		MeshRenderer mr = go.GetComponent<MeshRenderer>();
		if (mf != null){
			string meshPath = AssetDatabase.GetAssetPath(mf.sharedMesh);
			string meshName = mf.sharedMesh.name;
			if (meshPath.StartsWith(sourcePath)){
				UnityEngine.Object[] fbxObjects = AssetDatabase.LoadAllAssetsAtPath(meshPath.Replace(sourcePath, clonePath));
				foreach (UnityEngine.Object o in fbxObjects){
					if (o is Mesh && meshName.Equals(o.name)){
						mf.sharedMesh = (Mesh)o;
					}
				}
			}
		}
		if (mr != null){
			Material[] mats = mr.sharedMaterials;
			for (int i = 0; i < mats.Length; i++){
				string matPath = AssetDatabase.GetAssetPath(mats[i]);
				if (matPath.StartsWith(sourcePath)) {
					mats[i] = AssetDatabase.LoadAssetAtPath(matPath.Replace(sourcePath, clonePath), typeof(Material)) as Material;
				}
			}
			mr.sharedMaterials = mats;
		}
	}

	static void ReplaceMaterialTextures (UnityEngine.Object asset, string sourcePath, string clonePath)
	{
		Material mat = asset as Material;
		string texPath = AssetDatabase.GetAssetPath(mat.mainTexture);
		if (texPath.StartsWith(sourcePath)) {
			mat.mainTexture = AssetDatabase.LoadAssetAtPath(texPath.Replace(sourcePath, clonePath), typeof(Texture)) as Texture;
		}
	}
}
