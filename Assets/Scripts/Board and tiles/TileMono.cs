using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom
{
	public class TileMono : MonoBehaviour
	{
		public Tile Tile { get; private set; }

		/// <summary>
		/// Initialise the class. Only to be called from a factory.
		/// </summary>
		/// <param name="tile">The logic class.</param>
		public void Initialise(Tile tile)
        {
			Tile = tile;
        }
	}
}