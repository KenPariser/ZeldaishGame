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
	public class Action3DTextMeshChangeContent : IAction
	{
    
        public GameObject textObject;
        private TMPro.TextMeshPro textdata;

 

        public string content = "";

  

        // EXECUTABLE: ----------------------------------------------------------------------------

 

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            textdata = textObject.GetComponent<TMPro.TextMeshPro>();

   
                
                textdata.text = this.content;


          
            textdata.ForceMeshUpdate();

            yield return 0;
        }


      
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack1/TMP/3D TextMesh Change Content";
		private const string NODE_TITLE = "Change 3D TextMeshPro Content";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptextmesh;
          private SerializedProperty spContent;
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{

             return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.sptextmesh = this.serializedObject.FindProperty("textObject");
               this.spContent = this.serializedObject.FindProperty("content");
        }

        protected override void OnDisableEditorChild ()
		{
			this.sptextmesh = null;
            this.spContent = null;
        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.sptextmesh, new GUIContent("TextMeshPro Object"));
            EditorGUILayout.Space();

    
            EditorGUILayout.PropertyField(this.spContent, new GUIContent("New Text Content"));
        
     
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
