using LittleKingdom.Board;
using LittleKingdom.Input;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace LittleKingdom
{
    public class TownPlacement : MonoBehaviour
    {
        private const float ManualUpdatDelay = 0.5f;

        private InGameInput inGameInput;

        [Inject]
        private void Construct(InGameInput inGameInput) =>
            this.inGameInput = inGameInput;

        public void PlaceManually(Town town)
        {
            StartCoroutine(PlaceManuallyCoroutine(town));
        }

        private IEnumerator PlaceManuallyCoroutine(Town town)
        {
            // TODO: JR - make an infinite loop until the users clicks.
            Tile originTile = GetTownOriginTile();
            MoveTownToTile(town, originTile);
            yield return new WaitForSeconds(ManualUpdatDelay);
        }

        public void PlaceAutomatically(Town town)
        {
            throw new NotImplementedException();
        }

        private Tile GetTownOriginTile()
        {
            throw new NotImplementedException();
        }

        private void MoveTownToTile(Town town, Tile origin)
        {
            throw new NotImplementedException();
        }
    }
}