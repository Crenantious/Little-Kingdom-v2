using UnityEngine;

namespace LittleKingdom
{
    //This class is loaded before the default load time, so references are ensured to be set up before being called.
    // TODO: JR - dissolve into a Zenject installer.
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