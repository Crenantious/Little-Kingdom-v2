namespace LittleKingdom
{
    public static class Utilities
    {
        public static string GetAssemblyQualifiedName(string @namespace, string typeName, string assemblyName = "Scripts") =>
            $"{@namespace}.{typeName}, {assemblyName}";
    }
}