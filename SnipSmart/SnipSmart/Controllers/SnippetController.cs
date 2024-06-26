using SnipSmart.Data;
using SnipSmart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
namespace SnipSmart.Controllers;


[ApiController]
[Route("[controller]")]
public class SnippetController : ControllerBase
{
        ApplicationDbContext db;
        UserManager<User> _userManager;

        public SnippetController(ApplicationDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            _userManager = userManager;
        }
        //CRUD starting ---->
        [HttpGet]
        [Authorize]
        public IEnumerable<ISnippetModel> GetSnippets()
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            return db.Snippets.Where(s => s.UserID==user.Id).Select(s => 
                new ISnippetModel(){CollectionID = s.CollectionID, SnippetID = s.SnippetID, Content = s.Content, ContentType = s.ContentType, ContentSubType = s.ContentSubType, Link = s.Link, Description = s.Description}
            );
        }

        [HttpGet("{id}")]
        [Authorize]
        public ISnippetModel? GetSnippet(string id)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            return db.Snippets.Where(t  => t.SnippetID == id && t.UserID==user.Id).Select(s => 
                new ISnippetModel(){CollectionID = s.CollectionID, SnippetID = s.SnippetID, Content = s.Content, ContentType = s.ContentType, ContentSubType = s.ContentSubType, Link = s.Link, Description = s.Description}
            ).FirstOrDefault();
        }

        //[Route("[action]")]
        [Authorize]
        [HttpPost]
        public async Task<string> AddSnippet([FromBody] ISnippetModel s)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            Snippet obj = new Snippet();
            obj.UserID = user.Id;
            obj.ContentType = s.ContentType;
            obj.Content = s.Content;
            obj.Description = s.Description;
            obj.ContentSubType = s.ContentSubType;
            obj.Link = s.Link;
            db.Snippets.Add(obj);
            db.SaveChanges();
            return obj.SnippetID;
        }
        
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> EditSnippet([FromBody] ISnippetModel s)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            var old = db.Snippets.FirstOrDefault(t => t.SnippetID == s.SnippetID && t.UserID==user.Id);
            if (old != null)
            {
                old.Link = s.Link;
                old.Content = s.Content;
                old.Description = s.Description;
                old.ContentType = s.ContentType;
                old.ContentSubType = s.ContentSubType;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                throw new ArgumentException("Not your Snippet!");
            }
            
        }
        
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSnippet(string id)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            var snippetToDelete = db.Snippets.FirstOrDefault(t => t.SnippetID == id && t.UserID==user.Id);
            if (snippetToDelete != null)
            {
                db.Snippets.Remove(snippetToDelete);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                throw new ArgumentException("Not your snippet!");
            }
            
        }
        //----> CRUD ending 
        
        [Authorize]
        [HttpGet("bytag/{tag}")]
        public IEnumerable<ISnippetModel> GetSnippetsByTag(string tag)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            return db.Snippets.Where(s => s.UserID==user.Id && 
                                          s.Tags.Any(t => t.TagName==tag)).Select(s => 
                new ISnippetModel(){CollectionID = s.CollectionID, SnippetID = s.SnippetID, Content = s.Content, ContentType = s.ContentType, ContentSubType = s.ContentSubType, Link = s.Link, Description = s.Description}
            );
        }
        
        [Authorize]
        [HttpGet("bycontent_type/{content_type}")]
        public IEnumerable<ISnippetModel> GetSnippetsByContentType(string content_type)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            return db.Snippets.Where(s => s.UserID==user.Id && 
                                          s.ContentType==content_type).Select(s => 
                new ISnippetModel(){CollectionID = s.CollectionID, SnippetID = s.SnippetID, Content = s.Content, ContentType = s.ContentType, ContentSubType = s.ContentSubType, Link = s.Link, Description = s.Description}
            );
        }
    }