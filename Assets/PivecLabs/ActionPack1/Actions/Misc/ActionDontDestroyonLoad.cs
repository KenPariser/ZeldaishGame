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
    public class ActionDontDestroyonLoad : IAction
    {

	    public TargetGameObject target = new TargetGameObject();

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {

	        GameObject targetObject = this.target.GetGameObject(target);
	        if (targetObject != null) {
	        	
		        DontDestroyOnLoad(targetObject);
	        }
	        
            return true;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {

            return base.Execute(target, actions, index);
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

	    public static new string NAME = "ActionPack1/Misc/Dont Destroy on Load";
        private const string NODE_TITLE = "Dont Destroy on Load";
	    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptargetObject;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {
            return string.Format(
                 NODE_TITLE
             );
        }

        protected override void OnEnableEditorChild()
        {
            this.sptargetObject = this.serializedObject.FindProperty("target");
        }

        protected override void OnDisableEditorChild()
        {
            this.sptargetObject = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.sptargetObject, new GUIContent("Target Object"));

            this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
