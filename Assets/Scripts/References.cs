using LittleKingdom.Board;
using UnityEngine;

namespace LittleKingdom
{
	//This class is loaded before the default load time, so references are ensured to be set up before being called.
	public class References : MonoBehaviour
	{
		private static References instance;

		[SerializeField] private TilesInfo tilesInfo;

		/// <summary>
		/// Gets the resource tile information for creating the game board.
		/// </summary>
		public static TilesInfo TilesInfo => instance.tilesInfo;

		private void Awake()
        {
			instance = FindObjectOfType<References>();
        }
	}
}