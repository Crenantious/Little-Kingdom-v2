using UnityEngine;

namespace LittleKingdom.Input
{
    public class RaycastFromPointer
    {
        private readonly IStandardInput input;
        private readonly IReferences references;

        public RaycastFromPointer(IStandardInput input, IReferences references)
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