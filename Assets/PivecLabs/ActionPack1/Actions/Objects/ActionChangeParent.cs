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
	public class ActionChangeParent : IAction 
	{
		public enum RELATIVE
		{
			Local,
			Global
		}

		public enum PARENT
		{
			ChangeParent,
			ClearParent
		}

        // PROPERTIES: ----------------------------------------------------------------------------

        public TargetGameObject target = new TargetGameObject(TargetGameObject.Target.Player);

		public PARENT changeParent = PARENT.ChangeParent;
        public TargetGameObject newParent = new TargetGameObject(TargetGameObject.Target.GameObject);


        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            Transform targetTrans = this.target.GetTransform(target);
            if (targetTrans != null)
            {
                switch (this.changeParent)
                {
                    case PARENT.ChangeParent:
                        Transform newParentTransform = this.newParent.GetTransform(target);
                        if (newParentTransform != null) targetTrans.SetParent(newParentTransform);
                        break;

                    case PARENT.ClearParent:
                        targetTrans.SetParent(null);
                        break;
                }

   
            }

            return true;
        }

		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public static new string NAME = "ActionPack1/Objects/Change Parent";
		private const string NODE_TITLE = "Change {0} Parent";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

		// PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty spTarget;

		private SerializedProperty spChangeParent;
		private SerializedProperty spNewParent;


		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE, this.target.ToString());
		}

		protected override void OnEnableEditorChild ()
		{
			this.spTarget = serializedObject.FindProperty("target");

			this.spChangeParent = serializedObject.FindProperty("changeParent");
			this.spNewParent = serializedObject.FindProperty("newParent");

		}

		protected override void OnDisableEditorChild ()
		{
            this.spTarget = null;
			this.spChangeParent = null;
			this.spNewParent = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spTarget, new GUIContent("Target Object"));
			EditorGUILayout.Space();
	
			EditorGUILayout.PropertyField(this.spChangeParent);
			if (this.spChangeParent.intValue == (int)PARENT.ChangeParent)
			{
				EditorGUILayout.PropertyField(this.spNewParent);
                EditorGUILayout.Space();
			}


			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}