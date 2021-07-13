namespace PivecLabs.ActionPack
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
	using GameCreator.Core.Hooks;
	using GameCreator.Variables;
	using PivecLabs.MinMaxSliderAttribute;
	
	#if UNITY_EDITOR
	using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ActionRepeatActionsRandomTimes : IAction 
	{
       		

		public enum Source
        {
            Actions,
            Variable
        }

        public Source source = Source.Actions;
        public Actions actions;

        [VariableFilter(Variable.DataType.GameObject)]
        public VariableProperty variable = new VariableProperty(Variable.VarType.LocalVariable);

        public NumberProperty repeatTimes = new NumberProperty(5);

		private bool actionsComplete = false;
		private bool forceStop = false;
		
	
		
		[MinMaxRange(1, 20)]
		public MinMaxInt MinMaxVal;
		
	

		private float randomVar;


		// EXECUTABLE: ----------------------------------------------------------------------------
		
        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
		{
            Actions actionsToExecute = null;

            switch (this.source)
            {
                case Source.Actions:
                    actionsToExecute = this.actions;
                    break;

                case Source.Variable:
                    GameObject value = this.variable.Get(target) as GameObject;
                    if (value != null) actionsToExecute = value.GetComponent<Actions>();
                    break;
            }

            if (actionsToExecute != null)
            {
				
	            randomVar = Random.Range(MinMaxVal.min, MinMaxVal.max); 
	            int times = (int)randomVar;
                
                
                for (int i = 0; i < times && !this.forceStop; ++i)
                {
                    this.actionsComplete = false;
                    actionsToExecute.actionsList.Execute(target, this.OnCompleteActions);

                    WaitUntil wait = new WaitUntil(() =>
                    {
                        if (actionsToExecute == null) return true;
                        if (this.forceStop)
                        {
                            actionsToExecute.actionsList.Stop();
                            return true;
                        }

                        return this.actionsComplete;
                    });

                    yield return wait;
                }
            }

			yield return 0;
		}

        private void OnCompleteActions()
        {
            this.actionsComplete = true;
        }

        public override void Stop()
        {
            this.forceStop = true;
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

        #if UNITY_EDITOR

  		
		public static new string NAME = "ActionPack1/Misc/Repeat Actions Random Times";
		private const string NODE_TITLE = "Repeat Actions {0} Random Times";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spSource;
        private SerializedProperty spActions;
        private SerializedProperty spVariable;
		private SerializedProperty spMinMax;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{
            string actionsName = (this.source == Source.Actions
                ? (this.actions == null ? "none" : this.actions.name)
                : this.variable.ToString()
            );

            return string.Format(
				NODE_TITLE,
                actionsName
			);
		}

		protected override void OnEnableEditorChild ()
		{
            this.spSource = this.serializedObject.FindProperty("source");
            this.spVariable = this.serializedObject.FindProperty("variable");
            this.spActions = this.serializedObject.FindProperty("actions");
			this.spMinMax = this.serializedObject.FindProperty("MinMaxVal");
		}

		protected override void OnDisableEditorChild ()
		{
            this.spSource = null;
            this.spVariable = null;
			this.spActions = null;
			this.spMinMax = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.spSource);

            EditorGUI.indentLevel++;
            switch (this.spSource.enumValueIndex)
            {
                case (int)Source.Actions:
                    EditorGUILayout.PropertyField(this.spActions);
                    break;

                case (int)Source.Variable:
                    EditorGUILayout.PropertyField(this.spVariable);
                    break;
            }

   			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spMinMax, new GUIContent("Random Range"));
			EditorGUI.indentLevel--;
 

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}