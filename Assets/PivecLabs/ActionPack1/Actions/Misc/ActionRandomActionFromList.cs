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
	public class ActionRandomActionFromList : IAction
    {

        [System.Serializable]
        public class ActionObject
        {
            public Actions actionToExecute;
           
            [Range(1f,100)]
	        public int Probability;
            [HideInInspector]
	        public int actionweight;
        }

        [SerializeField]
        public List<ActionObject> ListofActions = new List<ActionObject>();

        Actions actionsToExecute = null;

 
  
        public bool waitToFinish = false;

        public bool executeAllActions = false;

        private bool actionsComplete = false;
        private bool forceStop = false;

    
        // EXECUTABLE: ----------------------------------------------------------------------------

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {


                int rand = RandomProbability();

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

            yield return 0;

        }


        public int RandomProbability()
        {

	        int weightTotal = 0;
            if (ListofActions.Capacity > 0)
            {
                for (int i = 0; i < ListofActions.Capacity; i++)
                {
                    weightTotal += ListofActions[i].Probability;

                }

                int result = 0, total = 0;
                int randVal = Random.Range(0, weightTotal);

                for (result = 0; result < ListofActions.Capacity; result++)
                {
                    total += ListofActions[result].Probability;
                    if (total > randVal) break;
                }

                return result;

            }
            return 0;
        }

      

    

        private void OnCompleteActions()
        {
            this.actionsComplete = true;
           
        }




        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
       
	    public static new string NAME = "ActionPack1/Misc/Random Action From List";
        private const string NODE_TITLE = "Execute Random Action from List";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

         private SerializedProperty spWaittoFinish;
        private SerializedProperty spsizeDistribution;




        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {

            return string.Format(NODE_TITLE);
        }


        protected override void OnEnableEditorChild()
        {
            this.spWaittoFinish = this.serializedObject.FindProperty("waitToFinish");
 



        }

        protected override void OnDisableEditorChild()
        {
            this.spWaittoFinish = null;
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
                    
	                SetProbabilityForList();
	                 
                EditorGUI.indentLevel--;
                }
            }

      
     private void SetProbabilityForList()
	    {
		    float tempCount = 0;
			    foreach (ActionObject action in ListofActions)
		    {
			    if (action.Probability == 0)
			    {
				    action.Probability = 100;
			    }
			    tempCount += action.Probability;
		    }

          

	    }

#endif
    }
}