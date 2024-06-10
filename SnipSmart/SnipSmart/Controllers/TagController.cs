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
        
        //[Route("[action]")]
        [Authorize] //works
        [HttpPost("AddTagToSnippet")]
        public async Task<string> AddTagToSnippet(SnippetAndTag args)
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
                    tag.SnippetID = snippet.SnippetID;
                    tag.UserID = user.Id;
                    snippet.Tags.Add(tag);
                    db.SaveChanges();
                    return tag.TagID;
                }
            }

            return "1";

        }
        //[Route("[action]")]
        [Authorize]
        [HttpDelete("RemoveTagFromSnippet")]
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
        //[Route("[action]")]
        [Authorize] //works
        [HttpGet("GetTagsFromSnippet/{SnippetID}")]
        public IEnumerable<ITagModel> GetTagsFromSnippet(string SnippetID)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            return db.Snippets
                .Where(s => s.SnippetID == SnippetID && s.UserID == user.Id)
                .SelectMany(s => s.Tags)
                .Select(t => new ITagModel()
                {
                    SnippetID = t.SnippetID,
                    TagID = t.TagID,
                    TagName = t.TagName
                });

        }
        
        
        //CRUD starting ---->
        [Authorize]
        [HttpGet("GetDistinctNames")] //works
        public IEnumerable<String> GetDistinctNames()
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            return db.Tags.Where( s => s.UserID==user.Id).Select(s => s.TagName).Distinct();
        }
        [Authorize]
        [HttpGet] //works
        public IEnumerable<ITagModel> GetTags()
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            return db.Tags.Where( s => s.UserID==user.Id).Select(s => new ITagModel()
            {
                TagName = s.TagName,
                SnippetID = s.SnippetID,
                TagID = s.TagID
            });
        }
        
        [HttpGet("{id}")]
        [Authorize] //works
        public ITagModel GetTag(string id)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            return db.Tags.Where(t => t.TagID == id && t.UserID == user.Id).Select(t => new ITagModel()
            {
                SnippetID = t.SnippetID,
                TagID = t.TagID,
                TagName = t.TagName
            }).FirstOrDefault();
        }
        
        [Authorize]
        [HttpPut] //works
        public async Task<IActionResult> EditTag([FromBody] ITagModel s)
        {
            var user = _userManager.Users.FirstOrDefault
                (t => t.UserName == this.User.Identity.Name);
            var old = db.Tags.Where(t => t.TagID == s.TagID && t.UserID==user.Id).FirstOrDefault() ;
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
        [Authorize] //works
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