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
            public string CollectionName { get; set; }
        }
        
        //[Route("[action]")]
        [Authorize] //works
        [HttpPost("AddSnippetToCollection")]
        public async Task<IActionResult> AddSnippetToCollection(SnippetAndCollection args)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            var collection = db.Collections.Where(c => c.CollectionName == args.CollectionName && c.UserID==user.Id).FirstOrDefault();
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
        //[Route("[action]")]
        [Authorize] //works
        [HttpDelete("RemoveSnippetFromCollection")]
        public async Task<IActionResult> RemoveSnippetFromCollection(SnippetAndCollection args)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            var collection = db.Collections.Where(c => c.CollectionName == args.CollectionName && c.UserID==user.Id).FirstOrDefault();
            var snippet = db.Snippets.Where(s => s.SnippetID == args.SnippetID && s.UserID==user.Id && s.CollectionID==collection.CollectionID).FirstOrDefault();
            if (collection != null && snippet != null)
            {
                //snippet.CollectionID = CollectionID;
                collection.Snippets.Remove(snippet);
                db.SaveChanges();
                return Ok();
            }
            if (collection == null || snippet == null)
            {
                return NotFound();
            }
            return Unauthorized();

        }
        //[Route("[action]")]
        [Authorize] //works
        [HttpGet("GetSnippetsFromCollection/{CollectionID}")]
        public async Task<IEnumerable<ISnippetModel>> GetSnippetsFromCollection(string CollectionID)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            var collection = db.Collections.Where(c => c.CollectionID == CollectionID && c.UserID==user.Id).FirstOrDefault();
            if (collection != null)
            {
                return collection.Snippets.Select(s=> new ISnippetModel()
                {
                    SnippetID=s.SnippetID,
                    CollectionID = s.CollectionID,
                    Link = s.Link,
                    ContentType = s.ContentType,
                    ContentSubType = s.ContentSubType,
                    Content = s.Content,
                    Description = s.Description
                    
                });
            }

            return new List<ISnippetModel>();

        }
        
        [Authorize] //works
        [HttpGet("GetSnippetsWithoutCollection")]
        public async Task<IEnumerable<ISnippetModel>> GetSnippetsWithoutCollection()
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            var snippets = db.Snippets.Where(c => c.CollectionID == null  && c.UserID==user.Id);
            if (snippets != null)
            {
                return snippets.Select(s=> new ISnippetModel()
                {
                    SnippetID=s.SnippetID,
                    CollectionID = s.CollectionID,
                    Link = s.Link,
                    ContentType = s.ContentType,
                    ContentSubType = s.ContentSubType,
                    Content = s.Content,
                    Description = s.Description
                    
                });
            }

            return new List<ISnippetModel>();

        }
        
        
        
        //CRUD starting ---->
        [HttpGet] //works
        [Authorize]
        public IEnumerable<ICollectionModel> GetCollections()
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            return db.Collections.Where(s => s.UserID==user.Id).Select(s => new ICollectionModel()
            {
                CollectionID = s.CollectionID,
                CollectionName = s.CollectionName
            });
        }
        
        [Authorize] //Works
        [HttpGet("{id}")]
        public ICollectionModel? GetCollection(string id)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            return db.Collections.Where(t => t.CollectionID == id && t.UserID==user.Id).Select(s => new ICollectionModel()
            {
                CollectionID = s.CollectionID,
                CollectionName = s.CollectionName
            }).FirstOrDefault();
        }

        //[Route("[action]")]
        [Authorize]
        [HttpPost] //works
        public async void AddCollection([FromBody] ICollectionModel s)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            Collection obj = new Collection();
            obj.UserID = user.Id;
            obj.CollectionName = s.CollectionName;
            db.Collections.Add(obj);
            db.SaveChanges();
        }
        
        [Authorize]
        [HttpPut] //works
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
        
        [Authorize] //works
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