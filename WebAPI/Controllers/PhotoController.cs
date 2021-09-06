using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharedLib;
using WebAPI.Database;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;

        public PhotoController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET /test
        [HttpGet("/test")]
        public async Task<ActionResult<string>> Test()
        {
            unitOfWork.Photo.Delete(await unitOfWork.Photo.GetByIdAsync(2));
            unitOfWork.Photo.Delete(await unitOfWork.Photo.GetByIdAsync(3));
            unitOfWork.Photo.Delete(await unitOfWork.Photo.GetByIdAsync(4));
            await unitOfWork.SaveChangesAsync();
            return "WebAPI is working !";
        }

        // GET api/photo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhotoItem>>> Get()
        {
            return await unitOfWork.Photo.GetAllAsync();
        }

        // GET api/photo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhotoItem>> Get(int id)
        {
            return await unitOfWork.Photo.GetByIdAsync(id);
        }

        // POST api/photo
        [HttpPost("add")]
        public async Task Post([FromBody] PhotoItem photo)
        {
            Debug.WriteLine("InserMany");
            unitOfWork.Photo.Insert(photo);
            await unitOfWork.SaveChangesAsync();
        }

        // POST api/photo
        [HttpPost("addMany")]
        public async Task Post([FromBody] List<PhotoItem> photos)
        {
            Debug.WriteLine("InserMany");
            unitOfWork.Photo.InsertMany(photos);
            await unitOfWork.SaveChangesAsync();
        }

        // GET api/photo/ban/5
        [HttpGet("ban/{id}")]
        public async Task<ActionResult<string>> Ban(long id)
        {
            unitOfWork.Photo.Ban(id);
            await unitOfWork.SaveChangesAsync();
            return $"Coucour {id}";
        }

        // DELETE api/photo/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
