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
	public class ActionShakeObject : IAction
	{
        public GameObject shaker;
        private float shakeX;
        private float shakeY;
        private float shakeZ;
        public NumberProperty shake_x = new NumberProperty(0.01f);
        public NumberProperty shake_y = new NumberProperty(0.01f);
        public NumberProperty shake_z = new NumberProperty(0.01f);
        public NumberProperty InitialtimerValue = new NumberProperty(0.0f);
        public NumberProperty IntervaltimerValue = new NumberProperty(0.0f);
        public NumberProperty LoopCount = new NumberProperty(0f);
        public bool InfiniteLoops;
        private float loopsCounted = 1;

        private bool Up;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	        CancelInvoke("shakingObject");
            shakeX = shake_x.GetValue(this.gameObject);
            shakeY = shake_y.GetValue(this.gameObject);
            shakeZ = shake_z.GetValue(this.gameObject);


            InvokeRepeating("shakingObject", InitialtimerValue.GetValue(target), IntervaltimerValue.GetValue(target));

         

            return true;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
          
            return base.Execute(target, actions, index);
        }

        private void shakingObject()
        {
            if (Up)
            {
                shaker.transform.Translate(shakeX, shakeY, shakeZ);
                Up = false;
            }
            else
            {
                shaker.transform.Translate(-shakeX, -shakeY, -shakeZ);
                Up = true;
            }

            loopsCounted++;

            if ((LoopCount.GetValue() < loopsCounted) && (InfiniteLoops == false))
            {
                CancelInvoke("shakingObject");

            }


        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack1/Objects/Shake an Object";
		private const string NODE_TITLE = "Shake an Object";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spFloatObjectShaker; 
        private SerializedProperty spFloatObjectShake_x;
        private SerializedProperty spFloatObjectShake_y;
        private SerializedProperty spFloatObjectShake_z;
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
            this.spFloatObjectShaker = this.serializedObject.FindProperty("shaker"); 
            this.spFloatObjectShake_x = this.serializedObject.FindProperty("shake_x");
            this.spFloatObjectShake_y = this.serializedObject.FindProperty("shake_y");
            this.spFloatObjectShake_z = this.serializedObject.FindProperty("shake_z");
            this.spInitialtimerValue = this.serializedObject.FindProperty("InitialtimerValue");
            this.spIntervaltimerValue = this.serializedObject.FindProperty("IntervaltimerValue");
            this.spLoopCount = this.serializedObject.FindProperty("LoopCount");
            this.spInfiniteLoops = this.serializedObject.FindProperty("InfiniteLoops");
        }

        protected override void OnDisableEditorChild ()
		{
            this.spFloatObjectShaker = null; 
            this.spFloatObjectShake_x = null;
            this.spFloatObjectShake_y = null;
            this.spFloatObjectShake_z = null;
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

            EditorGUILayout.PropertyField(this.spFloatObjectShaker, new GUIContent("GameObject")); 
            EditorGUILayout.PropertyField(this.spFloatObjectShake_x, new GUIContent("shake_x"));
            EditorGUILayout.PropertyField(this.spFloatObjectShake_y, new GUIContent("shake_y"));
            EditorGUILayout.PropertyField(this.spFloatObjectShake_z, new GUIContent("shake_z"));
            EditorGUI.indentLevel--;
            EditorGUILayout.LabelField("Set Time to Shake", EditorStyles.boldLabel);
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
