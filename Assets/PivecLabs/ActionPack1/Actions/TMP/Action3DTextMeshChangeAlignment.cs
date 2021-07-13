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
	public class Action3DTextMeshChangeAlignment : IAction
	{
    
        public GameObject textObject;
        private TMPro.TextMeshPro textdata;
   
        public enum ALIGN
        {
            Left,
            Center,
            Right,
            Justified
        }
        public ALIGN alignment = ALIGN.Left;

  
        // EXECUTABLE: ----------------------------------------------------------------------------



        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            textdata = textObject.GetComponent<TMPro.TextMeshPro>();

             switch (this.alignment)
            {
                case ALIGN.Left:
                    textdata.alignment = TMPro.TextAlignmentOptions.Left;
                    break;
                case ALIGN.Center:
                    textdata.alignment = TMPro.TextAlignmentOptions.Center;
                    break;
                case ALIGN.Right:
                    textdata.alignment = TMPro.TextAlignmentOptions.Right;
                    break;
                case ALIGN.Justified:
                    textdata.alignment = TMPro.TextAlignmentOptions.Justified;
                    break;
            }

        
            textdata.ForceMeshUpdate();

            yield return 0;
        }

      
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack1/TMP/3D TextMesh Change Alignment";
		private const string NODE_TITLE = "Change 3D TextMeshPro Alignment";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptextmesh;
        private SerializedProperty spAlignment;
  
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{

             return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.sptextmesh = this.serializedObject.FindProperty("textObject");
            this.spAlignment = this.serializedObject.FindProperty("alignment");
       }

        protected override void OnDisableEditorChild ()
		{
			this.sptextmesh = null;
            this.spAlignment = null;
       }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.sptextmesh, new GUIContent("TextMeshPro Object"));
            EditorGUILayout.Space();     
            EditorGUILayout.PropertyField(this.spAlignment, new GUIContent("Text alignment"));
 
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
