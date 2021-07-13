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
	public class ActionBulletTimeScale : IAction
	{
        public NumberProperty timeScale = new NumberProperty(1.0f);
		[Range(0f, 5f)] public float transition = 1.0f;
        
		public NumberProperty waituntil = new NumberProperty(1.0f);
		public Easing.EaseType easing = Easing.EaseType.QuadInOut;


        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            float startTime = Time.unscaledTime;
            float initTimeScale = Time.timeScale;
            float initDeltaTime = Time.fixedDeltaTime;

            while (Time.unscaledTime - startTime < this.transition)
            {
                float t = (Time.unscaledTime - startTime) / this.transition;
                float value = Mathf.Lerp(initTimeScale, this.timeScale.GetValue(target), t);
                TimeManager.Instance.SetTimeScale(value);

                yield return null;
            }
            
		     yield return new WaitForSeconds(waituntil.GetValue(target));


	        startTime = Time.unscaledTime;
	        initTimeScale = Time.timeScale;
	        initDeltaTime = Time.fixedDeltaTime;

        	 while (Time.unscaledTime - startTime < this.transition)
        	{
	    	  float t = (Time.unscaledTime - startTime) / this.transition;
	        	 float value = Mathf.Lerp(initTimeScale, 1.0f, t);
	        	 TimeManager.Instance.SetTimeScale(value);

	        	yield return null;
        	}
 
            yield return 0;
        }

        #if UNITY_EDITOR
		public static new string NAME = "ActionPack1/Misc/Bullet Time Scale";
		private const string NODE_TITLE = "Bullet Time Scale";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";
		
		// PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty sptimeScale; 
		private SerializedProperty sptransition; 
		private SerializedProperty spwaituntil;
		private SerializedProperty speasing;

		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.sptimeScale = this.serializedObject.FindProperty("timeScale"); 
			this.sptransition = this.serializedObject.FindProperty("transition"); 
			this.spwaituntil = this.serializedObject.FindProperty("waituntil");
			this.speasing = this.serializedObject.FindProperty("easing");
		}

		protected override void OnDisableEditorChild ()
		{
			this.sptimeScale = null; 
			this.sptransition = null;
			this.spwaituntil = null;
			this.speasing = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.sptimeScale, new GUIContent("Bullet Time Scale"));
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.sptransition, new GUIContent("Transition"));
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spwaituntil, new GUIContent("Elapsed Bullet Time"));
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.speasing, new GUIContent("Easing Effect"));
			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}

