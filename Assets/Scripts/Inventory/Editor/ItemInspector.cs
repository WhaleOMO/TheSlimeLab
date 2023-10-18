using UnityEditor;
using UnityEngine;

namespace Inventory.Editor
{
    /// <summary>
    /// Custom Inspector for Item objects
    /// </summary>
    [CustomEditor(typeof(Item))]
    public class ItemInspector : UnityEditor.Editor
    {
        private SerializedProperty itemIDProp;
        private SerializedProperty itemNameProp;
        private SerializedProperty itemDescProp;
        private SerializedProperty iconProp;
        private SerializedProperty canStackProp;
        private SerializedProperty maxStackProp;
        
        private GameObject previewObject;
        private UnityEditor.Editor gameObjectEditor;
        
        private void OnEnable()
        {
            itemIDProp = serializedObject.FindProperty("id");
            itemNameProp = serializedObject.FindProperty("itemName");
            itemDescProp = serializedObject.FindProperty("description");
            iconProp = serializedObject.FindProperty("icon");
            canStackProp = serializedObject.FindProperty("isStackable");
            maxStackProp = serializedObject.FindProperty("maxStackAmount");
            
            if ((target as Item).prefab)
            {
                previewObject = Instantiate((target as Item).prefab);
                previewObject.hideFlags = HideFlags.HideAndDontSave;
                gameObjectEditor = UnityEditor.Editor.CreateEditor(previewObject);
                // Try setting color
                if (previewObject.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
                {
                    var tempMaterial = new Material(renderer.sharedMaterial);
                    tempMaterial.color = serializedObject.FindProperty("color").colorValue;
                    renderer.sharedMaterial = tempMaterial;
                }
            }
        }
        
        public void OnDisable()
        {
            OnDestroy();
        }
        
        void OnDestroy()
        {
            if (gameObjectEditor)
            {
                DestroyImmediate(gameObjectEditor);
            }

            if (previewObject)
            {
                DestroyImmediate(previewObject);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Item item = (Item)target;

            EditorGUILayout.BeginHorizontal();

            float iconSize = 100;

            iconProp.objectReferenceValue = (Sprite)EditorGUI.ObjectField(new Rect(20, 20, iconSize, iconSize),
                item.icon, typeof(Sprite), false);

            GUILayout.Space(110);
            float fieldsHeight = EditorGUIUtility.singleLineHeight * 2;
            float spacing = (iconSize - fieldsHeight) / 2;

            EditorGUILayout.BeginVertical();
                GUILayout.Space(spacing);
                float labelWidth = 80;
                // ID
                Rect rect = EditorGUILayout.GetControlRect();
                EditorGUI.LabelField(new Rect(rect.x, rect.y, labelWidth, rect.height), "ID");
                itemIDProp.intValue =
                    EditorGUI.IntField(new Rect(rect.x + labelWidth, rect.y, rect.width - labelWidth, rect.height),
                        item.id);
                // Name
                rect = EditorGUILayout.GetControlRect();
                EditorGUI.LabelField(new Rect(rect.x, rect.y, labelWidth, rect.height), "Name");
                itemNameProp.stringValue =
                    EditorGUI.TextField(new Rect(rect.x + labelWidth, rect.y, rect.width - labelWidth, rect.height),
                        item.itemName);
                // Description
                rect = EditorGUILayout.GetControlRect();
                EditorGUI.LabelField(new Rect(rect.x, rect.y, labelWidth, rect.height), "Description");
                itemDescProp.stringValue =
                    EditorGUI.TextField(new Rect(rect.x + labelWidth, rect.y, rect.width - labelWidth, rect.height * 2),
                        item.description);
                GUILayout.Space(spacing);
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            // Draw the default inspector content but exclude drawn fields
            DrawPropertiesExcluding(serializedObject, "m_Script", "id", "itemName", "icon", "description",
                "maxStackAmount");

            if (canStackProp.boolValue)
            {
                maxStackProp.intValue = EditorGUILayout.IntField("Max Stack Amount", item.maxStackAmount);
            }

            bool modified = serializedObject.ApplyModifiedProperties();

            if (gameObjectEditor && modified)
            {
                // Destroy previous editor
                if (gameObjectEditor)
                {
                    DestroyImmediate(gameObjectEditor);
                    gameObjectEditor = null;
                }

                // Destroy previous preview object
                if (previewObject)
                {
                    DestroyImmediate(previewObject);
                    previewObject = null;
                }

                if (!(target as Item).prefab)
                {
                    return;
                }

                previewObject = Instantiate((target as Item).prefab);
                previewObject.hideFlags = HideFlags.HideAndDontSave;
                gameObjectEditor = UnityEditor.Editor.CreateEditor(previewObject);
                
                // Try setting color
                if (previewObject.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
                {
                    var tempMaterial = new Material(renderer.sharedMaterial);
                    tempMaterial.color = serializedObject.FindProperty("color").colorValue;
                    renderer.sharedMaterial = tempMaterial;
                }
            }

            // Drawing the preview within OnInspectorGUI
            if (gameObjectEditor)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Preview");
                float width = EditorGUIUtility.currentViewWidth;
                Rect r = GUILayoutUtility.GetRect(width, 400); // Adjust the 400 if you want a different height.
                gameObjectEditor.OnInteractivePreviewGUI(r, EditorStyles.whiteBoldLabel);
            }
        }
    }
}