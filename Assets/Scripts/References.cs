using LittleKingdom.Board;
using UnityEngine;

namespace LittleKingdom
{
	//This class is loaded before the default load time, so references are ensured to be set up before being called.
	//This should only be placed on one GameObject.
	// TODO: JR - dissolve into a Zenject installer.
	public class References : MonoBehaviour
	{
		private static References instance;

		public static int DefaultLayer => instance.defaultLayer;
		[SerializeField] private int defaultLayer;

		public static int IgnoreRaycastLayer => instance.ignoreRaycastLayer;
		[SerializeField] private int ignoreRaycastLayer;

		private void Awake()
        {
			instance = FindObjectOfType<References>();
        }
	}
}