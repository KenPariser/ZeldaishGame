using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PivecLabs.MinMaxSliderAttribute {


	[System.Serializable]
	public class MinMaxType<T> {
		public T min;
		public T max;
	}
	
	[System.Serializable]
	public class MinMaxInt : MinMaxType<int> { }
	
	
	[System.AttributeUsage(System.AttributeTargets.Field)]
	public class MinMaxRangeAttribute : PropertyAttribute {
		public readonly float MinLimit = 0;
		public readonly float MaxLimit = 1;

		public MinMaxRangeAttribute(int min, int max) {
			MinLimit = min;
			MaxLimit = max;
		}
	}
	

}
