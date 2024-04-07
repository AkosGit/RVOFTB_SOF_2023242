using SnipSmart.Data;
using SnipSmart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            return db.Snippets.Where(s => s.UserID==user.Id).Select(s => s as ISnippetModel);
        }

        [HttpGet("{id}")]
        [Authorize]
        public ISnippetModel? GetSnippets(string id)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            return db.Snippets.Select(s => s as ISnippetModel).FirstOrDefault(t => t.SnippetID == id && t.UserID==user.Id);
        }

        //[Route("[action]")]
        [Authorize]
        [HttpPost]
        public async void AddSnippet([FromBody] Snippet s)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            s.UserID = user.Id;
            s.SnippetID = Guid.NewGuid().ToString();
            db.Snippets.Add(s as Snippet);
            db.SaveChanges();
        }
        
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> EditSnippet([FromBody] Snippet s)
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
    }