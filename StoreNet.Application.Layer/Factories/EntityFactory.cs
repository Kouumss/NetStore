using StoreNet.Domain.Layer.Entities;
using StoreNet.Domain.Layer.Interfaces;

namespace StoreNet.Application.Layer.Factories
{
    public class EntityFactory : IEntityFactory
    {
        private readonly IEntityIdGenerator _idGenerator;

        // Constructeur pour injecter le générateur d'ID
        public EntityFactory(IEntityIdGenerator idGenerator)
        {
            _idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator), "The ID Generator service cannot be null.");
        }

        // Méthode pour créer une entité et lui attribuer un ID généré automatiquement
        public T CreateEntity<T>() where T : BaseEntity, new()
        {
            // Créer une nouvelle instance de l'entité
            var entity = new T();

            // Générer un ID pour l'entité
            entity.Id = _idGenerator.GenerateId();
            entity.CreatedAt = DateTime.UtcNow;

            return entity;
        }
    }
}
