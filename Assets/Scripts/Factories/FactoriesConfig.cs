using UnityEngine;

namespace LittleKingdom
{
    [CreateAssetMenu(fileName = "Factories config", menuName = "Game/Factories config")]
	public class FactoriesConfig : ScriptableObject
	{
		private static GameObject tilePrefab;
		public static GameObject TilePrefab => tilePrefab;
	}
}