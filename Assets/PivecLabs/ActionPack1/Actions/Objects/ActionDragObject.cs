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

	public class ActionDragObject : IAction
	{

		public TargetGameObject target = new TargetGameObject();
		public GameObject objectToDrag;
		public bool allowDragging;
		public bool restrictDragging;
		public bool xAxis;
		public bool yAxis;
		public bool zAxis;
		
		private bool dragging;
		private Rigidbody r;

		private Vector3 speed = Vector3.zero;
		private Vector2 lastMousePosition = Vector2.zero;

	
		private Vector3 mOffset;
		private float mZCoord;


		[SerializeField]
		[Range(0, 2)]
		public int mouseButton = 0;

		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{


			objectToDrag = this.target.GetGameObject(target);
		
			return true;

		}

		private Vector3 GetMouseAsWorldPoint()
		{


			Vector3 mousePoint = Input.mousePosition;
			mousePoint.z = mZCoord;
			return Camera.main.ScreenToWorldPoint(mousePoint);

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
							mZCoord = Camera.main.WorldToScreenPoint(
							objectToDrag.transform.position).z;
							mOffset = objectToDrag.transform.position - GetMouseAsWorldPoint();

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

						
					}

					if (restrictDragging == true)

					{
					
						if (xAxis == true)

						{
		
							Vector3 newYposition = GetMouseAsWorldPoint() + mOffset;
							objectToDrag.transform.position = new Vector3(objectToDrag.transform.position.x,newYposition.y, objectToDrag.transform.position.z);

						}
						
						if (yAxis == true)

						{
							Vector3 newXposition = GetMouseAsWorldPoint() + mOffset;
							objectToDrag.transform.position = new Vector3(newXposition.x, objectToDrag.transform.position.y, objectToDrag.transform.position.z);
						}
	
						if (zAxis == true)

						{
							Vector3 newZposition = GetMouseAsWorldPoint() + mOffset;
							objectToDrag.transform.position = new Vector3(objectToDrag.transform.position.x, objectToDrag.transform.position.y, newZposition.z);
						}

						
					}
						
					else
					
					objectToDrag.transform.position = GetMouseAsWorldPoint() + mOffset;


					

					lastMousePosition = Input.mousePosition;
				}

			}

		}



		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack1/Objects/Drag Object with Mouse";
		private const string NODE_TITLE = "Drag Object with Mouse";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

		// PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty spobjectToDrag;
		private SerializedProperty spallowDragging;
		private SerializedProperty spmouseButton;
		private SerializedProperty sprestrictDragging;
		private SerializedProperty spxAxis;
		private SerializedProperty spyAxis;
		private SerializedProperty spzAxis;

		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{

			return string.Format(NODE_TITLE);
		}


		protected override void OnEnableEditorChild()
		{
			this.spobjectToDrag = this.serializedObject.FindProperty("target");
			this.spallowDragging = this.serializedObject.FindProperty("allowDragging");
			this.spmouseButton = this.serializedObject.FindProperty("mouseButton");
			this.sprestrictDragging = this.serializedObject.FindProperty("restrictDragging");
			this.spxAxis = this.serializedObject.FindProperty("xAxis");
			this.spyAxis = this.serializedObject.FindProperty("yAxis");
			this.spzAxis = this.serializedObject.FindProperty("zAxis");

		}

		protected override void OnDisableEditorChild()
		{
			this.spobjectToDrag = null;
			this.spallowDragging = null;
			this.spmouseButton = null;
			this.sprestrictDragging = null;
			this.spxAxis = null;
			this.spyAxis = null;
			this.spzAxis = null;


		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spobjectToDrag, new GUIContent("Game Object to Drag"));
			EditorGUILayout.PropertyField(this.spallowDragging, new GUIContent("Allow Dragging"));
			EditorGUILayout.Space();
			if (allowDragging == true)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(this.sprestrictDragging, new GUIContent("Restrict Dragging"));
				if (restrictDragging == true)
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField(spxAxis, new GUIContent("only x Axis"));
					EditorGUILayout.PropertyField(spyAxis, new GUIContent("only y Axis"));
					EditorGUILayout.PropertyField(spzAxis, new GUIContent("only z Axis"));
					EditorGUI.indentLevel--;

				}
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