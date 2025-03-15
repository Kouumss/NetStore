using StoreNet.Domain.Layer.Entities;

namespace StoreNet.Application.Layer.Factories
{
    public interface IEntityFactory
    {
        T CreateEntity<T>() where T : BaseEntity, new();
    }
}

