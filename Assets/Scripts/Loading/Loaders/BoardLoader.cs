using LittleKingdom.Board;
using LittleKingdom.Loading;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom
{
    [RequireComponent(typeof(BoardMono))]
    public class BoardLoader : Loader<BoardLC>
    {
        private BoardMono board;
        [SerializeField] private GameSetupLoader gameSetupLoader;

        private void Awake()
        {
            Dependencies.Add(gameSetupLoader);
            board = GetComponent<BoardMono>();
        }

        public override void Load(BoardLC config) =>
            board.Create();

        public override void Unload()
        {
            // TODO
            throw new System.NotImplementedException();
        }
    }
}