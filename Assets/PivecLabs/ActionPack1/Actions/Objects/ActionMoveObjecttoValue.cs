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
	public class ActionMoveObjecttoValue : IAction
	{
		public TargetGameObject target = new TargetGameObject();
		public GameObject objecttoMove;
        private float originalValy;
        private float originalValx;
        private float originalValz;
        private Vector3 objOrigPos;
        private Vector3 objNewPos;
  
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
	        objecttoMove = this.target.GetGameObject(target);

            objectTransform = objecttoMove.GetComponent<Transform>();

            objOrigPos = objectTransform.position;

            originalValx = objectTransform.position.x;
            originalValy = objectTransform.position.y;
            originalValz = objectTransform.position.z;
	        
	        if (xvalue == true)
	        { 
		        objNewPos.x = transform_x.GetValue(target);
	        }
	        else 
	        {
	        	objNewPos.x = originalValx;
	        }
	        
	        if (yvalue == true)
	        { 
		        objNewPos.y = transform_y.GetValue(target);
	        }
	        else 
	        {
	        	objNewPos.y = originalValy;
	        }
	
	        if (zvalue == true)
	        { 
		        objNewPos.z = transform_z.GetValue(target);
	        }
	        else 
	        {
	        	objNewPos.z = originalValz;
	        }
	
    

            float vMoveSpeed = moveSpeed.GetValue(target);
	        float initTime = Time.time;
            
	        if (vMoveSpeed > 0)
	        
	        {
		        while (Time.time - initTime < vMoveSpeed)
		        {
			        if (objectTransform == null) break;
			        float t = (Time.time - initTime) / vMoveSpeed;
			        float easeValue = Easing.GetEase(easing, 0.0f, 1.0f, t);

			        objectTransform.position = Vector3.Lerp(
				        objOrigPos,
				        objNewPos,
				        easeValue
			        );



			        yield return null;
		        }
	        	
	      

	        }
	        
	        else 
	        {
	        	
	         objectTransform.position = objNewPos;
		        
	        	
	        }
	        
	        
	        
	        if (returnTo == true)

            {
                if (returnToWait == true)
                {
                    yield return new WaitForSeconds(waituntilreturn.GetValue(target));
                }

		        initTime = Time.time;
                
		        if (vMoveSpeed > 0)

		        {
		        	
		       
                while (Time.time - initTime < vMoveSpeed)
                {
                    if (objectTransform == null) break;
                    float t = (Time.time - initTime) / vMoveSpeed;
                    float easeValue = Easing.GetEase(easing, 0.0f, 1.0f, t);

                    objectTransform.position = Vector3.Lerp(
                        objNewPos,
                        objOrigPos,
                        easeValue
                    );

                    yield return null;
            	 }
		        }

		        else
		        {
			        objectTransform.position = objOrigPos;
		        }
            }
            
	     
            
	        yield return 0;

        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

        #if UNITY_EDITOR

		public static new string NAME = "ActionPack1/Objects/Move Object to Value";
		private const string NODE_TITLE = "Move Object to Value";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";


        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spEasing; 
        private SerializedProperty spMoveObject; 
        private SerializedProperty spMoveObjectTransformY;
        private SerializedProperty spMoveObjectTransformX;
        private SerializedProperty spMoveObjectTransformZ;
        private SerializedProperty spMoveObjectSpeed;
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
			this.spMoveObject = this.serializedObject.FindProperty("target"); 
			this.spMoveObjectTransformY = this.serializedObject.FindProperty("transform_y");
            this.spMoveObjectTransformX = this.serializedObject.FindProperty("transform_x");
            this.spMoveObjectTransformZ = this.serializedObject.FindProperty("transform_z");
            this.spMoveObjectSpeed = this.serializedObject.FindProperty("moveSpeed");
            this.spxValue = this.serializedObject.FindProperty("xvalue");
            this.spyValue = this.serializedObject.FindProperty("yvalue");
            this.spzValue = this.serializedObject.FindProperty("zvalue");
            this.spreturnTo = this.serializedObject.FindProperty("returnTo");
            this.spreturnToWait = this.serializedObject.FindProperty("returnToWait");
            this.spwaituntilreturn = this.serializedObject.FindProperty("waituntilreturn");
        }

        protected override void OnDisableEditorChild ()
		{
            this.spMoveObject = null; 
            this.spMoveObjectTransformY = null;
            this.spMoveObjectTransformX = null;
            this.spMoveObjectTransformZ = null;
            this.spMoveObjectSpeed = null;
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

            EditorGUILayout.PropertyField(this.spMoveObject, new GUIContent("Object to Move"));
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(new GUIContent("Axis to Move"));
            EditorGUI.indentLevel++;
                     EditorGUILayout.PropertyField(this.spxValue, new GUIContent("x"));
            if (xvalue)
            { EditorGUILayout.PropertyField(this.spMoveObjectTransformX, new GUIContent("x to move")); }
            EditorGUILayout.PropertyField(this.spyValue, new GUIContent("y"));
            if (yvalue)
            { EditorGUILayout.PropertyField(this.spMoveObjectTransformY, new GUIContent("y to move")); }
            EditorGUILayout.PropertyField(this.spzValue, new GUIContent("z"));
            if (zvalue)
            { EditorGUILayout.PropertyField(this.spMoveObjectTransformZ, new GUIContent("z to move")); }
            EditorGUILayout.Space();
            EditorGUI.indentLevel--;
			EditorGUILayout.PropertyField(this.spMoveObjectSpeed, new GUIContent("Time to arrive"));
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
