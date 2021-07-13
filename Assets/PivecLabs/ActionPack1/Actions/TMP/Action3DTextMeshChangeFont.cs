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
	public class Action3DTextMeshChangeFont : IAction
	{
    
        public GameObject textObject;
        private TMPro.TextMeshPro textdata;

        public TMPro.TMP_FontAsset font;

        // EXECUTABLE: ----------------------------------------------------------------------------


        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            textdata = textObject.GetComponent<TMPro.TextMeshPro>();

            if (this.font != null)
                textdata.font = font;

   
            textdata.ForceMeshUpdate();

            yield return 0;
        }


      
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack1/TMP/3D TextMesh Change Font";
		private const string NODE_TITLE = "Change 3D TextMeshPro Font";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptextmesh;
        private SerializedProperty spfont;
         private SerializedProperty spOutline;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{

             return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.sptextmesh = this.serializedObject.FindProperty("textObject");
            this.spfont = this.serializedObject.FindProperty("font");
        }

        protected override void OnDisableEditorChild ()
		{
			this.sptextmesh = null;
            this.spfont = null;
        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.sptextmesh, new GUIContent("TextMeshPro Object"));
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(this.spfont, new GUIContent("New TMP Font"));
            EditorGUILayout.Space();

  
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
