using UnityEngine;

namespace LittleKingdom.Input
{
    public class RaycastFromPointer
    {
        private readonly StandardInput input;
        private readonly IReferences references;

        public RaycastFromPointer(StandardInput input, IReferences references)
        {
            this.input = input;
            this.references = references;
        }

        public bool Cast(out RaycastHit hit, float maxDistance = 100)
        {
            Ray ray = references.ActiveCamera.ScreenPointToRay(input.GetPointerPosition());
            return Physics.Raycast(ray, out hit, maxDistance);
        }
    }
}