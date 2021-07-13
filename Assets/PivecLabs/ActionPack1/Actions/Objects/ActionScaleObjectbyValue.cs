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
	public class ActionScaleObjectbyValue : IAction
	{
		public TargetGameObject target = new TargetGameObject();
		public GameObject objecttoScale;
        private float originalValy;
        private float originalValx;
        private float originalValz;
        private Vector3 objOrigPos;
        private Vector3 objNewPos;
		public Vector3 rotation = new Vector3(90f, 0f, 0f);

        public NumberProperty transform_y = new NumberProperty(0.0f);
        public NumberProperty transform_x = new NumberProperty(0.0f);
        public NumberProperty transform_z = new NumberProperty(0.0f);
        public NumberProperty moveSpeed = new NumberProperty(1.0f);
 
        public bool xvalue;
        public bool yvalue;
        public bool zvalue;

        public bool returnTo;
        public bool returnToWait;
        public NumberProperty waituntilreturn = new NumberProperty(1.0f);
		public Easing.EaseType easing = Easing.EaseType.QuadInOut;

        private Transform objectTransform;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
         

            return false;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {

	        objecttoScale = this.target.GetGameObject(target);
	        objectTransform = objecttoScale.GetComponent<Transform>();

	        objOrigPos = objectTransform.localScale;

	        originalValx = objectTransform.localScale.x;
	        originalValy = objectTransform.localScale.y;
	        originalValz = objectTransform.localScale.z;
	        
	        if (xvalue == true)
	        { 
		        objNewPos.x = originalValx + transform_x.GetValue(target);
	        }
	        else 
	        {
	        	objNewPos.x = originalValx;
	        }
	        
	        if (yvalue == true)
	        { 
		        objNewPos.y = originalValy + transform_y.GetValue(target);
	        }
	        else 
	        {
	        	objNewPos.y = originalValy;
	        }
	
	        if (zvalue == true)
	        { 
		        objNewPos.z = originalValz + transform_z.GetValue(target);
	        }
	        else 
	        {
	        	objNewPos.z = originalValz;
	        }
	
  

	            float vMoveSpeed = moveSpeed.GetValue(target);
	            float initTime = Time.time;

	            while (Time.time - initTime < vMoveSpeed)
	            {
		            if (objectTransform == null) break;
		            float t = (Time.time - initTime) / vMoveSpeed;
		            float easeValue = Easing.GetEase(easing, 0.0f, 1.0f, t);

  
		            objectTransform.localScale = Vector3.Lerp(
			            objOrigPos,
			            objNewPos,
			            easeValue
		            );


                yield return null;
            }

            if (returnTo == true)

            {
                if (returnToWait == true)
                {
                    yield return new WaitForSeconds(waituntilreturn.GetValue(target));
                }

                initTime = Time.time;

	            while (Time.time - initTime < vMoveSpeed)
	            {
		            if (objectTransform == null) break;
		            float t = (Time.time - initTime) / vMoveSpeed;
		            float easeValue = Easing.GetEase(easing, 0.0f, 1.0f, t);

  
		
		            objectTransform.localScale = Vector3.Lerp(
			            objNewPos,
			            objOrigPos,
			            easeValue
		            );



                    yield return null;
                }

             

            }
	        yield return 0;
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

        #if UNITY_EDITOR

		public static new string NAME = "ActionPack1/Objects/Scale Object by Value";
		private const string NODE_TITLE = "Scale Object by Value";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";


        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spEasing; 
        private SerializedProperty spScaleObject; 
        private SerializedProperty spScaleObjectTransformY;
        private SerializedProperty spScaleObjectTransformX;
        private SerializedProperty spScaleObjectTransformZ;
        private SerializedProperty spScaleObjectSpeed;
        private SerializedProperty spxValue;
        private SerializedProperty spyValue;
        private SerializedProperty spzValue;
        private SerializedProperty spreturnTo;
        private SerializedProperty spreturnToWait;
        private SerializedProperty spwaituntilreturn;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
            this.spEasing = this.serializedObject.FindProperty("easing"); 
			this.spScaleObject = this.serializedObject.FindProperty("target"); 
			this.spScaleObjectTransformY = this.serializedObject.FindProperty("transform_y");
			this.spScaleObjectTransformX = this.serializedObject.FindProperty("transform_x");
			this.spScaleObjectTransformZ = this.serializedObject.FindProperty("transform_z");
			this.spScaleObjectSpeed = this.serializedObject.FindProperty("moveSpeed");
            this.spxValue = this.serializedObject.FindProperty("xvalue");
            this.spyValue = this.serializedObject.FindProperty("yvalue");
            this.spzValue = this.serializedObject.FindProperty("zvalue");
            this.spreturnTo = this.serializedObject.FindProperty("returnTo");
            this.spreturnToWait = this.serializedObject.FindProperty("returnToWait");
            this.spwaituntilreturn = this.serializedObject.FindProperty("waituntilreturn");
        }

        protected override void OnDisableEditorChild ()
		{
            this.spScaleObject = null; 
            this.spScaleObjectTransformY = null;
            this.spScaleObjectTransformX = null;
            this.spScaleObjectTransformZ = null;
            this.spScaleObjectSpeed = null;
            this.spxValue = null;
            this.spyValue = null;
            this.spzValue = null;
            this.spreturnTo = null;
            this.spreturnToWait = null;
			this.spwaituntilreturn = null;
			this.spEasing = null;
        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.spScaleObject, new GUIContent("Object to Scale"));
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(new GUIContent("Axis to Move"));
            EditorGUI.indentLevel++;
                     EditorGUILayout.PropertyField(this.spxValue, new GUIContent("x"));
            if (xvalue)
            { EditorGUILayout.PropertyField(this.spScaleObjectTransformX, new GUIContent("x value")); }
            EditorGUILayout.PropertyField(this.spyValue, new GUIContent("y"));
            if (yvalue)
            { EditorGUILayout.PropertyField(this.spScaleObjectTransformY, new GUIContent("y value")); }
            EditorGUILayout.PropertyField(this.spzValue, new GUIContent("z"));
            if (zvalue)
            { EditorGUILayout.PropertyField(this.spScaleObjectTransformZ, new GUIContent("z value")); }
            EditorGUILayout.Space();
            EditorGUI.indentLevel--;
			EditorGUILayout.PropertyField(this.spScaleObjectSpeed, new GUIContent("Time to arrive"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spreturnTo, new GUIContent("Return Object"));
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(this.spreturnToWait, new GUIContent("Wait before return"));
            if (returnToWait)
            { EditorGUILayout.PropertyField(this.spwaituntilreturn, new GUIContent("Time to wait")); }
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spEasing, new GUIContent("Easing Effect"));

            EditorGUI.indentLevel--;
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
