namespace PivecLabs.ActionPack
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
    using GameCreator.Variables;

#if UNITY_EDITOR
    using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class Action3DTextMeshTimer : IAction
	{
        public GameObject textObject;
        private TMPro.TextMeshPro textdata;


        public enum RESULT
		{
			Nothing,
            Action,
            Condition
        }
        public RESULT timerResult = RESULT.Action;

        public bool countdown;
        public bool countup;

        public NumberProperty InitialtimerValue = new NumberProperty(0.0f);
        public NumberProperty TotaltimerValue = new NumberProperty(10.0f);
        public Actions actionToCall;
        public Conditions conditionToCall;

        private float timervalue;
		private float totaltime;
		private bool timerCanceled;
		
        // EXECUTABLE: ----------------------------------------------------------------------------


        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
	        textdata = textObject.GetComponent<TMPro.TextMeshPro>();

            timervalue = TotaltimerValue.GetValue(target);
	        timerCanceled = false;
 
            if (countdown == false)
            {
                timervalue = 1;
            }
            CancelInvoke("Timer"); 

            InvokeRepeating("Timer", InitialtimerValue.GetValue(target), 1.0f); 

	        totaltime = ((TotaltimerValue.GetValue(target) + InitialtimerValue.GetValue(target))*10);

	        while(totaltime > 10 )
	        {
		        totaltime--;
		        yield return new WaitForSeconds(0.1f);
	        }

            CancelInvoke("Timer");

            if (countdown == true)
            {
                textdata.text = "0";
            }
            
	        if (timerCanceled == false)
	        {
		        switch (this.timerResult)
		        {
		        case RESULT.Nothing:
			        break;
		        case RESULT.Action:
			        this.actionToCall.Execute(gameObject, null);
			        break;
		        case RESULT.Condition:
			        this.conditionToCall.Interact(gameObject);
			        break;
		        }

	        	
	        }
           yield return 0;
        }

        private void Timer()
        {
           
 
            if (countdown == true)
            {
                textdata.text = timervalue.ToString();

                timervalue--;

            }
            else
            {
                textdata.text = timervalue.ToString();

                timervalue++;
            }


        }
        
		public void StopTimer(float reset)
		{
			totaltime = 0;
			timervalue = reset;
			timerCanceled = true;

		}
		


        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

        public static new string NAME = "ActionPack1/TMP/3D TextMesh Timer";
		private const string NODE_TITLE = "Display 3D Timer and Action";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptextmesh;

        private SerializedProperty spInitialtimerValue;
        private SerializedProperty spTotaltimerValue;
        private SerializedProperty spactionToCall;
        private SerializedProperty spconditionToCall;
        private SerializedProperty sptimerResult;

        private SerializedProperty spcountdown;
        private SerializedProperty spcountup;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.sptextmesh = this.serializedObject.FindProperty("textObject");

            this.spInitialtimerValue = this.serializedObject.FindProperty("InitialtimerValue");
            this.spTotaltimerValue = this.serializedObject.FindProperty("TotaltimerValue");
            this.spactionToCall = this.serializedObject.FindProperty("actionToCall");
            this.spconditionToCall = this.serializedObject.FindProperty("conditionToCall");
            this.sptimerResult = this.serializedObject.FindProperty("timerResult");

            this.spcountdown = this.serializedObject.FindProperty("countdown");
            this.spcountup = this.serializedObject.FindProperty("countup");

        }

        protected override void OnDisableEditorChild ()
		{
			this.sptextmesh = null;
            this.spInitialtimerValue = null;
            this.spTotaltimerValue = null;
            this.spactionToCall = null;
            this.spconditionToCall = null;
            this.sptimerResult = null;
            this.spcountdown = null;
            this.spcountup = null;

        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

	        EditorGUILayout.PropertyField(this.sptextmesh, new GUIContent("TextMeshPro Object"));
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(this.spInitialtimerValue, new GUIContent("Time before start"));
            EditorGUILayout.PropertyField(this.spTotaltimerValue, new GUIContent("Timer Value"));
            EditorGUILayout.LabelField(new GUIContent("Count Direction"));
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 100;
            EditorGUILayout.PropertyField(this.spcountdown, new GUIContent("down"));
            EditorGUILayout.PropertyField(this.spcountup, new GUIContent("up"));
            countup = countdown ? false : true;
            EditorGUIUtility.labelWidth = 0;
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(this.sptimerResult, new GUIContent("When Timer expires"));

            switch ((RESULT)this.sptimerResult.intValue)
            {
            case RESULT.Nothing:
	            
	            break;
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
