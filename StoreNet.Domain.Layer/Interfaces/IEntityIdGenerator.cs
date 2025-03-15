namespace StoreNet.Domain.Layer.Interfaces;

// Interface pour générer un identifiant unique pour une entité
public interface IEntityIdGenerator
{
    string GenerateId();  // Méthode pour générer l'identifiant unique
}
