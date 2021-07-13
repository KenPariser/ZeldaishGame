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
	public class Action3DTextMeshChangeOutline : IAction
	{
    
        public GameObject textObject;
        private TMPro.TextMeshPro textdata;

         public ColorProperty outlinecolor = new ColorProperty(Color.black);

		public NumberProperty outlinewidth = new NumberProperty(0.1f);


        public bool textOutline = false;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
          
         
            return false;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            textdata = textObject.GetComponent<TMPro.TextMeshPro>();

          
            if (textOutline == true)
            {
                textdata.outlineColor = outlinecolor.GetValue(target);

                textdata.outlineWidth = outlinewidth.GetValue(target);
            }
            else
            {

                textdata.outlineWidth = 0;
            }

  
            textdata.fontSharedMaterial.EnableKeyword("OUTLINE_ON");
            textdata.ForceMeshUpdate();

            yield return 0;
        }


      
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack1/TMP/3D TextMesh Change Outline";
		private const string NODE_TITLE = "Change 3D TextMeshPro Outline";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptextmesh;
        private SerializedProperty spColoroutline;
        private SerializedProperty spColoroutlinesize;
        private SerializedProperty spOutline;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{

             return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.sptextmesh = this.serializedObject.FindProperty("textObject");
           this.spColoroutline = this.serializedObject.FindProperty("outlinecolor");
            this.spColoroutlinesize = this.serializedObject.FindProperty("outlinewidth");
           this.spOutline = this.serializedObject.FindProperty("textOutline");
        }

        protected override void OnDisableEditorChild ()
		{
			this.sptextmesh = null;
            this.spColoroutline = null;
           this.spColoroutlinesize = null;
            this.spOutline = null;
        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.sptextmesh, new GUIContent("TextMeshPro Object"));
            EditorGUILayout.Space();

  
            EditorGUILayout.PropertyField(this.spOutline, new GUIContent("Text Outline"));
            EditorGUI.indentLevel++;
            if (textOutline == true)
            {
                EditorGUILayout.PropertyField(this.spColoroutlinesize, new GUIContent("Outline size"));
                EditorGUILayout.PropertyField(this.spColoroutline, new GUIContent("Outline colour"));
            }
            EditorGUI.indentLevel--;
        
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
