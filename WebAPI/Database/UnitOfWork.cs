using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Database.Repositories;

namespace WebAPI.Database
{
    public class UnitOfWork : IDisposable
    {
        public PhotoRepository Photo => photoRepository ?? (photoRepository = new PhotoRepository(context));
        private PhotoRepository photoRepository;

        private readonly Context context;

        public UnitOfWork(Context context)
        {
            this.context = context;
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context?.Dispose();
        }
    }
}
