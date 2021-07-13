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
	public class ActionRandomAction : IAction
	{
       
        public Actions actions1;
    
        public Actions actions2;
       
        public Actions actions3;
      
        public Actions actions4;

        Actions actionsToExecute = null;

        public bool fromVar;

        [VariableFilter(Variable.DataType.Number)]
        public VariableProperty variableRandom = new VariableProperty(Variable.VarType.GlobalVariable);

        public bool waitToFinish = false;

        private bool actionsComplete = false;
        private bool forceStop = false;
        private float randomVar;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {

            if (fromVar == true)
            {
                randomVar = (float)this.variableRandom.Get(target);

            }
            else
            {
                randomVar = Random.Range(1, 5); 

            }


            switch (randomVar)
                {
                    case 1:
                        actionsToExecute = this.actions1;
                        break;
                    case 2:
                        actionsToExecute = this.actions2;
                        break;
                    case 3:
                        actionsToExecute = this.actions3;
                        break;
                    case 4:
                        actionsToExecute = this.actions4;
                        break;
                }



          





            if (actionsToExecute != null)
            {
                this.actionsComplete = false;
                actionsToExecute.actionsList.Execute(target, this.OnCompleteActions);

                if (this.waitToFinish)
                {
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




        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
  
        public static new string NAME = "ActionPack1/Misc/Random Action";
        private const string NODE_TITLE = "Execute Random Action";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";


        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spActions1;
        private SerializedProperty spActions2;
        private SerializedProperty spActions3;
        private SerializedProperty spActions4;
        private SerializedProperty spVariableRandom;
        private SerializedProperty spWaittoFinish;
        private SerializedProperty spfromVar;


        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {

            return string.Format(NODE_TITLE);
        }


        protected override void OnEnableEditorChild()
        {
            this.spActions1 = this.serializedObject.FindProperty("actions1");
            this.spActions2 = this.serializedObject.FindProperty("actions2");
            this.spActions3 = this.serializedObject.FindProperty("actions3");
            this.spActions4 = this.serializedObject.FindProperty("actions4");
            this.spVariableRandom = this.serializedObject.FindProperty("variableRandom");
            this.spWaittoFinish = this.serializedObject.FindProperty("waitToFinish");
            this.spfromVar = this.serializedObject.FindProperty("fromVar");
        }

        protected override void OnDisableEditorChild()
        {
             this.spActions1 = null;
             this.spActions2 = null;
             this.spActions3 = null;
             this.spActions4 = null;
             this.spVariableRandom = null;
             this.spWaittoFinish = null;
             this.spfromVar = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            EditorGUILayout.LabelField("Execute 1 of 4 Random Actions", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spfromVar, new GUIContent("Value from Variable"));

            if (fromVar == true)
            {
                EditorGUILayout.LabelField("Randomise Variable from 1 to 5 (int)");
                EditorGUILayout.PropertyField(this.spVariableRandom);
                EditorGUILayout.Space();
            }
              
             EditorGUILayout.Space();

            EditorGUILayout.LabelField("Random Action 1", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
  
         
                    EditorGUILayout.PropertyField(this.spActions1);
           
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Random Action 2", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            
                    EditorGUILayout.PropertyField(this.spActions2);
         

            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Random Action 3", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
         
                    EditorGUILayout.PropertyField(this.spActions3);
       

            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Random Action 4", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
           
                    EditorGUILayout.PropertyField(this.spActions4);
           
          

            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spWaittoFinish);

            this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}