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
	public class ActionDontDestroyonLoadList : IAction
    {

	    
	    [System.Serializable]
	    public class TargetObjects
	    {
		    public TargetGameObject target = new TargetGameObject(TargetGameObject.Target.GameObject);
 
		}

	    [SerializeField]
	    public List<TargetObjects> ListofObjects = new List<TargetObjects>();
	    
        // EXECUTABLE: ----------------------------------------------------------------------------

    

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {

	        for (int i = 0; i < ListofObjects.Count; i++)
	        {
		        GameObject targetGO = this.ListofObjects[i].target.GetGameObject(target);
  
		        DontDestroyOnLoad(targetGO);
	        }
	        
            return base.Execute(target, actions, index);
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

	    public static new string NAME = "ActionPack1/Misc/Dont Destroy on Load List";
	    private const string NODE_TITLE = "Dont Destroy on Load List";
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
            this.sptargetObject = this.serializedObject.FindProperty("ListofObjects");
        }

        protected override void OnDisableEditorChild()
        {
            this.sptargetObject = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

	        EditorGUILayout.LabelField("Object List");

	        SerializedProperty property = serializedObject.FindProperty("ListofObjects");
	        ArrayGUI(property, "GameObject ", true);
	        EditorGUILayout.Space();
  

            this.serializedObject.ApplyModifiedProperties();
        }
	    private void ArrayGUI(SerializedProperty property, string itemType, bool visible)
	    {

		    {

			    EditorGUI.indentLevel++;
			    SerializedProperty arraySizeProp = property.FindPropertyRelative("Array.size");
			    EditorGUILayout.PropertyField(arraySizeProp);
             
			    for (int i = 0; i < arraySizeProp.intValue; i++)
			    {
				    EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i), new GUIContent(itemType + (i +1).ToString()), true);
                   
			    }
 			    EditorGUI.indentLevel--;
		    }
	    }

#endif
    }
}
