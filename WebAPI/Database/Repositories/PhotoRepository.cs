using SharedLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Database.Repositories
{
    public class PhotoRepository : Repository<PhotoItem>
    {
        public PhotoRepository(Context context) : base(context)
        {
        }

        public void Insert(PhotoItem photo)
        {
            if (Table.Any(p => p.IdFromPlat == photo.IdFromPlat))
                return;

            Table.Add(photo);
        }

        public void InsertMany(List<PhotoItem> photos)
        {
            photos.ForEach(p => Insert(p));
        }

        public async void Ban(long id)
        {
            var photo = await GetByIdAsync(id);
            if (photo == null) throw new Exception($"The photo [{id}] was not found in the database.");
            photo.Removed = true;
            Table.Update(photo);
        }
    }
}
