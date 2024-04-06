using SnipSmart.Data;
using SnipSmart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace SnipSmart.Controllers;

[ApiController]
[Route("[controller]")]
public class TagController : ControllerBase
{
        ApplicationDbContext db;
        UserManager<User> _userManager;

        public TagController(ApplicationDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            _userManager = userManager;
        }
        //CRUD starting ---->
        [HttpGet]
        public IEnumerable<ITagModel> GetTags()
        {
            return db.Tags;
        }

        [HttpGet("{id}")]
        [Authorize]
        public ITagModel? GetTags(string id)
        {
            return db.Tags.FirstOrDefault(t => t.TagID == id);
        }

        [Route("[action]")]
        [Authorize]
        [HttpPost]
        public async void AddTag([FromBody] ITagModel s)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            db.Tags.Add(s as Tag);
            s.UserID = user.Id;
            s.TagID = Guid.NewGuid().ToString();
            db.SaveChanges();
        }
    
        [HttpPut]
        public async Task<IActionResult> EditTag([FromBody] ITagModel s)
        {
            var userName = this.User.Identity.Name;
            var old = db.Tags.FirstOrDefault(t => t.TagID == s.TagID);
            if (old.User.UserName == userName)
            {
                old.TagName = s.TagName;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                throw new ArgumentException("Not your Tag!");
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollection(string id)
        {
            var userName = this.User.Identity.Name;
            var tagToDelete = db.Tags.FirstOrDefault(t => t.TagID == id);
            if (tagToDelete.User.UserName == userName)
            {
                db.Tags.Remove(tagToDelete);
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