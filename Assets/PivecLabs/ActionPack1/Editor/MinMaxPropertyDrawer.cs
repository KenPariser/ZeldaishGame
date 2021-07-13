using UnityEngine;
using UnityEditor;

	namespace PivecLabs.MinMaxSliderAttribute {
		
	[CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
	public class MinMaxSliderDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			MinMaxRangeAttribute minMaxAttr = attribute as MinMaxRangeAttribute;

			var minProperty = property.FindPropertyRelative("min");
			var maxProperty = property.FindPropertyRelative("max");

			DrawMinMaxSlider(position, label, minMaxAttr, minProperty, maxProperty);
		}
			

		protected void DrawMinMaxSlider(Rect position, GUIContent label, MinMaxRangeAttribute minMaxAttr, SerializedProperty minProperty, SerializedProperty maxProperty) {
			float textWidth = 30f;
			float paddingWidth = 10f;
			float fieldWidth = (position.width - EditorGUIUtility.labelWidth) / 2 - textWidth - paddingWidth / 2;
			position.height = EditorGUIUtility.singleLineHeight;
			float minValue = 0;
			float maxValue = 0;
			minValue = minProperty.intValue;
			maxValue = maxProperty.intValue;					
			EditorGUI.BeginChangeCheck();
			EditorGUI.MinMaxSlider(position, label, ref minValue, ref maxValue, minMaxAttr.MinLimit, minMaxAttr.MaxLimit);
			position.y += EditorGUIUtility.singleLineHeight;
			position.x += EditorGUIUtility.labelWidth;
			position.width = textWidth;
			GUI.Label(position, new GUIContent("Min"));
			position.x += position.width;
			position.width = fieldWidth;
			minValue = Mathf.Clamp(EditorGUI.IntField(position, Mathf.RoundToInt(minValue)), minMaxAttr.MinLimit, maxValue);
			position.x += paddingWidth;
			position.x += position.width;
			position.width = textWidth;
			GUI.Label(position, new GUIContent("Max"));
			position.x += position.width;
			position.width = fieldWidth;
			maxValue = Mathf.Clamp(EditorGUI.FloatField(position, maxValue), minValue, minMaxAttr.MaxLimit);
			minProperty.intValue = Mathf.RoundToInt(minValue);
			maxProperty.intValue = Mathf.RoundToInt(maxValue);
		}
	
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			MinMaxRangeAttribute minMax = attribute as MinMaxRangeAttribute;
			float size = EditorGUIUtility.singleLineHeight * 2;
			

			return size;
		}
	}
}