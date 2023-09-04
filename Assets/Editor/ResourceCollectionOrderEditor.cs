using LittleKingdom.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace LittleKingdom
{
    //TODO : JR - maybe add grouping to the halter types (maybe via an attribute with a string group name parameter)
    // So they can be moved around the order together. Seems like way too much work though.
    [CustomEditor(typeof(ResourceCollectionOrder))]
    public class ResourceCollectionOrderEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            serializedObject.Update();

            UpdateHandlerOrder(typeof(IProduceResources), serializedObject.FindProperty("producers"));
            UpdateHandlerOrder(typeof(IHaltResources), serializedObject.FindProperty("halters"));
            UpdateHandlerOrder(typeof(IMoveResources), serializedObject.FindProperty("movers"));

            serializedObject.ApplyModifiedProperties();
        }

        private void UpdateHandlerOrder(Type handlerType, SerializedProperty handlers)
        {
            HashSet<string> handlerTypes = GetHandlerTypesNames(handlerType);
            RemoveNonExistantHandlerTypes(handlerTypes, handlers);
            AddHandlerTypes(GetMissingHandlerTypes(handlerTypes, handlers), handlers);
        }

        private static HashSet<string> GetHandlerTypesNames(Type handlerType) =>
            Assembly.GetAssembly(handlerType)
                .GetTypes()
                .Where(t => handlerType.IsAssignableFrom(t) &&
                            t != handlerType &&
                            !t.IsAbstract &&
                            !t.IsInterface)
                .Select(t => t.Name)
                .ToHashSet();

        private static void RemoveNonExistantHandlerTypes(HashSet<string> handlerTypes, SerializedProperty handlers)
        {
            for (int i = 0; i < handlers.arraySize; i++)
            {
                string typeName = handlers.GetArrayElementAtIndex(i).stringValue;
                if (handlerTypes.Contains(typeName) is false)
                {
                    handlers.DeleteArrayElementAtIndex(i);
                    i--;
                }
            }
        }

        private static HashSet<string> GetMissingHandlerTypes(HashSet<string> handlerTypes, SerializedProperty handlers)
        {
            for (int i = 0; i < handlers.arraySize; i++)
            {
                string typeName = handlers.GetArrayElementAtIndex(i).stringValue;
                if (handlerTypes.Contains(typeName))
                    handlerTypes.Remove(typeName);
            }
            return handlerTypes;
        }

        private static void AddHandlerTypes(HashSet<string> handlerTypes, SerializedProperty handlers)
        {
            foreach (string halter in handlerTypes)
            {
                handlers.arraySize++;
                handlers.GetArrayElementAtIndex(handlers.arraySize - 1).stringValue = halter;
            }
        }
    }
}