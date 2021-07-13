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

	public class ActionDragObjecttoRotateonX : IAction
	{
		public TargetGameObject target = new TargetGameObject();

		public GameObject objectToDrag;
		public bool allowDragging;
		private bool dragging;

		private Vector3 speed = Vector3.zero;
		private Vector2 lastMousePosition = Vector2.zero;

		public bool RotateX = true;
		public bool InvertX = false;
		private int _xMultiplier
		{
			get { return InvertX ? -1 : 1; }
		}


		public bool RotateY = true;
		public bool InvertY = false;
		private int _yMultiplier
		{
			get { return InvertY ? -1 : 1; }
		}

		public bool RotateZ = true;
		public bool InvertZ = false;
		private int _zMultiplier
		{
			get { return InvertZ ? -1 : 1; }
		}
		
		public int invert;

		[Range(1f, 20.0f)]
		public float dragSpeed = 10f;
		public bool xAxis;
		public bool yAxis;
		public bool zAxis;
		
		public enum AXIS
		{
			xAxis,
			yAxis
		}

		public AXIS selectAxis = AXIS.xAxis;

		[SerializeField]
		[Range(0, 2)]
		public int mouseButton = 0;
	
		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{


			objectToDrag = this.target.GetGameObject(target);
			
		return true;

		}

		void Update()
		{
            if (allowDragging == true)
			{ 
			if (Input.GetMouseButtonDown(mouseButton))
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit, 100.0f))
				{
						if (hit.transform.name == objectToDrag.name)
						{
							dragging = true;
						}
				}
			}

			if (Input.GetMouseButtonUp(mouseButton))
			{
				
					dragging = false;
				
			}

			if ((objectToDrag != null) && (dragging == true))

					{
						if (lastMousePosition == Vector2.zero) lastMousePosition = Input.mousePosition;

						if (Input.GetMouseButton(mouseButton))
						{

							var mouseDelta = ((Vector2)Input.mousePosition - lastMousePosition) * 100;
							mouseDelta.Set(mouseDelta.x / Screen.width, mouseDelta.y / Screen.height);
							
							switch (this.selectAxis)
							{
							case AXIS.xAxis:
								speed = new Vector3(mouseDelta.x * _xMultiplier, 0, 0);
								break;

							case AXIS.yAxis:
								speed = new Vector3(mouseDelta.y * _xMultiplier,0, 0);
								break;
							}
						

						}


						if (speed != Vector3.zero)
						{

							switch (this.selectAxis)
							{
							case AXIS.xAxis:
								objectToDrag.transform.Rotate(speed.x * dragSpeed,0, 0);
								break;

							case AXIS.yAxis:
								objectToDrag.transform.Rotate(speed.x * dragSpeed,0, 0);
								break;
							}
						
						}

						lastMousePosition = Input.mousePosition;
					}

				}

		}






		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack1/Objects/Drag Object to Rotate on xAxis";
		private const string NODE_TITLE = "Drag Object to Rotate on xAxis";
	public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

	// PROPERTIES: ----------------------------------------------------------------------------

	    private SerializedProperty spobjectToDrag;
		private SerializedProperty spallowDragging;
		private SerializedProperty spdragObject;
		private SerializedProperty spAxis;
		private SerializedProperty spdragspeed;
		private SerializedProperty spmouseButton;

		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
	{

			return string.Format(NODE_TITLE);
		}


	protected override void OnEnableEditorChild()
	{
		this.spobjectToDrag = this.serializedObject.FindProperty("target");
			this.spallowDragging = this.serializedObject.FindProperty("allowDragging");
			this.spAxis = this.serializedObject.FindProperty("selectAxis");
			this.spdragspeed = this.serializedObject.FindProperty("dragSpeed");
			this.spmouseButton = this.serializedObject.FindProperty("mouseButton");

		}

		protected override void OnDisableEditorChild()
	{
		this.spobjectToDrag = null;
			this.spallowDragging = null;
			this.spAxis = null;
			this.spdragspeed = null;
			this.spmouseButton = null;


		}

		public override void OnInspectorGUI()
	{
		this.serializedObject.Update();
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spobjectToDrag, new GUIContent("Game Object to Rotate"));
		EditorGUILayout.PropertyField(this.spallowDragging, new GUIContent("Allow Dragging of X Axis"));
		if (allowDragging == true)
			{
			EditorGUI.indentLevel++;
			
		

	
			EditorGUILayout.PropertyField(this.spAxis, new GUIContent("using Axis"));

				EditorGUILayout.PropertyField(this.spdragspeed, new GUIContent("Drag Speed"));
				
			
			EditorGUILayout.PropertyField(spmouseButton, new GUIContent("Mouse Button"));
			Rect position = EditorGUILayout.GetControlRect(false, 2 * EditorGUIUtility.singleLineHeight);
			position.height *= 0.5f;

			position.y += position.height - 10;
			position.x += EditorGUIUtility.labelWidth - 10;
			position.width -= EditorGUIUtility.labelWidth + 26;
			GUIStyle style = GUI.skin.label;
			style.fontSize = 10;
			style.alignment = TextAnchor.UpperLeft; EditorGUI.LabelField(position, "Left", style);
			style.alignment = TextAnchor.UpperCenter; EditorGUI.LabelField(position, "Right", style);
			style.alignment = TextAnchor.UpperRight; EditorGUI.LabelField(position, "Middle", style);
			EditorGUI.indentLevel--;
			}
			this.serializedObject.ApplyModifiedProperties();
	}

#endif
}
}