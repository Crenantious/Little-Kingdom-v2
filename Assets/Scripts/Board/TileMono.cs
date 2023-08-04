using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Board
{
	public class TileMono : MonoBehaviour
	{
        /// <summary>
        /// The logic class for this <see cref="MonoBehaviour"/>.
        /// </summary>
        public Tile Tile { get; private set; }

		/// <summary>
		/// The index of the column on the board that this tile is located.
		/// </summary>
		public int Column { get; set; }

		/// <summary>
		/// The index of the row on the board that this tile is located.
		/// </summary>
		public int Row { get; set; }

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