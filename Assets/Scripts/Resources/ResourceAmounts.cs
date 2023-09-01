using System;
using System.Collections.Generic;
using System.Linq;

namespace LittleKingdom.Resources
{
    public class ResourceAmounts
    {
        private readonly Dictionary<ResourceType, int> resources;

        public int Total { get; private set; } = 0;

        public ResourceAmounts(params (ResourceType resourceType, int amount)[] values) =>
            Set(values.ToDictionary(k => k.resourceType, v => v.amount));

        public ResourceAmounts(Dictionary<ResourceType, int> resources) =>
            Set(resources);

        /// <summary>
        /// Sets the resources by splitting out combinations of <see cref="ResourceType"/>
        /// to ensure only individual enum values are used.
        /// </summary>
        public void Set(Dictionary<ResourceType, int> resources)
        {
            // Since ResourceType has [Flag], we need to ensure this only contains
            // the exact ResourceTypes (no combinations). Otherwise ReduceBy gets very messy.
            foreach (ResourceType exactResource in GetResourceTypes())
            {
                Set(exactResource, GetExactResourceAmountFromCombinations(resources, exactResource));
            }
        }

        public void Set(ResourceType resource, int amount)
        {
            Total = amount - resources[resource];
            resources[resource] = amount;
        }

        public static ResourceAmounts Add(ResourceAmounts resources1, ResourceAmounts resources2)
        {
            ResourceAmounts result = new();
            foreach (ResourceType resource in GetResourceTypes())
            {
                result.Set(resource, resources1.resources[resource] + resources2.resources[resource]);
            }
            return result;
        }

        public void Add(ResourceAmounts add)
        {
            foreach (ResourceType resource in GetResourceTypes())
            {
                Add(resource, add.resources[resource]);
            }
        }

        public void Add(ResourceType resource, int amount)
        {
            resources[resource] += amount;
            Total += amount;
        }

        public static ResourceAmounts Subtract(ResourceAmounts resources1, ResourceAmounts resources2)
        {
            ResourceAmounts result = new();
            foreach (ResourceType resource in GetResourceTypes())
            {
                result.Set(resource, resources1.resources[resource] - resources2.resources[resource]);
            }
            return result;
        }

        public void Subtract(ResourceAmounts subtract)
        {
            foreach (ResourceType resource in GetResourceTypes())
            {
                Subtract(resource, subtract.resources[resource]);
            }
        }

        public void Subtract(ResourceType resource, int amount)
        {
            resources[resource] -= amount;
            Total -= amount;
        }

        public static ResourceAmounts ClampMin(ResourceAmounts resources1, ResourceAmounts resources2)
        {
            ResourceAmounts result = new();
            foreach (ResourceType resource in GetResourceTypes())
            {
                result.Set(resource, Math.Min(resources1.resources[resource], resources2.resources[resource]));
            }
            return result;
        }

        public void  ClampMin(ResourceAmounts min)
        {
            foreach (ResourceType resource in GetResourceTypes())
            {
                Set(resource, Math.Min(resources[resource], min.resources[resource]));
            }
        }

        private static Array GetResourceTypes() =>
            Enum.GetValues(typeof(ResourceType));

        private static int GetExactResourceAmountFromCombinations(Dictionary<ResourceType, int> combinations, ResourceType exactResource)
        {
            int amount = 0;
            foreach (ResourceType combinationResource in combinations.Keys)
            {
                if (IsExactInTheCombination(exactResource, combinationResource))
                    amount += combinations[combinationResource];
            }
            return amount;
        }

        private static bool IsExactInTheCombination(ResourceType exactResource, ResourceType combinationResource) =>
            (exactResource & combinationResource) == exactResource;
    }
}