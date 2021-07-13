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
	public class Action3DTextMeshChange : IAction
	{
    
        public GameObject textObject;
        private TMPro.TextMeshPro textdata;

        public TMPro.TMP_FontAsset font;

        public ColorProperty textcolor = new ColorProperty(Color.white);
        public ColorProperty outlinecolor = new ColorProperty(Color.black);

        public NumberProperty textsize = new NumberProperty(6f);
        public NumberProperty outlinewidth = new NumberProperty(1f);

        public string content = "";

        public enum ALIGN
        {
            Left,
            Center,
            Right,
            Justified
        }
        public ALIGN alignment = ALIGN.Left;

        public bool textAutoSize = false;
        public bool textOutline = false;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
          
         
            return false;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            textdata = textObject.GetComponent<TMPro.TextMeshPro>();

            if (this.font != null)
                textdata.font = font;

                textdata.color = textcolor.GetValue(target);
            if (textOutline == true)
            {
                textdata.outlineColor = outlinecolor.GetValue(target);

                textdata.outlineWidth = outlinewidth.GetValue(target);
            }
            else
            {

                textdata.outlineWidth = 0;
            }

            if (textAutoSize == false)
            {
                textdata.autoSizeTextContainer = false;
                textdata.fontSize = textsize.GetValue(target);
            }
                
            else
            {
                textdata.autoSizeTextContainer = true;
            }
                
                textdata.text = this.content;


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


            textdata.fontSharedMaterial.EnableKeyword("OUTLINE_ON");
            textdata.ForceMeshUpdate();

            yield return 0;
        }


      
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack1/TMP/3D TextMesh Change All";
		private const string NODE_TITLE = "Change 3D TextMeshPro All Properties";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptextmesh;
        private SerializedProperty spfont;
        private SerializedProperty spColortext;
        private SerializedProperty spColoroutline;
        private SerializedProperty spColortextsize;
        private SerializedProperty spColoroutlinesize;
        private SerializedProperty spAlignment;
        private SerializedProperty spContent;
        private SerializedProperty spAutoSize;
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
            this.spColortext = this.serializedObject.FindProperty("textcolor");
            this.spColoroutline = this.serializedObject.FindProperty("outlinecolor");
            this.spColortextsize = this.serializedObject.FindProperty("textsize");
            this.spColoroutlinesize = this.serializedObject.FindProperty("outlinewidth");
            this.spContent = this.serializedObject.FindProperty("content");
            this.spAlignment = this.serializedObject.FindProperty("alignment");
            this.spAutoSize = this.serializedObject.FindProperty("textAutoSize");
            this.spOutline = this.serializedObject.FindProperty("textOutline");
        }

        protected override void OnDisableEditorChild ()
		{
			this.sptextmesh = null;
            this.spfont = null;
            this.spColortext = null;
            this.spColoroutline = null;
            this.spColortextsize = null;
            this.spColoroutlinesize = null;
            this.spAlignment = null;
            this.spContent = null;
            this.spAutoSize = null;
            this.spOutline = null;
        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.sptextmesh, new GUIContent("TextMeshPro Object"));
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(this.spfont, new GUIContent("New TMP Font"));
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(this.spContent, new GUIContent("New Text Content"));
        
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(new GUIContent("Update Properties"));
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(this.spAlignment, new GUIContent("Text alignment"));
            EditorGUILayout.PropertyField(this.spColortext, new GUIContent("Text colour"));

            EditorGUILayout.PropertyField(this.spAutoSize, new GUIContent("Text AutoSize"));
            EditorGUI.indentLevel++;
            if (textAutoSize == false)
            {
                EditorGUILayout.PropertyField(this.spColortextsize, new GUIContent("Text size"));
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.PropertyField(this.spOutline, new GUIContent("Text Outline"));
            EditorGUI.indentLevel++;
            if (textOutline == true)
            {
                EditorGUILayout.PropertyField(this.spColoroutlinesize, new GUIContent("Outline size"));
                EditorGUILayout.PropertyField(this.spColoroutline, new GUIContent("Outline colour"));
            }
            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
