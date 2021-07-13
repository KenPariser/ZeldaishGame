namespace PivecLabs.ActionPack
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
    using GameCreator.Core.Hooks;
    using GameCreator.Variables;

#if UNITY_EDITOR
    using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ActionFloatObject : IAction
	{
		public TargetGameObject target = new TargetGameObject();
		public GameObject floater;
         private float tempValy;
        private float tempValx;
        private float tempValz;
        private Vector3 objPos;

        public NumberProperty amplitude_y = new NumberProperty(0.01f);
        public NumberProperty speed_y = new NumberProperty(1.0f);
        public NumberProperty amplitude_x = new NumberProperty(0.01f);
        public NumberProperty speed_x = new NumberProperty(5.0f);
        public NumberProperty amplitude_z = new NumberProperty(0.01f);
        public NumberProperty speed_z = new NumberProperty(5.0f);

        public NumberProperty InitialtimerValue = new NumberProperty(0.0f);
		public NumberProperty IntervaltimerValue = new NumberProperty(0.1f);
        public NumberProperty LoopCount = new NumberProperty(0f);
        public bool InfiniteLoops;
        private float loopsCounted = 1;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {

	        floater = this.target.GetGameObject(target);

            CancelInvoke("Floating");

            InvokeRepeating("Floating", InitialtimerValue.GetValue(target), IntervaltimerValue.GetValue(target));
       
           
            return true;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
          
            return base.Execute(target, actions, index);
        }

        private void Floating()
        {
            tempValy = floater.transform.position.y;
            tempValx = floater.transform.position.x;
            tempValz = floater.transform.position.z;

            objPos.y = tempValy + amplitude_y.GetValue(this.gameObject) * Mathf.Sin(speed_y.GetValue(this.gameObject) * Time.time);
            objPos.x = tempValx + amplitude_x.GetValue(this.gameObject) * Mathf.Sin(speed_x.GetValue(this.gameObject) * Time.time);
            objPos.z = tempValz + amplitude_z.GetValue(this.gameObject) * Mathf.Sin(speed_z.GetValue(this.gameObject) * Time.time);
            floater.transform.position = objPos;

            loopsCounted++;

            if ((LoopCount.GetValue(this.gameObject) < loopsCounted) && (InfiniteLoops == false))
            {
                CancelInvoke("Floating");

            }


        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack1/Objects/Float an Object";
		private const string NODE_TITLE = "Floating Object";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spFloatObjectFloater; 
        private SerializedProperty spFloatObjectAmplitudeY;
        private SerializedProperty spFloatObjectSpeedY;
        private SerializedProperty spFloatObjectAmplitudeX;
        private SerializedProperty spFloatObjectSpeedX;
        private SerializedProperty spFloatObjectAmplitudeZ;
        private SerializedProperty spFloatObjectSpeedZ;
        private SerializedProperty spInitialtimerValue;
        private SerializedProperty spIntervaltimerValue;
        private SerializedProperty spLoopCount;
        private SerializedProperty spInfiniteLoops;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.spFloatObjectFloater = this.serializedObject.FindProperty("target"); 
            this.spFloatObjectAmplitudeY = this.serializedObject.FindProperty("amplitude_y");
            this.spFloatObjectSpeedY = this.serializedObject.FindProperty("speed_y");
            this.spFloatObjectAmplitudeX = this.serializedObject.FindProperty("amplitude_x");
            this.spFloatObjectSpeedX = this.serializedObject.FindProperty("speed_x");
            this.spFloatObjectAmplitudeZ = this.serializedObject.FindProperty("amplitude_z");
            this.spFloatObjectSpeedZ = this.serializedObject.FindProperty("speed_z");
            this.spInitialtimerValue = this.serializedObject.FindProperty("InitialtimerValue");
            this.spIntervaltimerValue = this.serializedObject.FindProperty("IntervaltimerValue");
            this.spLoopCount = this.serializedObject.FindProperty("LoopCount");
            this.spInfiniteLoops = this.serializedObject.FindProperty("InfiniteLoops");
        }

        protected override void OnDisableEditorChild ()
		{
            this.spFloatObjectFloater = null; 
            this.spFloatObjectAmplitudeY = null;
            this.spFloatObjectSpeedY = null;
            this.spFloatObjectAmplitudeX = null;
            this.spFloatObjectSpeedX = null;
            this.spFloatObjectAmplitudeZ = null;
            this.spFloatObjectSpeedZ = null;
            this.spInitialtimerValue = null;
            this.spIntervaltimerValue = null;
            this.spLoopCount = null;
            this.spInfiniteLoops = null;

        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
            EditorGUILayout.LabelField("Object to Float", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(this.spFloatObjectFloater, new GUIContent("GameObject")); 
            EditorGUILayout.PropertyField(this.spFloatObjectAmplitudeY, new GUIContent("amplitude y Value"));
            EditorGUILayout.PropertyField(this.spFloatObjectSpeedY, new GUIContent("speed y Value"));
            EditorGUILayout.PropertyField(this.spFloatObjectAmplitudeX, new GUIContent("amplitude x Value"));
            EditorGUILayout.PropertyField(this.spFloatObjectSpeedX, new GUIContent("speed x Value"));
            EditorGUILayout.PropertyField(this.spFloatObjectAmplitudeZ, new GUIContent("amplitude z Value"));
            EditorGUILayout.PropertyField(this.spFloatObjectSpeedZ, new GUIContent("speed z Value"));
            EditorGUILayout.Space();
            EditorGUI.indentLevel--;

            EditorGUILayout.LabelField("Set Time for Floating", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(this.spInitialtimerValue, new GUIContent("Time before start"));
            EditorGUILayout.PropertyField(this.spIntervaltimerValue, new GUIContent("Repeat Interval Value"));
            EditorGUILayout.PropertyField(this.spInfiniteLoops, new GUIContent("Infinite Loops"));
            if (InfiniteLoops == false)
            {
                EditorGUILayout.PropertyField(this.spLoopCount, new GUIContent("Loop Count"));

            }
            EditorGUI.indentLevel--;

            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
