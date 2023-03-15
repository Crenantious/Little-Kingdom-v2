using System.Collections.Generic;

namespace LittleKingdom.Loading
{
    public interface ILoader
    {
        public List<ILoader> Dependencies { get; }
        public void Load();
    }
}