using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnipSmart.Models;

public class Snippet : ISnippetModel
{
    [Key]
    public string SnippetID { get; set; }
    public string UserID { get; set; }
    public string CollectionID { get; set; }
    
    //virtual members
    [NotMapped]
    public virtual Collection? Collection { get; set; } 
    [NotMapped]
    public virtual ICollection<Tag>? Tags { get; set; }
    [NotMapped]
    public virtual  User? User { get; set; } //virtual member
    
    public string Link { get; set; }
    //public byte[] File { get; set; }
    public string ContentType { get; set; }
    public string ContentSubType { get; set; }
    public string Content { get; set; }
    public string Description { get; set; }

    public Snippet()
    {
        this.CollectionID = Guid.NewGuid().ToString();
    }
}