using LittleKingdom.Tiles;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom
{
	public class PrefabReferences : MonoBehaviour
	{
		private static PrefabReferences instance;

		[SerializeField] private GameObject tile;

		public static GameObject Tile => instance.tile;

		private void Awake()
		{
			instance = FindObjectOfType<PrefabReferences>();
		}
	}
}