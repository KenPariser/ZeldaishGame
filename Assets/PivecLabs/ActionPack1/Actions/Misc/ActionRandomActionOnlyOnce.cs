namespace PivecLabs.ActionPack
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using GameCreator.Core;

#if UNITY_EDITOR
    using UnityEditor;
#endif



    [AddComponentMenu("")]
	public class ActionRandomActionOnlyOnce : IAction
    {
        public enum RESULT
        {
            Action,
            Condition
        }
        public RESULT result = RESULT.Action;

        public Actions actionToCall;
        public Conditions conditionToCall;

        [System.Serializable]
        public class ActionObject
        {
            public Actions actionToExecute;
           
              }

        [SerializeField]
        public List<ActionObject> ListofActions = new List<ActionObject>();

        Actions actionsToExecute = null;

 
  
        public bool waitToFinish = false;
        public bool executeOnFinish = false;

 
        private bool actionsComplete = false;
        private bool forceStop = false;
        private bool repeat = false;
        private int rand;
        private int randTotal;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
           
            if (repeat == false)
            {
                rand = Random.Range(0, ListofActions.Capacity);
                randTotal = ListofActions.Capacity;
            }

            else if (repeat == true)

            {
                
                rand = Random.Range(0, randTotal);
                 }

            if (randTotal > 0)
            {
                actionsToExecute = ListofActions[rand].actionToExecute;


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

                ListofActions.RemoveAt(rand);
                randTotal = (randTotal - 1);
                repeat = true;
            }
            else
            {
                switch (this.result)
                {
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


  

            private void OnCompleteActions()
        {
            this.actionsComplete = true;
           
        }




        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
       
        public static new string NAME = "ActionPack1/Misc/Random Action Only Once";
        private const string NODE_TITLE = "Execute Random Action Only Once";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

         private SerializedProperty spWaittoFinish;
         private SerializedProperty spexecuteOnFinish;
        private SerializedProperty spactionToCall;
        private SerializedProperty spconditionToCall;
        private SerializedProperty spresult;




        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {

            return string.Format(NODE_TITLE);
        }


        protected override void OnEnableEditorChild()
        {
            this.spWaittoFinish = this.serializedObject.FindProperty("waitToFinish");
            this.spexecuteOnFinish = this.serializedObject.FindProperty("executeOnFinish");
            this.spactionToCall = this.serializedObject.FindProperty("actionToCall");
            this.spconditionToCall = this.serializedObject.FindProperty("conditionToCall");
            this.spresult = this.serializedObject.FindProperty("result");




        }

        protected override void OnDisableEditorChild()
        {
            this.spWaittoFinish = null;
            this.spexecuteOnFinish = null;
            this.spactionToCall = null;
            this.spconditionToCall = null;
            this.spresult = null;

        }




        public override void OnInspectorGUI()
        {



            this.serializedObject.Update();
            EditorGUILayout.LabelField("Execute 1 Random Action from List", EditorStyles.boldLabel);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Action List");

            SerializedProperty property = serializedObject.FindProperty("ListofActions");
            ArrayGUI(property, "Action ", true);
            EditorGUILayout.Space();
              EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spWaittoFinish);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spexecuteOnFinish);

            if (executeOnFinish == true)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(this.spresult, new GUIContent("Call after Finished"));

                switch ((RESULT)this.spresult.intValue)
                {
                    case RESULT.Action:
                        EditorGUILayout.PropertyField(this.spactionToCall, new GUIContent("Action to Call"));
                        break;
                    case RESULT.Condition:
                        EditorGUILayout.PropertyField(this.spconditionToCall, new GUIContent("Condition to Call"));
                        break;

                }
                EditorGUI.indentLevel--;

            }
            serializedObject.ApplyModifiedProperties();

        }



         private void ArrayGUI(SerializedProperty property, string itemType, bool visible)
            {

                 {

                    EditorGUI.indentLevel++;
                    SerializedProperty arraySizeProp = property.FindPropertyRelative("Array.size");
                    EditorGUILayout.PropertyField(arraySizeProp);
             
                for (int i = 0; i < arraySizeProp.intValue; i++)
                    {
                        EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i), new GUIContent(itemType + (i +1).ToString()), true);
                   
                    }

                EditorGUI.indentLevel--;
                }
            }

      


#endif
    }
}