﻿namespace App.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using App.Data.Models;
    using App.Data.Repositories;
    using App.Services.Data.Contracts;

    public class EntityService : IEntityService
    {
        private readonly IDbRepository<Entity> entities;

        public EntityService(IDbRepository<Entity> entities)
        {
            this.entities = entities;
        }

        public IQueryable<Entity> GetAllEntities()
        {
            return this.entities.GetAll();
        }

        public IQueryable<Entity> GetEntityById(int id)
        {
            return this.entities.GetAll()
                .Where(e => e.Id == id);
        }

        public async Task<Entity> AddEntity(Entity entity)
        {
            this.entities.Add(entity);

            await this.entities.SaveChangesAsync();

            return entity;
        }

        public async Task<Entity> EditEntity(Entity entity)
        {
            var entityToEdit = this.GetEntityById(entity.Id).FirstOrDefault();

            if (entityToEdit == null)
            {
                return null;
            }

            entityToEdit.Title = entity.Title;
            entityToEdit.Content = entity.Content;

            await this.entities.SaveChangesAsync();

            return entityToEdit;
        }

        public async Task DeleteEntity(int id)
        {
            this.entities.Delete(id);

            await this.entities.SaveChangesAsync();
        }
    }
}
