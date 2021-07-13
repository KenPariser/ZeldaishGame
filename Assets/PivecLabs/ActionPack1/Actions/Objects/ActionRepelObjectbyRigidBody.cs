namespace PivecLabs.ActionPack
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
    using GameCreator.Core.Hooks;
    using GameCreator.Variables;

#if UNITY_EDITOR
    using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ActionRepelObjectbyRigidBody : IAction
	{
		
		public TargetGameObject target = new TargetGameObject();
		
		public TargetGameObject gravityObject = new TargetGameObject();
		public float GravityForce = 30.0f;
		
		public enum FORCEMODE
		{
			Acceleration,
			Impulse,
			Force,
			VelocityChange
		}
		
		public FORCEMODE mode = FORCEMODE.Force;
		private float intensity;
		private Vector3 newForce;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{

			GameObject gravityGO = this.gravityObject.GetGameObject(target);
				
			GameObject targetGO = this.target.GetGameObject(target);
			if (targetGO == null) return true;
			
			SphereCollider gravityObject = gravityGO.GetComponent<SphereCollider>();
			
			
	
			Rigidbody targetRB = targetGO.GetComponent<Rigidbody>();
			if (targetRB != null)
			{
				intensity = Vector3.Distance(gravityGO.transform.position, targetGO.transform.position) / 1;

				newForce = ((gravityGO.transform.position + targetGO.transform.position) * intensity * GravityForce * Time.deltaTime);

			}
			
			switch (this.mode)
		 {
		 case FORCEMODE.Acceleration:
			 targetRB.AddForce(newForce, ForceMode.Acceleration) ;
			 break;
		 case FORCEMODE.Impulse:
			 targetRB.AddForce(newForce, ForceMode.Impulse) ;
			 break;
		 case FORCEMODE.Force:
			 targetRB.AddForce(newForce, ForceMode.Force) ;
			 break;
		 case FORCEMODE.VelocityChange:
			 targetRB.AddForce(newForce, ForceMode.VelocityChange) ;
			 break;
		 }
			
			return true;
        }

    

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack1/Objects/Repel Object with RigidBody";
		private const string NODE_TITLE = "Repel Object with RigidBody";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty spTarget; 
		private SerializedProperty spTarget2; 
		private SerializedProperty spGravityForce; 
		private SerializedProperty spmode; 

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.spTarget = this.serializedObject.FindProperty("target"); 
			this.spTarget2 = this.serializedObject.FindProperty("gravityObject"); 
			this.spGravityForce = this.serializedObject.FindProperty("GravityForce"); 
			this.spmode = this.serializedObject.FindProperty("mode"); 
		}

        protected override void OnDisableEditorChild ()
		{
            this.spTarget = null; 
			this.spTarget2 = null; 
			this.spGravityForce = null; 
			this.spmode = null; 

        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.spTarget2, new GUIContent("Repelling Game Object")); 
			EditorGUILayout.Space();
			EditorGUILayout.LabelField(new GUIContent("Target Game Object"));
			EditorGUILayout.PropertyField(this.spTarget, new GUIContent("must have RigidBody"));          
			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(this.spGravityForce, new GUIContent("Force")); 
			EditorGUILayout.PropertyField(this.spmode, new GUIContent("Force Mode")); 
			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
