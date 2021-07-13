﻿namespace GameCreator.UIComponents
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
	public class ActionSlideElementIn : IAction
	{

		public GameObject canvasElement;
		public RectTransform animElement;

        public enum ALIGN
        {
            Top,
            Bottom,
            Left,
	        Right,
	        TopLeftCorner,
	        TopRightCorner,
	        BottomLeftCorner,
	        BottomRightCorner
        }
        public ALIGN alignment = ALIGN.Top;

        public NumberProperty AnimateDuration = new NumberProperty(1.0f);
		public Easing.EaseType easing = Easing.EaseType.BackInOut;

		public Vector3 OriginalStartPosition;
		public Vector3 animStartPosition;
		public Vector3 AnimInPosition;
   		public Vector3 AnimOutPositionTop;
		public Vector3 AnimOutPositionBottom;
		public Vector3 AnimOutPositionLeft;
		public Vector3 AnimOutPositionRight;
		public Vector3 AnimOutPositionTopLeft;
		public Vector3 AnimOutPositionTopRight;
		public Vector3 AnimOutPositionBottomLeft;
		public Vector3 AnimOutPositionBottomRight;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
          
         
            return false;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
           
            animElement = canvasElement.GetComponent<RectTransform>();

			
	        var pos = (VariablesManager.GetLocal(canvasElement, "originalPosition", true));
	  	     OriginalStartPosition =  (Vector3)pos;


	        AnimInPosition = new Vector3(OriginalStartPosition.x, OriginalStartPosition.y, OriginalStartPosition.z);
	        AnimOutPositionTop = new Vector3(animElement.localPosition.x, Screen.height, animElement.localPosition.z);
	        AnimOutPositionBottom = new Vector3(animElement.localPosition.x, -Screen.height, animElement.localPosition.z);
	        AnimOutPositionLeft = new Vector3(-Screen.width, animElement.localPosition.y, animElement.localPosition.z);
	        AnimOutPositionRight = new Vector3(Screen.width, animElement.localPosition.y, animElement.localPosition.z);
	        AnimOutPositionTopLeft = new Vector3(Screen.width,Screen.height, animElement.localPosition.z);
	        AnimOutPositionTopRight = new Vector3(-Screen.width, Screen.height, animElement.localPosition.z);
	        AnimOutPositionBottomRight = new Vector3(Screen.width, -Screen.height, animElement.localPosition.z);
	        AnimOutPositionBottomLeft = new Vector3(-Screen.width, -Screen.height, animElement.localPosition.z);

            float vMoveSpeed = AnimateDuration.GetValue(target);
            if (vMoveSpeed < 0.2f) vMoveSpeed = 0.2f;

            float initTime = Time.unscaledTime;

           
                switch (this.alignment)
                {
                    case ALIGN.Top:
                        while (Time.unscaledTime - initTime < vMoveSpeed)
                        {
                            if (animElement == null) break;
                            float t = (Time.unscaledTime - initTime) / vMoveSpeed;
                            float easeValue = Easing.GetEase(easing, 0.0f, 1.0f, t);

                        

                            animElement.localPosition = Vector3.Lerp(
								 AnimOutPositionTop,
								  AnimInPosition,
								 easeValue
                        );

                           yield return null;
                        }

                        break;
                    case ALIGN.Bottom:
                        while (Time.unscaledTime - initTime < vMoveSpeed)
                        {
                            if (animElement == null) break;
                            float t = (Time.unscaledTime - initTime) / vMoveSpeed;
                            float easeValue = Easing.GetEase(easing, 0.0f, 1.0f, t);

                            animElement.localPosition = Vector3.Lerp(
								 AnimOutPositionBottom,
								  AnimInPosition,
								 easeValue
                        );
                           yield return null;
                        }
                        break;
                    case ALIGN.Left:
					
						while (Time.unscaledTime - initTime < vMoveSpeed)
                        {
                            if (animElement == null) break;
                            float t = (Time.unscaledTime - initTime) / vMoveSpeed;
                            float easeValue = Easing.GetEase(easing, 0.0f, 1.0f, t);

                            animElement.localPosition = Vector3.Lerp(
								 AnimOutPositionLeft,
								  AnimInPosition,
								 easeValue
                        );
                           yield return null;
                        }

                        break;
                    case ALIGN.Right:
                        while (Time.unscaledTime - initTime < vMoveSpeed)
                        {
                            if (animElement == null) break;
                            float t = (Time.unscaledTime - initTime) / vMoveSpeed;
                            float easeValue = Easing.GetEase(easing, 0.0f, 1.0f, t);

                            animElement.localPosition = Vector3.Lerp(
								 AnimOutPositionRight,
								  AnimInPosition,
                                 easeValue
                        );
                           yield return null;
                        }

	      
	                    break;
                    case ALIGN.TopLeftCorner:
	                    while (Time.unscaledTime - initTime < vMoveSpeed)
	                    {
		                    if (animElement == null) break;
		                    float t = (Time.unscaledTime - initTime) / vMoveSpeed;
		                    float easeValue = Easing.GetEase(easing, 0.0f, 1.0f, t);

                        

		                    animElement.localPosition = Vector3.Lerp(
			                    AnimOutPositionTopLeft,
			                    AnimInPosition,
			                    easeValue
		                    );

		                    yield return null;
	                    }

	                    break;
                    case ALIGN.TopRightCorner:
	                    while (Time.unscaledTime - initTime < vMoveSpeed)
	                    {
		                    if (animElement == null) break;
		                    float t = (Time.unscaledTime - initTime) / vMoveSpeed;
		                    float easeValue = Easing.GetEase(easing, 0.0f, 1.0f, t);

		                    animElement.localPosition = Vector3.Lerp(
			                    AnimOutPositionTopRight,
			                    AnimInPosition,
			                    easeValue
		                    );
		                    yield return null;
	                    }
	                    break;
                    case ALIGN.BottomLeftCorner:
					
	                    while (Time.unscaledTime - initTime < vMoveSpeed)
	                    {
		                    if (animElement == null) break;
		                    float t = (Time.unscaledTime - initTime) / vMoveSpeed;
		                    float easeValue = Easing.GetEase(easing, 0.0f, 1.0f, t);

		                    animElement.localPosition = Vector3.Lerp(
			                    AnimOutPositionBottomLeft,
			                    AnimInPosition,
			                    easeValue
		                    );
		                    yield return null;
	                    }

	                    break;
                    case ALIGN.BottomRightCorner:
	                    while (Time.unscaledTime - initTime < vMoveSpeed)
	                    {
		                    if (animElement == null) break;
		                    float t = (Time.unscaledTime - initTime) / vMoveSpeed;
		                    float easeValue = Easing.GetEase(easing, 0.0f, 1.0f, t);

		                    animElement.localPosition = Vector3.Lerp(
			                    AnimOutPositionBottomRight,
			                    AnimInPosition,
			                    easeValue
		                    );
		                    yield return null;
	                    }

	      
	                    break;
                     }


	        yield return new WaitForSeconds(0.2f);

        
        }

	
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "UI/Elements/Slide Element In";
		private const string NODE_TITLE = "Slide {0} In";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spcanvas;
        private SerializedProperty spAlignment;
        private SerializedProperty spAnimstate;
        private SerializedProperty spAnimDuration;
 
        // INSPECTOR METHODS: ---------------------------------------------------------------------




        public override string GetNodeTitle()
		{
			if 	(canvasElement != null)		
				return string.Format(NODE_TITLE, canvasElement.name);
			else 	
				return string.Format(NODE_TITLE, "");

		}

		protected override void OnEnableEditorChild ()
		{
	
            this.spcanvas = this.serializedObject.FindProperty("canvasElement");
            this.spAlignment = this.serializedObject.FindProperty("alignment");
            this.spAnimstate = this.serializedObject.FindProperty("animState");
            this.spAnimDuration = this.serializedObject.FindProperty("AnimateDuration");
    
        }

        protected override void OnDisableEditorChild ()
		{
	
            this.spcanvas = null;
            this.spAlignment = null;
            this.spAnimstate = null;
            this.spAnimDuration = null;
     
        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.PropertyField(this.spcanvas, new GUIContent("Canvas Element"));
              EditorGUILayout.Space();
     
            EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(this.spAlignment, new GUIContent("Slide in from"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spAnimDuration, new GUIContent("Duration"));
            EditorGUILayout.Space();
            EditorGUI.indentLevel--;
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
