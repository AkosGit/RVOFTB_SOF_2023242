using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SnipSmart.Models;

public class User : IdentityUser
{


    //virtual members
    [NotMapped]
    public virtual ICollection<Snippet> Snippets { get; } = new List<Snippet>();
    [NotMapped]
    public virtual ICollection<Collection> Collections { get; } = new List<Collection>();
    [NotMapped]
    public virtual ICollection<Tag> Tags { get; } = new List<Tag>();
}