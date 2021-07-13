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
	public class Action3DTextMeshChangeSize : IAction
	{
    
        public GameObject textObject;
        private TMPro.TextMeshPro textdata;

        public NumberProperty textsize = new NumberProperty(6f);
 

        public bool textAutoSize = false;
  
        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
          
         
            return false;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            textdata = textObject.GetComponent<TMPro.TextMeshPro>();

    

            if (textAutoSize == false)
            {
                textdata.autoSizeTextContainer = false;
                textdata.fontSize = textsize.GetValue(target);
            }
                
            else
            {
                textdata.autoSizeTextContainer = true;
            }
                
  
            textdata.ForceMeshUpdate();

            yield return 0;
        }


      
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack1/TMP/3D TextMesh Change Size";
		private const string NODE_TITLE = "Change 3D TextMeshPro Size";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptextmesh;
        private SerializedProperty spColortextsize;
        private SerializedProperty spAutoSize;
 
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{

             return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.sptextmesh = this.serializedObject.FindProperty("textObject");
            this.spColortextsize = this.serializedObject.FindProperty("textsize");
            this.spAutoSize = this.serializedObject.FindProperty("textAutoSize");
        }

        protected override void OnDisableEditorChild ()
		{
			this.sptextmesh = null;
           this.spColortextsize = null;
           this.spAutoSize = null;
        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.sptextmesh, new GUIContent("TextMeshPro Object"));
            EditorGUILayout.Space();

  

            EditorGUILayout.PropertyField(this.spAutoSize, new GUIContent("Text AutoSize"));
            EditorGUI.indentLevel++;
            if (textAutoSize == false)
            {
                EditorGUILayout.PropertyField(this.spColortextsize, new GUIContent("Text size"));
            }
            EditorGUI.indentLevel--;
  
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
