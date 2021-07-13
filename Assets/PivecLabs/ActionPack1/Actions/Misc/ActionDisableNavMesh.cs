namespace PivecLabs.ActionPack
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using UnityEngine.UI;
	using UnityEngine.Events;
	using GameCreator.Core;
    using GameCreator.Characters;
    using GameCreator.Core.Hooks;

#if UNITY_EDITOR
    using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ActionDisableNavMesh : IAction
	{
      
		public bool navmesh;
        public TargetCharacter character = new TargetCharacter(TargetCharacter.Target.Player);
        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {

            Character characterTarget = this.character.GetCharacter(target);
            if (characterTarget == null) return true;

            CharacterLocomotion locomotion = characterTarget.characterLocomotion;

            if (navmesh == true)
            {

                locomotion.canUseNavigationMesh = true;


            }
            else
            {

                locomotion.canUseNavigationMesh = false;

            }


            return true;
         
        }

  

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack1/Misc/Disable NavMesh";
		private const string NODE_TITLE = "{0} NavMesh";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spnavmesh;
        private SerializedProperty spcharacter;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {

	        return string.Format(
		        NODE_TITLE, 
		        (this.navmesh ? "Enable" : "Disable")
	        );
        }


        protected override void OnEnableEditorChild()
        {
   	        this.spnavmesh = this.serializedObject.FindProperty("navmesh");
            this.spcharacter = this.serializedObject.FindProperty("character");

        }

        protected override void OnDisableEditorChild()
        {
  	        this.spnavmesh = null;
            this.spcharacter = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            EditorGUILayout.LabelField("Enable or Disable NavMesh use", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(this.spcharacter, new GUIContent("Character"));
            EditorGUILayout.Space();
	        EditorGUILayout.LabelField(new GUIContent("NavMesh"));
	        EditorGUI.indentLevel++;
	        EditorGUILayout.PropertyField(this.spnavmesh, new GUIContent("Enable/Disable"));
	        EditorGUILayout.Space();
	  
	        EditorGUI.indentLevel--;
            this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
