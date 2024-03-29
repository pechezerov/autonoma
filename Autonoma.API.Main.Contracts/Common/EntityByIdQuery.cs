﻿using Autonoma.API.Queries;

namespace Autonoma.API.Main.Contracts.Common
{
    public class EntityByIdQuery : Query
    {
        public EntityByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
