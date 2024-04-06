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
        public IEnumerable<ISnippetModel> GetSnippets()
        {
            return db.Snippets;
        }

        [HttpGet("{id}")]
        [Authorize]
        public ISnippetModel? GetSnippets(string id)
        {
            return db.Snippets.FirstOrDefault(t => t.SnippetID == id);
        }

        [Route("[action]")]
        [Authorize]
        [HttpPost]
        public async void AddSnippet([FromBody] ISnippetModel s)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            db.Snippets.Add(s as Snippet);
            s.UserID = user.Id;
            s.SnippetID = Guid.NewGuid().ToString();
            db.SaveChanges();
        }
    
        [HttpPut]
        public async Task<IActionResult> EditSnippet([FromBody] ISnippetModel s)
        {
            var userName = this.User.Identity.Name;
            var old = db.Snippets.FirstOrDefault(t => t.SnippetID == s.SnippetID);
            if (old.User.UserName == userName)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSnippet(string id)
        {
            var userName = this.User.Identity.Name;
            var snippetToDelete = db.Snippets.FirstOrDefault(t => t.SnippetID == id);
            if (snippetToDelete.User.UserName == userName)
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