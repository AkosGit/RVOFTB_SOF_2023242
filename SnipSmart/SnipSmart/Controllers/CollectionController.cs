using SnipSmart.Data;
using SnipSmart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace SnipSmart.Controllers;

[ApiController]
[Route("[controller]")]
public class CollectionController : ControllerBase
{
        ApplicationDbContext db;
        UserManager<User> _userManager;

        public CollectionController(ApplicationDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            _userManager = userManager;
        }
        
        //CRUD starting ---->
        [HttpGet]
        public IEnumerable<ICollectionModel> GetCollections()
        {
            return db.Collections;
        }

        [HttpGet("{id}")]
        [Authorize]
        public ICollectionModel? GetCollections(string id)
        {
            return db.Collections.FirstOrDefault(t => t.CollectionID == id);
        }

        [Route("[action]")]
        [Authorize]
        [HttpPost]
        public async void AddCollection([FromBody] ICollectionModel s)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            db.Collections.Add(s as Collection);
            s.UserID = user.Id;
            s.CollectionID = Guid.NewGuid().ToString();
            db.SaveChanges();
        }
    
        [HttpPut]
        public async Task<IActionResult> EditCollection([FromBody] ICollectionModel s)
        {
            var userName = this.User.Identity.Name;
            var old = db.Collections.FirstOrDefault(t => t.CollectionID == s.CollectionID);
            if (old.User.UserName == userName)
            {
                old.CollectionName = s.CollectionName;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                throw new ArgumentException("Not your Collection!");
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollection(string id)
        {
            var userName = this.User.Identity.Name;
            var collectionToDelete = db.Collections.FirstOrDefault(t => t.CollectionID == id);
            if (collectionToDelete.User.UserName == userName)
            {
                db.Collections.Remove(collectionToDelete);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                throw new ArgumentException("Not your snippet!");
            }
            
        }
        //----> CRUD ending 
    }