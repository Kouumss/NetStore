using Microsoft.EntityFrameworkCore;
using StoreNet.Domain.Layer.Entities;
using StoreNet.Domain.Layer.Interfaces;
using StoreNet.Infrastructure.Layer.Data;

namespace StoreNet.Infrastructure.Layer.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Vérifie si un client existe selon un prédicat spécifique
        public async Task<bool> AnyAsync(Func<Customer, bool> predicate)
        {
            // Appliquer directement un `Any` sur la base de données, pas besoin d'AsNoTracking ici.
            return await _context.Customers.AnyAsync(c => predicate(c));
        }

        // Ajoute un nouveau client à la base de données
        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        // Récupère un client par son email (insensible à la casse)
        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _context.Customers
                .AsNoTracking() // Pas de suivi des entités pour la lecture
                .FirstOrDefaultAsync(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        // Récupère un client actif par son ID, en incluant seulement les informations nécessaires
        public async Task<Customer> GetByIdAsync(string id)
        {
            var customer = await _context.Customers
                .AsNoTracking()
                .Include(c => c.Addresses)  // Inclut les adresses associées
                .Include(c => c.Orders)     // Inclut les commandes associées
                    .ThenInclude(o => o.OrderItems)  // Inclut les éléments de commande
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);  // Assure-toi que c'est un client actif

            if (customer is null)
            {
                throw new KeyNotFoundException($"No active customer found with ID {id}.");
            }

            return customer;
        }

        // Récupère tous les clients avec leurs adresses et commandes associées
        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers
                .AsNoTracking()
                .Include(c => c.Addresses)
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderItems)
                .ToListAsync();  // Retourne tous les clients et leurs relations
        }

        // Récupère un client avec ses adresses uniquement (optimisation si tu n'as besoin que des adresses)
        public async Task<Customer?> GetCustomerWithAddressesAsync(string customerId)
        {
            return await _context.Customers
                .AsNoTracking()
                .Include(c => c.Addresses)  // Inclut uniquement les adresses
                .FirstOrDefaultAsync(c => c.Id == customerId && c.IsActive);  // Recherche du client actif
        }

        // Met à jour un client existant dans la base de données
        public async Task UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);  // Marque le client comme mis à jour
            await _context.SaveChangesAsync();     // Sauvegarde les modifications
        }

        // Supprime un client en le marquant comme inactif (soft delete)
        public async Task DeleteAsync(Customer customer)
        {
            customer.IsActive = false;  // Marque le client comme inactif
            _context.Customers.Update(customer);  // Marque l'entité comme mise à jour
            await _context.SaveChangesAsync();     // Sauvegarde les modifications
        }

        // Récupère un client avec ses adresses et commandes associées
        public async Task<Customer?> GetCustomerWithAddressesAndOrdersAsync(string customerId)
        {
            return await _context.Customers
                .AsNoTracking() // Pas de suivi pour la lecture
                .Include(c => c.Addresses)  // Inclut les adresses
                .Include(c => c.Orders)     // Inclut les commandes
                    .ThenInclude(o => o.OrderItems)  // Inclut les éléments de commande
                .FirstOrDefaultAsync(c => c.Id == customerId && c.IsActive);  // Recherche du client actif
        }
    }
}
