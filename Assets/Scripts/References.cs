using LittleKingdom.Tiles;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom
{
	public class References : MonoBehaviour
	{
		private static References instance;

		[SerializeField] private TilesInfo tilesInfo;

		public static TilesInfo TilesInfo => instance.tilesInfo;

		private void Awake()
        {
			instance = FindObjectOfType<References>();
        }
	}
}