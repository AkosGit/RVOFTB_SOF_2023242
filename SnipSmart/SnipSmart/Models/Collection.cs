using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnipSmart.Models;

public class Collection : ICollectionModel
{
    
    [Key]
    public string? CollectionID { get; set; }
    [ForeignKey("User")]
    [MaxLength(450)]
    public string UserID { get; set; }


    [NotMapped]
    public virtual User User { get; set; } //virtual member
    [NotMapped]
    public virtual ICollection<Snippet>? Snippets { get; } = new List<Snippet>(); //virtual member
    
    public string CollectionName { get; set; }

    public Collection()
    {
        this.CollectionID = Guid.NewGuid().ToString();
    }
}