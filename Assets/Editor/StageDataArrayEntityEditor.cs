using UnityEditor;

using UnityEditorInternal;

using UnityEngine;

[CustomEditor(typeof(StageDataArrayEntity))]
public class StageDataArrayEntityEditor :Editor
{
    private ReorderableList list;

    private void OnEnable()
    {
        SerializedProperty arrayProperty = serializedObject.FindProperty("array");

        list = new ReorderableList(serializedObject, arrayProperty, true, true, true, true);

        // ヘッダーの描画
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Stage Data");
        };

        // 要素の高さを動的に計算
        list.elementHeightCallback = (int index) =>
        {
            SerializedProperty element = arrayProperty.GetArrayElementAtIndex(index);
            float height = EditorGUIUtility.singleLineHeight + 4; // デフォルトの高さ

            if (element.isExpanded)
            {
                height += EditorGUI.GetPropertyHeight(element, true);
            }
            return height;
        };

        // 要素の描画をカスタマイズ
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            SerializedProperty element = arrayProperty.GetArrayElementAtIndex(index);

            rect.y += 2;

            // StageNumを取得
            SerializedProperty stageNumProp = element.FindPropertyRelative("stageNum");
            SerializedProperty sceneNameProp = element.FindPropertyRelative("sceneName");
            int stageNum = index + 1;
            stageNumProp.intValue = stageNum;
            sceneNameProp.enumValueIndex = stageNum - 1;

            // フォールドアウト（展開用三角）の描画
            element.isExpanded = EditorGUI.Foldout(
                new Rect(rect.x + 10, rect.y, 15, EditorGUIUtility.singleLineHeight),
                element.isExpanded,
                GUIContent.none
            );

            // ラベルの描画
            EditorGUI.LabelField(
                new Rect(rect.x + 15, rect.y, rect.width - 15, EditorGUIUtility.singleLineHeight),
                "Stage " + stageNum
            );

            if (element.isExpanded)
            {
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + 2, rect.width, EditorGUI.GetPropertyHeight(element, true)),
                    element,
                    GUIContent.none,
                    true
                );
                EditorGUI.indentLevel--;
            }
        };

        // 元の配列を非表示
        arrayProperty.isExpanded = false;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // カスタムReorderableListを描画
        list.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
}
