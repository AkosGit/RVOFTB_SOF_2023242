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
        
        public class SnippetAndTag
        {
            public string SnippetID { get; set; }
            public string TagName { get; set; }
        }
        [Route("[action]")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddTagToSnippet(SnippetAndTag args)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            var snippet = db.Snippets.Where(s => s.SnippetID == args.SnippetID && s.UserID == user.Id).FirstOrDefault();
            if (snippet != null)
            {
                if (!snippet.Tags.Where(t => t.TagName == args.TagName && t.SnippetID == args.SnippetID && t.UserID==user.Id).Any())
                {
                    Tag tag = new Tag();
                    tag.TagName = args.TagName;
                    tag.UserID = user.Id;
                    AddTag(tag);
                    snippet.Tags.Add(tag);
                    db.SaveChanges();
                }
                return Ok();
            }
            return Unauthorized();

        }
        [Route("[action]")]
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> RemoveTagFromSnippet(SnippetAndTag args)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            var snippet = db.Snippets.Where(s => s.SnippetID == args.SnippetID && s.UserID==user.Id).FirstOrDefault();
            if (snippet != null)
            {
                var tag = snippet.Tags.Where(t => t.TagName == args.TagName && t.UserID==user.Id).First();
                snippet.Tags.Remove(tag);
                db.SaveChanges();
                return Ok();
            }
            return Unauthorized();

        }
        [Route("[action]")]
        [Authorize]
        [HttpGet]
        public async Task<ICollection<ITagModel>> GetTagsFromSnippet([FromBody] string SnippetID)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            var tags = db.Snippets.Where(s => s.SnippetID==SnippetID && s.UserID==user.Id).Select(s => s.Tags);
            if (tags != null)
            {
                return tags.Select(s => s as ITagModel).ToList();
            }

            return new List<ITagModel>();

        }
        
        
        //CRUD starting ---->
        [Authorize]
        [HttpGet]
        public IEnumerable<ITagModel> GetTags()
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            return db.Tags.Where( s => s.UserID==user.Id).Select( s=> s as ITagModel);
        }
        
        [HttpGet("{id}")]
        [Authorize]
        public ITagModel GetTags(string id)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            return db.Tags.FirstOrDefault(t => t.TagID == id && t.UserID == user.Id)  as  ITagModel;
        }

        //[Route("[action]")]
        //[Authorize]
        //[HttpPost]
        [NonAction]
        public async void AddTag([FromBody] ITagModel s)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            db.Tags.Add(s as Tag);
            s.UserID = user.Id;
            s.TagID = Guid.NewGuid().ToString();
            db.SaveChanges();
        }
    
        [NonAction]
        public async Task<IActionResult> EditTag([FromBody] ITagModel s)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            var old = db.Tags.FirstOrDefault(t => t.TagID == s.TagID && s.UserID==user.Id) ;
            if (old != null)
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
        [Authorize]
        public async Task<IActionResult> DeleteTag(string id)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            var tagToDelete = db.Tags.FirstOrDefault(t => t.TagID == id && t.UserID==user.Id);
            if (tagToDelete != null)
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