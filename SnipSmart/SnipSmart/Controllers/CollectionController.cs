using SnipSmart.Data;
using SnipSmart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

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

        public class SnippetAndCollection
        {
            public string SnippetID { get; set; }
            public string CollectionID { get; set; }
        }
        
        [Route("[action]")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddSnippetToCollection(SnippetAndCollection args)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            var collection = db.Collections.Where(c => c.CollectionID == args.CollectionID && c.UserID==user.Id).FirstOrDefault();
            var snippet = db.Snippets.Where(s => s.SnippetID == args.SnippetID && s.UserID==user.Id).FirstOrDefault();
            if (collection != null && snippet != null)
            {
                //snippet.CollectionID = args.CollectionID;
                collection.Snippets.Add(snippet);
                db.SaveChanges();
                return Ok();
            }
            return Unauthorized();

        }
        [Route("[action]")]
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> RemoveSnippetFromCollection(SnippetAndCollection args)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            var collection = db.Collections.Where(c => c.CollectionID == args.CollectionID && c.UserID==user.Id).FirstOrDefault();
            var snippet = db.Snippets.Where(s => s.SnippetID == args.SnippetID && s.UserID==user.Id).FirstOrDefault();
            if (collection != null && snippet != null)
            {
                //snippet.CollectionID = CollectionID;
                collection.Snippets.Remove(snippet);
                db.SaveChanges();
                return Ok();
            }
            return Unauthorized();

        }
        [Route("[action]")]
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<ISnippetModel>> GetSnippetsFromCollection([FromBody] string CollectionID)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            var collection = db.Collections.Where(c => c.CollectionID == CollectionID && c.UserID==user.Id).FirstOrDefault();
            if (collection != null)
            {
                return collection.Snippets.Select(s=> s as ISnippetModel);
            }

            return new List<ISnippetModel>();

        }
        
        
        
        //CRUD starting ---->
        [HttpGet]
        [Authorize]
        public IEnumerable<ICollectionModel> GetCollections()
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            return db.Collections.Where(s => s.UserID==user.Id).Select(s => s as ICollectionModel);
        }
        
        [Authorize]
        [HttpGet("{id}")]
        public ICollectionModel? GetCollections(string id)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            return db.Collections.FirstOrDefault(t => t.CollectionID == id && t.UserID==user.Id);
        }

        //[Route("[action]")]
        [Authorize]
        [HttpPost]
        public async void AddCollection([FromBody] ICollectionModel s)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            s.UserID = user.Id;
            s.CollectionID = Guid.NewGuid().ToString();
            db.Collections.Add(s as Collection);
            db.SaveChanges();
        }
        
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> EditCollection([FromBody] ICollectionModel s)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            var old = db.Collections.FirstOrDefault(t => t.CollectionID == s.CollectionID && t.UserID==user.Id);
            if (old != null)
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
        
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollection(string id)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            var collectionToDelete = db.Collections.FirstOrDefault(t => t.CollectionID == id && t.UserID==user.Id);
            if (collectionToDelete != null)
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