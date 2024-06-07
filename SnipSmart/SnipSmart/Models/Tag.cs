using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnipSmart.Models;

public class Tag : ITagModel
{
    public string? SnippetID { get; set; }
    [Key]
    public string? TagID { get; set; }
    [ForeignKey("User")]
    [MaxLength(450)]
    public string UserID { get; set; }
    
    //Virtual members
    [NotMapped]
    public virtual User User { get; set; } 
    [NotMapped]
    public virtual Snippet? Snippet { get; set; }

    public string TagName{ get; set; }

    public Tag()
    {
        this.TagID = Guid.NewGuid().ToString();
    }
}