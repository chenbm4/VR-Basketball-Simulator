using UnityEngine;
using UnityEditor;
using System;
using QuickPrimitives;

[CustomEditor(typeof(QcColumnMesh))]
public class QcColumnMeshEditor : Editor
{
    private QcColumnMesh.QcColumnProperties oldProp = new QcColumnMesh.QcColumnProperties();

    override public void OnInspectorGUI()
    {
        QcColumnMesh mesh = target as QcColumnMesh;

        mesh.properties.width = EditorGUILayout.Slider("Width", mesh.properties.width, 0.1f, 10);
        mesh.properties.depth = EditorGUILayout.Slider("Depth", mesh.properties.depth, 0.1f, 10);
        mesh.properties.height = EditorGUILayout.Slider("Height", mesh.properties.height, 0.1f, 10);

        mesh.properties.offset =
                    EditorGUILayout.Vector3Field("Offset", mesh.properties.offset);

        mesh.properties.sides = EditorGUILayout.IntSlider("Sides", mesh.properties.sides, 3, 16);

        using (var group =
            new EditorGUILayout.FadeGroupScope(Convert.ToSingle(mesh.properties.sides == 3)))
        {
            if (group.visible)
            {
                mesh.properties.triangleIncline = EditorGUILayout.Slider("Triangle Incline", mesh.properties.triangleIncline, 0.0f, 1.0f);
            }
        }

        mesh.properties.hollow.enabled = EditorGUILayout.ToggleLeft("Hollow", mesh.properties.hollow.enabled);

        using (new EditorGUI.DisabledScope(!mesh.properties.hollow.enabled))
        {
            mesh.properties.hollow.ratio =
            EditorGUILayout.Slider("Wall Ratio", mesh.properties.hollow.ratio, 0.001f, 1);
        }

        mesh.properties.genTextureCoords = EditorGUILayout.Toggle("Gen Texture Coords", mesh.properties.genTextureCoords);
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
    }

    private void CheckValues(QcColumnMesh columnMesh)
    {
        if (columnMesh.properties.hollow.ratio > columnMesh.properties.width) columnMesh.properties.hollow.ratio = columnMesh.properties.width;
    }

    private void ShowVertexCount(QcColumnMesh mesh)
    {
        EditorGUILayout.HelpBox(mesh.vertices.Count + " vertices\r\n" + mesh.faces.Count + " triangles", MessageType.Info);
    }


    private void SaveMeshAsPrefab()
    {
        QcColumnMesh box = (QcColumnMesh)target;

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
