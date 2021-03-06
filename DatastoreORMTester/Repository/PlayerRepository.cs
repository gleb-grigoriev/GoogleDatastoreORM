﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatastoreORM.Repository;
using DatastoreORMTester.DAO;
using Google.Cloud.Datastore.V1;

namespace DatastoreORMTester.Repository
{
    public class PlayerRepository: DatastoreRepository<PlayerDAO, long>, IPlayerRepository
    {
        public PlayerRepository(string projectId) : base(projectId)
        {
        }

        public async Task<PlayerDAO[]> GetNewPlayers(int limit, DateTime fromTime)
        {
            var registerTimeParam = GetPropertyName(p => p.RegisterTime);
            Query query = new Query(_kind)
            {
                Limit = limit,
                Filter = Filter.GreaterThan(registerTimeParam, fromTime),
                Order = { { registerTimeParam, PropertyOrder.Types.Direction.Descending } }
            };
            var res = await _db.RunQueryAsync(query);
            return res.Entities.Select(BuildDalEntity).ToArray();
        }
    }
}
