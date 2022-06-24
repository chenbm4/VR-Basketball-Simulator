using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using QuickPrimitives;

[CustomEditor(typeof(QcGridMesh))]
public class QcGridEditor : Editor
{
    private QcGridMesh.QcGridProperties oldProp = new QcGridMesh.QcGridProperties();

    override public void OnInspectorGUI()
    {
        QcGridMesh mesh = target as QcGridMesh;

        mesh.properties.width = EditorGUILayout.Slider("Width", mesh.properties.width, 0.01f, 10);
        mesh.properties.height = EditorGUILayout.Slider("Height", mesh.properties.height, 0.01f, 10);
        mesh.properties.depth = EditorGUILayout.Slider("Depth", mesh.properties.depth, 0.0f, 10);

        mesh.properties.columnCount = EditorGUILayout.IntSlider("Column Count", mesh.properties.columnCount, 1, 20);
        mesh.properties.rowCount = EditorGUILayout.IntSlider("Row Count", mesh.properties.rowCount, 1, 20);

        mesh.properties.borderWidth = EditorGUILayout.Slider("Border Width", mesh.properties.borderWidth, 0.01f, 5);
        mesh.properties.borderHeight = EditorGUILayout.Slider("Border Height", mesh.properties.borderHeight, 0.01f, 5);

        mesh.properties.offset =
                    EditorGUILayout.Vector3Field("Offset", mesh.properties.offset);

        EditorGUILayout.Space();

        mesh.properties.genTextureCoords = EditorGUILayout.Toggle("Gen Texture Coords", mesh.properties.genTextureCoords);
        //using (new EditorGUI.DisabledScope(!mesh.properties.genTextureCoords))
        //{
        //    mesh.properties.textureWrapped = EditorGUILayout.Toggle("Wrap Texture", mesh.properties.textureWrapped);
        //}

        mesh.properties.addCollider = EditorGUILayout.Toggle("Add Collider", mesh.properties.addCollider);

        ShowVertexCount(mesh);

        if (GUILayout.Button("Save Mesh"))
        {
            SaveMeshAsPrefab();
        }

        CheckValues(mesh);

        if (oldProp.Modified(mesh.properties))
        {
            mesh.RebuildGeometry();
            oldProp.CopyFrom(mesh.properties);
        }

        if (oldProp.textureWrapped != mesh.properties.textureWrapped)
        {
            mesh.ReassignMaterial();
            oldProp.textureWrapped = mesh.properties.textureWrapped;
        }
    }

    private void CheckValues(QcGridMesh gridMesh)
    {
        if (gridMesh.properties.width < 0) gridMesh.properties.width = 1;
        if (gridMesh.properties.height < 0) gridMesh.properties.height = 1;
        if (gridMesh.properties.depth < 0) gridMesh.properties.depth = 1;
        if (gridMesh.properties.borderWidth < 0) gridMesh.properties.borderWidth = 0.1f;
        if (gridMesh.properties.borderHeight < 0) gridMesh.properties.borderHeight = 0.1f;

        if (gridMesh.properties.borderWidth * (gridMesh.properties.columnCount + 1) > gridMesh.properties.width)
            gridMesh.properties.borderWidth = gridMesh.properties.width / ((gridMesh.properties.columnCount + 1) * 2.0f);
        if (gridMesh.properties.borderHeight * (gridMesh.properties.rowCount + 1) > gridMesh.properties.height)
            gridMesh.properties.borderHeight = gridMesh.properties.height / ((gridMesh.properties.rowCount + 1) * 2.0f);
    }
    private void ShowVertexCount(QcGridMesh mesh)
    {
        EditorGUILayout.HelpBox(mesh.vertices.Count + " vertices\r\n" + mesh.faces.Count + " triangles", MessageType.Info);
    }

    private void SaveMeshAsPrefab()
    {
        QcGridMesh box = (QcGridMesh)target;

        MeshFilter meshFilter = box.GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;

        if (mesh != null)
        {
            string path = "Assets/QcPrimitives/Meshes/" + box.name + ".asset";

            // delete the asset at the path if it already exists:
            AssetDatabase.DeleteAsset(path);

            Mesh meshToSave = UnityEngine.Object.Instantiate(mesh) as Mesh;

            AssetDatabase.CreateAsset(meshToSave, path);

            AssetDatabase.SaveAssets();
            Debug.Log("Saved mesh at " + path);
        }
        else
        {
            Debug.Log("Unable to save mesh.");
        }
    }
}
