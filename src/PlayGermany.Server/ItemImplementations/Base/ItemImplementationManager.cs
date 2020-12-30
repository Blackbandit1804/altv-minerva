using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlayGermany.Server.DataAccessLayer.Context;
using PlayGermany.Server.DataAccessLayer.Enums;
using PlayGermany.Server.DataAccessLayer.Models;
using PlayGermany.Server.ItemImplementations.Base;

namespace PlayGermany.Server.Managers
{
    public class ItemImplementationManager
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;
        private readonly List<(ItemImplementationType, ItemImplementation)> _itemImplementations;

        public ItemImplementationManager(
            ILogger<ItemImplementationManager> logger,
            IEnumerable<ItemImplementation> itemImplementations,
            IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            _itemImplementations = new List<(ItemImplementationType, ItemImplementation)>();
            _dbContextFactory = dbContextFactory;

            foreach (var itemImplementation in itemImplementations)
            {
                if (_itemImplementations.Any(e => e.Item1 == itemImplementation.Type))
                {
                    logger.LogError($"ItemImplementation {itemImplementation.GetType().Name} with Type {Enum.GetName(itemImplementation.Type)} is already defined!");
                    continue;
                }

                _itemImplementations.Add((itemImplementation.Type, itemImplementation));
            }
        }

        public async Task<ItemImplementation> GetImplementation(Item item)
        {
            var itemDefinition = item.ItemDefinition;
            
            if (itemDefinition == null)
            {
                using var dbContext = _dbContextFactory.CreateDbContext();
                itemDefinition = await dbContext.ItemDefinitions.FirstOrDefaultAsync(e => e.Id == item.ItemDefinitionId);
            }

            return GetImplementation(itemDefinition);
        }

        public ItemImplementation GetImplementation(ItemDefinition itemDefinition)
        {
            var element = _itemImplementations.FirstOrDefault(e => e.Item1 == (ItemImplementationType) itemDefinition.ItemImplementationType);
            
            if (element == default(ValueTuple<ItemImplementationType, ItemImplementation>))
            {
                throw new NotSupportedException($"ItemImplementation {Enum.GetName(element.Item1)} not registered");
            }
            
            return element.Item2;
        }
    }
}