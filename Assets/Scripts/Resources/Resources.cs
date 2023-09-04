using System;
using System.Collections.Generic;
using System.Linq;

namespace LittleKingdom.Resources
{
    public class Resources
    {
        private const string NegativeResourceError = "A resource amount cannot be negative.";

        private readonly Dictionary<ResourceType, int> resources = new();

        /// <summary>
        /// Cannot be negative.
        /// </summary>
        public int Total { get; private set; } = 0;

        public Resources(params (ResourceType resourceType, int amount)[] resources) =>
            SetAll(resources);

        /// <summary>
        /// Throws if the resource does not exist. Should only be when attempting to retrieve <see cref="ResourceType.None"/>.
        /// </summary>
        /// <exception cref="KeyNotFoundException"/>
        public int Get(ResourceType resourceType) => resources[resourceType];

        // Sets the resources by splitting out combinations of "ResourceType".
        // to ensure only individual enum values are used.
        /// <summary>
        /// If the <see cref="ResourceType"/> is not specified, it will be set to 0.
        /// Throws if a resource amount is negative.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public void SetAll(params (ResourceType resourceType, int amount)[] resources)
        {
            // Since ResourceType has FlagsAttribute, we need to ensure this only contains
            // the exact ResourceTypes (no combinations). Otherwise, other methods can get very messy.
            foreach (ResourceType exactResource in GetResourceTypes())
            {
                Set(exactResource, GetExactResourceAmountFromCombinations(exactResource, resources));
            }
        }

        /// <summary>
        /// <paramref name="amount"/> cannot be negative.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Set(ResourceType resource, int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), NegativeResourceError);

            if (resources.ContainsKey(resource))
                Total -= resources[resource];
            Total += amount;
            resources[resource] = amount;
        }

        public static Resources Add(Resources resources1, Resources resources2)
        {
            Resources result = new();
            foreach (ResourceType resource in GetResourceTypes())
            {
                result.Set(resource, resources1.resources[resource] + resources2.resources[resource]);
            }
            return result;
        }

        public void Add(Resources add)
        {
            foreach (ResourceType resource in GetResourceTypes())
            {
                Add(resource, add.resources[resource]);
            }
        }

        /// <summary>
        /// Throws if the result is negative.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Add(ResourceType resource, int amount)
        {
            foreach (ResourceType individualResoource in GetResourceTypes())
            {
                if (IsIndividualResourceInTheCombination(individualResoource, resource) is false)
                    continue;

                if (resources[individualResoource] + amount < 0)
                    throw new ArgumentOutOfRangeException(nameof(amount), NegativeResourceError);

                resources[individualResoource] += amount;
                Total += amount;
            }
        }

        public static Resources Subtract(Resources resources1, Resources resources2)
        {
            Resources result = new();
            foreach (ResourceType resource in GetResourceTypes())
            {
                result.Set(resource, resources1.resources[resource] - resources2.resources[resource]);
            }
            return result;
        }

        public void Subtract(Resources subtract)
        {
            foreach (ResourceType resource in GetResourceTypes())
            {
                Subtract(resource, subtract.resources[resource]);
            }
        }

        /// <summary>
        /// Throws if the result is negative.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Subtract(ResourceType resource, int amount)
        {
            foreach (ResourceType individualResoource in GetResourceTypes())
            {
                if (IsIndividualResourceInTheCombination(individualResoource, resource) is false)
                    continue;

                if (resources[individualResoource] - amount < 0)
                    throw new ArgumentOutOfRangeException(nameof(amount), NegativeResourceError);

                resources[individualResoource] -= amount;
                Total -= amount;
            }
        }

        /// <returns>
        /// <see cref="Resources"/> where each <see cref="ResourceType"/> is the minimum value between
        /// those in <paramref name="resources1"/> and <paramref name="resources2"/>.
        /// </returns>
        public static Resources ClampMin(Resources resources1, Resources resources2)
        {
            Resources result = new();
            foreach (ResourceType resource in GetResourceTypes())
            {
                result.Set(resource, Math.Min(resources1.resources[resource], resources2.resources[resource]));
            }
            return result;
        }

        /// <returns>
        /// <see cref="Resources"/> where each <see cref="ResourceType"/> is the minimum value between
        /// those in <see langword="this"/> and <paramref name="resources2"/>.
        /// </returns>
        public void ClampMin(Resources min)
        {
            foreach (ResourceType resource in GetResourceTypes())
            {
                Set(resource, Math.Min(resources[resource], min.resources[resource]));
            }
        }

        private static IEnumerable<ResourceType> GetResourceTypes() =>
            ((ResourceType[])Enum.GetValues(typeof(ResourceType))).Where(r => r != ResourceType.None);

        /// <summary>
        /// Since <see cref="ResourceType"/> has the <see cref="FlagsAttribute"/>, this will look through
        /// <paramref name="resources"/> and sum all those that contain <paramref name="exact"/>.
        /// </summary>
        /// <param name="exact">This should be a single <see cref="ResourceType"/>.</param>
        private static int GetExactResourceAmountFromCombinations(
            ResourceType exact, params (ResourceType, int)[] resources)
        {
            int total = 0;
            foreach ((ResourceType combination, int amount) in resources)
            {
                if (IsIndividualResourceInTheCombination(exact, combination))
                    total += amount;
            }
            return total;
        }

        private static bool IsIndividualResourceInTheCombination(ResourceType exactResource, ResourceType combinationResource) =>
            (exactResource & combinationResource) == exactResource;
    }
}