using UnityEditor;
using UnityEngine;
using System;
using QuickPrimitives;

[CustomEditor(typeof(QcSectionMesh))]
public class QcSectionMeshEditor : Editor
{
    private QcSectionMesh.QcSectionProperties oldProp = new QcSectionMesh.QcSectionProperties();

    override public void OnInspectorGUI()
    {
        QcSectionMesh mesh = target as QcSectionMesh;

        mesh.properties.width = EditorGUILayout.Slider("Width", mesh.properties.width, 0.1f, 10);
        mesh.properties.depth = EditorGUILayout.Slider("Depth", mesh.properties.depth, 0.01f, 10);
        mesh.properties.height = EditorGUILayout.Slider("Height", mesh.properties.height, 0.1f, 10);

        mesh.properties.offset =
                    EditorGUILayout.Vector3Field("Offset", mesh.properties.offset);

        mesh.properties.type =
            (QcSectionMesh.QcSectionProperties.Types)EditorGUILayout.EnumPopup("Type", mesh.properties.type);

        
        using (var group =
            new EditorGUILayout.FadeGroupScope(Convert.ToSingle(mesh.properties.type ==
                                               QcSectionMesh.QcSectionProperties.Types.LType)))
        {
            if (group.visible)
            {
                EditorGUI.indentLevel++;
                mesh.properties.backThickness = EditorGUILayout.Slider("Back Thickness", mesh.properties.backThickness, 0.01f, mesh.properties.depth * 0.95f);
                mesh.properties.sideThickness = EditorGUILayout.Slider("Side Thickness", mesh.properties.sideThickness, 0.01f, mesh.properties.width * 0.95f);
                mesh.properties.capThickness = EditorGUILayout.Toggle("Cap Thickness", mesh.properties.capThickness);
                using (new EditorGUI.DisabledScope(!mesh.properties.capThickness))
                {
                    EditorGUI.indentLevel++;
                    mesh.properties.backCap = EditorGUILayout.Slider("Back Cap", mesh.properties.backCap, 0.01f, mesh.properties.depth * 0.95f);
                    mesh.properties.sideCap = EditorGUILayout.Slider("Side Cap", mesh.properties.sideCap, 0.01f, mesh.properties.width * 0.95f);
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }
        }

        using (var group =
            new EditorGUILayout.FadeGroupScope(Convert.ToSingle(mesh.properties.type ==
                                               QcSectionMesh.QcSectionProperties.Types.IType)))
        {
            if (group.visible)
            {
                EditorGUI.indentLevel++;
                mesh.properties.frontThickness = EditorGUILayout.Slider("Front Thickness", mesh.properties.frontThickness, 0.01f, mesh.properties.depth * 0.95f);
                mesh.properties.backThickness = EditorGUILayout.Slider("Back Thickness", mesh.properties.backThickness, 0.01f, mesh.properties.depth * 0.95f);
                mesh.properties.sideThickness = EditorGUILayout.Slider("Side Thickness", mesh.properties.sideThickness, 0.01f, mesh.properties.width * 0.95f);
                mesh.properties.capThickness = EditorGUILayout.Toggle("Cap Thickness", mesh.properties.capThickness);
                using (new EditorGUI.DisabledScope(!mesh.properties.capThickness))
                {
                    EditorGUI.indentLevel++;
                    mesh.properties.frontCap = EditorGUILayout.Slider("Front Cap", mesh.properties.frontCap, 0.01f, mesh.properties.depth * 0.95f);
                    mesh.properties.backCap = EditorGUILayout.Slider("Back Cap", mesh.properties.backCap, 0.01f, mesh.properties.depth * 0.95f);
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }
        }

        using (var group =
            new EditorGUILayout.FadeGroupScope(Convert.ToSingle(mesh.properties.type ==
                                               QcSectionMesh.QcSectionProperties.Types.CType)))
        {
            if (group.visible)
            {
                EditorGUI.indentLevel++;
                mesh.properties.frontThickness = EditorGUILayout.Slider("Front Thickness", mesh.properties.frontThickness, 0.01f, mesh.properties.depth * 0.95f);
                mesh.properties.backThickness = EditorGUILayout.Slider("Back Thickness", mesh.properties.backThickness, 0.01f, mesh.properties.depth * 0.95f);
                mesh.properties.sideThickness = EditorGUILayout.Slider("Side Thickness", mesh.properties.sideThickness, 0.01f, mesh.properties.width * 0.95f);
                mesh.properties.capThickness = EditorGUILayout.Toggle("Cap Thickness", mesh.properties.capThickness);
                using (new EditorGUI.DisabledScope(!mesh.properties.capThickness))
                {
                    EditorGUI.indentLevel++;
                    mesh.properties.frontCap = EditorGUILayout.Slider("Front Cap", mesh.properties.frontCap, 0.01f, mesh.properties.depth * 0.95f);
                    mesh.properties.backCap = EditorGUILayout.Slider("Back Cap", mesh.properties.backCap, 0.01f, mesh.properties.depth * 0.95f);
                    EditorGUI.indentLevel--; 
                }
                EditorGUI.indentLevel--; 
            }
        }

        using (var group =
            new EditorGUILayout.FadeGroupScope(Convert.ToSingle(mesh.properties.type ==
                                               QcSectionMesh.QcSectionProperties.Types.TType)))
        {
            if (group.visible)
            {
                EditorGUI.indentLevel++;
                mesh.properties.backThickness = EditorGUILayout.Slider("Back Thickness", mesh.properties.backThickness, 0.01f, mesh.properties.depth * 0.95f);
                mesh.properties.sideThickness = EditorGUILayout.Slider("Side Thickness", mesh.properties.sideThickness, 0.01f, mesh.properties.width * 0.95f);
                mesh.properties.capThickness = EditorGUILayout.Toggle("Cap Thickness", mesh.properties.capThickness);
                using (new EditorGUI.DisabledScope(!mesh.properties.capThickness))
                {
                    EditorGUI.indentLevel++;
                    mesh.properties.backCap = EditorGUILayout.Slider("Back Cap", mesh.properties.backCap, 0.01f, mesh.properties.depth * 0.95f);
                    mesh.properties.sideCap = EditorGUILayout.Slider("Side Cap", mesh.properties.sideCap, 0.01f, mesh.properties.width * 0.95f);
                    EditorGUI.indentLevel--; 
                }
                EditorGUI.indentLevel--;
            }
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

    private void CheckValues(QcSectionMesh boxMesh)
    {
    }

    private void ShowVertexCount(QcSectionMesh mesh)
    {
        EditorGUILayout.HelpBox(mesh.vertices.Count + " vertices\r\n" + mesh.faces.Count + " triangles", MessageType.Info);
    }

    private void SaveMeshAsPrefab()
    {
        QcSectionMesh box = (QcSectionMesh)target;

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
