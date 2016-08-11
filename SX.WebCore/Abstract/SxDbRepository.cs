﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace SX.WebCore.Abstract
{
    public abstract class SxDbRepository<TKey, TModel, TDbContext>
        where TModel : SxDbModel<TKey>
        where TDbContext : SxDbContext
    {
        private static string _connectionString;
        static SxDbRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DbContext"].ConnectionString;
        }
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        public virtual TModel Create(TModel model)
        {
            if (model is SxDbModel<TKey>)
                prepareUpdatedModel(model as SxDbModel<TKey>);

            var dbContext = Activator.CreateInstance<TDbContext>();
            dbContext.Configuration.AutoDetectChangesEnabled = false;
            dbContext.Entry(model).State = EntityState.Added;
            dbContext.Configuration.AutoDetectChangesEnabled = true;
            dbContext.SaveChanges();

            return model;
        }
        private static void prepareUpdatedModel(SxDbModel<TKey> model)
        {
            var date = DateTime.Now;
            if (model.DateCreate == DateTime.MinValue)
                model.DateCreate = date;
            if (model is SxDbUpdatedModel<TKey>)
            {
                var m = model as SxDbUpdatedModel<TKey>;
                if (m.DateUpdate == DateTime.MinValue)
                    m.DateUpdate = date;
                if (model is SxMaterial)
                    (model as SxMaterial).DateOfPublication = model.DateCreate;
            }

        }

        public virtual TModel Update(TModel model, bool changeDateUpdate = true, params string[] propertiesForChange)
        {
            var dbContext = Activator.CreateInstance<TDbContext>();

            var modelType = typeof(TModel);
            var keys = getEntityKeys(dbContext, modelType, model);
            var oldModel = dbContext.Set<TModel>().Find(keys);
            if (oldModel == null) return null;
            var oldModelType = oldModel.GetType();
            var propsForChange = modelType.GetProperties()
                .Where(x => propertiesForChange.Contains(x.Name))
                .Select(x => new { Name = x.Name, Value = x.GetValue(model) }).ToArray();
            foreach (var prop in propsForChange)
            {
                var oldProp = oldModelType.GetProperty(prop.Name);
                oldProp.SetValue(oldModel, prop.Value);
            }

            if (changeDateUpdate && oldModel is SxDbUpdatedModel<TKey>)
                (oldModel as SxDbUpdatedModel<TKey>).DateUpdate = DateTime.Now;

            dbContext.Configuration.AutoDetectChangesEnabled = false;
            dbContext.Entry(oldModel).State = EntityState.Modified;
            dbContext.Configuration.AutoDetectChangesEnabled = true;
            dbContext.SaveChanges();

            return oldModel;
        }

        public virtual TModel Update(TModel model, object[] additionalData, bool changeDateUpdate = true, params string[] propertiesForChange)
        {
            return Update(model, changeDateUpdate, propertiesForChange);
        }

        public virtual TModel Update(TModel model)
        {
            throw new NotImplementedException("Не поддерживается в данном контексте");
        }

        public virtual void Delete(TModel model)
        {
            var dbContext = Activator.CreateInstance<TDbContext>();
            dbContext.Set<TModel>().Remove(model);
            dbContext.SaveChanges();
        }
        public virtual async Task DeleteAsync(TModel model)
        {
            await Task.Run(()=> {
                Delete(model);
            });
        }

        public virtual IQueryable<TModel> All
        {
            get
            {
                var dbContext = Activator.CreateInstance<TDbContext>();
                return dbContext.Set<TModel>().AsNoTracking();
            }
        }

        public virtual TModel[] Read(SxFilter filter)
        {
            return new TModel[0];
        }
        public virtual async Task<TModel[]> ReadAsync(SxFilter filter)
        {
            return await Task.Run(()=> {
                return Read(filter);
            });
        }

        public virtual TModel GetByKey(params object[] id)
        {
            var dbContext = Activator.CreateInstance<TDbContext>();
            var dbSet = dbContext.Set<TModel>();
            return dbSet.Find(id);
        }
        public virtual async Task<TModel> GetByKeyAsync(params object[] id)
        {
            return await Task.Run(() =>
            {
                return GetByKey(id);
            });
        }

        private static object[] getEntityKeys(DbContext dbContext, Type modelType, TModel model)
        {
            var objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
            var keyNames = ((IEnumerable<System.Data.Entity.Core.Metadata.Edm.EdmMember>)objectContext.MetadataWorkspace
                .GetType(modelType.Name, modelType.Namespace, System.Data.Entity.Core.Metadata.Edm.DataSpace.OSpace)
                .MetadataProperties
                .Where(x => x.Name == "KeyMembers")
                .First()
                .Value).Select(x => x.Name).ToArray();
            object[] keys = new object[keyNames.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = modelType.GetProperty(keyNames[i].ToString()).GetValue(model);
            }
            return keys;
        }
    }
}
