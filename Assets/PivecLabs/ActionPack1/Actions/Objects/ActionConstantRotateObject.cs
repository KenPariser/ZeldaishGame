namespace PivecLabs.ActionPack
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.Events;

	using GameCreator.Core;
	using GameCreator.Core.Hooks;
	using GameCreator.Variables;

#if UNITY_EDITOR
	using UnityEditor;
#endif
	[AddComponentMenu("")]

	public class ActionConstantRotateObject : IAction
	{

		public TargetGameObject target = new TargetGameObject();
		private GameObject objectToRotate;
		
		public float rotationSpeed = 100.0f;
  
		public bool xAxis = false;
		public bool yAxis = false;
		public bool zAxis = false;
		public bool reverse = false;

		private float xaxis = 0.0f;
		private float yaxis = 0.0f;
		private float zaxis = 0.0f;

		private Vector3 rotateAxis; 


		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{

			objectToRotate = this.target.GetGameObject(target);

			if (xAxis && !reverse) {xaxis = 90.0f;}
			else if (xAxis && reverse) {xaxis = -90.0f;}
			else {xaxis = 0.0f;}
		
			if (yAxis && !reverse) {yaxis = 90.0f;}
			else if (yAxis && reverse) {yaxis = -90.0f;}
			else {yaxis = 0.0f;}

			if (zAxis && !reverse) {zaxis = 90.0f;}
			else if (zAxis && reverse) {zaxis = -90.0f;}
			else {zaxis = 0.0f;}

    	
			rotateAxis = new Vector3(xaxis, yaxis, zaxis);
		
			return true;

		}


	


		void Update(){
		
			if(objectToRotate) 
			    objectToRotate.transform.Rotate(rotateAxis, rotationSpeed * Time.deltaTime);
		}

	

		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack1/Objects/Rotate Object Constantly";
		private const string NODE_TITLE = "Rotate Object Constantly";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

		// PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty sptarget;

		private SerializedProperty sprotationSpeed;
		private SerializedProperty spxAxis;
		private SerializedProperty spyAxis;
		private SerializedProperty spzAxis;
		private SerializedProperty spreverse;

		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{

			return string.Format(NODE_TITLE);
		}


		protected override void OnEnableEditorChild()
		{
			this.sptarget = this.serializedObject.FindProperty("target");
			this.sprotationSpeed = this.serializedObject.FindProperty("rotationSpeed");
			this.spxAxis = this.serializedObject.FindProperty("xAxis");
			this.spyAxis = this.serializedObject.FindProperty("yAxis");
			this.spzAxis = this.serializedObject.FindProperty("zAxis");
			this.spreverse = this.serializedObject.FindProperty("reverse");

		}

		protected override void OnDisableEditorChild()
		{
			this.sptarget = null;
			this.sprotationSpeed = null;
			this.spxAxis = null;
			this.spyAxis = null;
			this.spzAxis = null;
			this.spreverse = null;


		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.sptarget, new GUIContent("Object to Rotate"));
			EditorGUILayout.Space();
				EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(this.sprotationSpeed, new GUIContent("Rotation Speed"));
			EditorGUILayout.Space();
			EditorGUILayout.LabelField(new GUIContent("Axis to Rotate"));
			EditorGUILayout.PropertyField(this.spxAxis, new GUIContent("xAxis"));
			EditorGUILayout.PropertyField(this.spyAxis, new GUIContent("yAxis"));
			EditorGUILayout.PropertyField(this.spzAxis, new GUIContent("zAxis"));
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spreverse, new GUIContent("Reverse"));
			
					EditorGUI.indentLevel--;
		
			this.serializedObject.ApplyModifiedProperties();
		}

#endif
	}
}