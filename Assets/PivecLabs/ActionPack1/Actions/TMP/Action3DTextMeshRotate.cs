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
	public class Action3DTextMeshRotate : IAction
	{
        public GameObject textObject;
        private TMPro.TextMeshPro textdata;
        public TargetPosition lookAt = new TargetPosition(TargetPosition.Target.Invoker);
    
        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
           
    
            return false;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {

            Vector3 relativePos = (textObject.transform.position - this.lookAt.GetPosition(target));
            textObject.transform.rotation = Quaternion.LookRotation(relativePos);

            return base.Execute(target, actions, index);
        }


        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

        public static new string NAME = "ActionPack1/TMP/3D TextMesh Rotate";
		private const string NODE_TITLE = "Rotate 3D TestMeshPro";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptextmesh;
        private SerializedProperty spLookAt;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.sptextmesh = this.serializedObject.FindProperty("textObject");
            this.spLookAt = serializedObject.FindProperty("lookAt");

        }

        protected override void OnDisableEditorChild ()
		{
			this.sptextmesh = null;
            this.spLookAt = null;

        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

	        EditorGUILayout.PropertyField(this.sptextmesh, new GUIContent("TextMeshPro Object"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spLookAt);


            this.serializedObject.ApplyModifiedProperties();
		}

#endif
    }
}
