using LittleKingdom;
using System;

namespace Assets.Scripts.Exceptions
{
    public class AssetFoundException : Exception
    {
        private const string NoAssetsFoundError = "Unable to load asset of type {0} with name {1}.";

        public AssetFoundException(Type type, string name) :
            base(NoAssetsFoundError.FormatConst(nameof(type), name))
        {

        }
    }
}