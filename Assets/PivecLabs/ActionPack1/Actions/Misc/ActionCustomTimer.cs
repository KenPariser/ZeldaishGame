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
	public class ActionCustomTimer : IAction
	{
        public enum RESULT
        {
            Action,
            Condition
        }
        public RESULT timerResult = RESULT.Action;


        public NumberProperty InitialtimerValue = new NumberProperty(0.0f);
        public NumberProperty IntervaltimerValue = new NumberProperty(0.0f);
        public NumberProperty LoopCount = new NumberProperty(0f);
        public bool InfiniteLoops;
        public Actions actionToCall;
        public Conditions conditionToCall;

        private float loopsCounted = 1;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {

                  CancelInvoke("Finished"); 

                  InvokeRepeating("Finished", InitialtimerValue.GetValue(target), IntervaltimerValue.GetValue(target)); 


            return true;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
          
            return base.Execute(target, actions, index);
        }


        private void Finished()
        {
            switch (this.timerResult)
            {
                case RESULT.Action:
                    this.actionToCall.Execute(gameObject, null);
                    break;
                case RESULT.Condition:
                    this.conditionToCall.Interact(gameObject);
                    break;

            }

            loopsCounted++;
           
            if ((LoopCount.GetValue(this.gameObject) < loopsCounted) && (InfiniteLoops == false))
            {
                CancelInvoke("Finished");
        
            }

       
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack1/Misc/Custom Timer";
		private const string NODE_TITLE = "Custom Timer";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";


        // PROPERTIES: ----------------------------------------------------------------------------


        private SerializedProperty spInitialtimerValue;
        private SerializedProperty spIntervaltimerValue;
        private SerializedProperty spLoopCount;
        private SerializedProperty spInfiniteLoops;
        private SerializedProperty spactionToCall;
        private SerializedProperty spconditionToCall;
        private SerializedProperty sptimerResult;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
            this.spInitialtimerValue = this.serializedObject.FindProperty("InitialtimerValue");
            this.spIntervaltimerValue = this.serializedObject.FindProperty("IntervaltimerValue");
            this.spLoopCount = this.serializedObject.FindProperty("LoopCount");
            this.spInfiniteLoops = this.serializedObject.FindProperty("InfiniteLoops");
            this.spactionToCall = this.serializedObject.FindProperty("actionToCall");
            this.spconditionToCall = this.serializedObject.FindProperty("conditionToCall");
            this.sptimerResult = this.serializedObject.FindProperty("timerResult");

        }

        protected override void OnDisableEditorChild ()
		{
            this.spInitialtimerValue = null;
            this.spIntervaltimerValue = null;
            this.spLoopCount = null;
            this.spInfiniteLoops = null;
            this.spactionToCall = null;
            this.spconditionToCall = null;
            this.sptimerResult = null;

        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
            EditorGUILayout.LabelField("Set Timer for Actions and Conditions", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(this.spInitialtimerValue, new GUIContent("Time before start"));
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(this.spIntervaltimerValue, new GUIContent("Repeat Interval Value"));
            EditorGUILayout.PropertyField(this.spInfiniteLoops, new GUIContent("Infinite Loops"));
            if (InfiniteLoops == false)
            {
                EditorGUILayout.PropertyField(this.spLoopCount, new GUIContent("Loop Count"));

            }
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(this.sptimerResult, new GUIContent("Call after each Interval"));
        
            switch ((RESULT)this.sptimerResult.intValue)
            {
                case RESULT.Action:
                    EditorGUILayout.PropertyField(this.spactionToCall, new GUIContent("Action to Call"));
                    break;
                case RESULT.Condition:
                    EditorGUILayout.PropertyField(this.spconditionToCall, new GUIContent("Condition to Call"));
                    break;
                
            }
            this.serializedObject.ApplyModifiedProperties();


        }

#endif
    }
}
