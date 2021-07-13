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
	public class ActionRepelAllObjectsbyRigidBody : IAction
	{
		
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
		private Vector3 thrust;
		
		public TargetPosition position = new TargetPosition(TargetPosition.Target.Invoker);

		 public NumberProperty radius = new NumberProperty(10f);
		public NumberProperty xforce = new NumberProperty(0f);
		public NumberProperty yforce = new NumberProperty(0f);
		public NumberProperty zforce = new NumberProperty(0f);


        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{

			Vector3 Position = ( this.position.GetPosition(target)	);

				
			
			float Radius = this.radius.GetValue(target);
	
			newForce = new Vector3(this.xforce.GetValue(target), this.yforce.GetValue(target), this.zforce.GetValue(target));

				
			Collider[] hitColliders = Physics.OverlapSphere(Position, Radius);
			foreach (var hitCollider in hitColliders)
			{
				Rigidbody targetRB = hitCollider.GetComponent<Rigidbody>();
				
				if (targetRB != null)
				{
					
			
				
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
				
		
				}
				
			}
	

	
			
			return true;
        }

    

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack1/Objects/Repel All Objects with RigidBody";
		private const string NODE_TITLE = "Repel All Objects with RigidBody";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty spTarget; 
		private SerializedProperty spRadius; 
		private SerializedProperty spxForce; 
		private SerializedProperty spyForce; 
		private SerializedProperty spzForce; 
		private SerializedProperty spmode; 

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.spTarget = this.serializedObject.FindProperty("position"); 
			this.spRadius = this.serializedObject.FindProperty("radius"); 
			this.spxForce = this.serializedObject.FindProperty("xforce"); 
			this.spyForce = this.serializedObject.FindProperty("yforce"); 
			this.spzForce = this.serializedObject.FindProperty("zforce"); 
			this.spmode = this.serializedObject.FindProperty("mode"); 
		}

        protected override void OnDisableEditorChild ()
		{
            this.spTarget = null; 
			this.spRadius = null; 
			this.spxForce = null; 
			this.spyForce = null; 
			this.spzForce = null; 
			this.spmode = null; 

        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spTarget, new GUIContent("Repelling Game Object"));          
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spRadius, new GUIContent("Radius")); 
			EditorGUILayout.Space();
			EditorGUI.indentLevel++;

			EditorGUILayout.PropertyField(this.spxForce, new GUIContent("xAxis Force")); 
			EditorGUILayout.PropertyField(this.spyForce, new GUIContent("yAxis Force")); 
			EditorGUILayout.PropertyField(this.spzForce, new GUIContent("zAxis Force")); 
			EditorGUI.indentLevel--;

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spmode, new GUIContent("Force Mode")); 
			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
