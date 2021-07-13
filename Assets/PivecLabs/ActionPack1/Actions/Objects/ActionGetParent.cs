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
    public class ActionGetParent : IAction
    {
 	    public TargetGameObject target = new TargetGameObject(TargetGameObject.Target.Player);

        [VariableFilter(Variable.DataType.GameObject)]
        public VariableProperty parentGameObject = new VariableProperty();

        private GameObject outputGameObject;
        private Transform parenttrans;
        
        public override bool InstantExecute (GameObject target, IAction[] actions, int index)
        {
	       
	        Transform targetTrans = this.target.GetTransform(target);
	        if (targetTrans != null)
	        {
		        parenttrans = targetTrans.transform.parent;
                outputGameObject = parenttrans.gameObject;
            }
            
            this.parentGameObject.Set(outputGameObject, target);

            return true;
        }

        #if UNITY_EDITOR
	    public static new string NAME = "ActionPack1/Objects/Get Parent";
	    private const string NODE_TITLE = "Get {0} Parent";
	    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";
	    // PROPERTIES: ----------------------------------------------------------------------------

	    private SerializedProperty spTarget;
	    private SerializedProperty spVariable;



	    // INSPECTOR METHODS: ---------------------------------------------------------------------

	    public override string GetNodeTitle()
	    {
		    return string.Format(NODE_TITLE, this.target.ToString());
	    }

	    protected override void OnEnableEditorChild ()
	    {
		    this.spTarget = serializedObject.FindProperty("target");
		    this.spVariable = serializedObject.FindProperty("parentGameObject");


	    }

	    protected override void OnDisableEditorChild ()
	    {
		    this.spTarget = null;
		    this.spVariable = null;
	    }

	    public override void OnInspectorGUI()
	    {
		    this.serializedObject.Update();
		    EditorGUILayout.Space();
		    EditorGUILayout.PropertyField(this.spTarget, new GUIContent("Target Object"));
		    EditorGUILayout.Space();
	
		    EditorGUILayout.PropertyField(this.spVariable, new GUIContent("Variable"));
		    EditorGUILayout.Space();


		    this.serializedObject.ApplyModifiedProperties();
	    }

		#endif
    }
}

