using System;

namespace Autonoma.Configuration.Exceptions
{
    public class EntityNotFoundException<TEntity> : Exception 
    {
        public TEntity Entity { get; set; }

        public EntityNotFoundException(TEntity entity)
        {
            Entity = entity;
        }

        public EntityNotFoundException(int id) : base($"Не найден экземпляр сущности {nameof(Entity)} c идентификатором {id}")
        {

        }

        public EntityNotFoundException(string externalId) : base($"Не найден экземпляр сущности {nameof(Entity)} c внешним идентификатором {externalId}")
        {

        }
    }
}
