using System.Collections.Generic;

namespace LittleKingdom.Resources
{
    public interface IGetResourceHolders
    {
        public IEnumerable<IHoldResources> Get();
    }
}