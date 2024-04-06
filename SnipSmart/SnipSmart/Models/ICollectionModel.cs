namespace SnipSmart.Models;

public interface ICollectionModel
{
    public string CollectionID { get; set; }
    public string UserID { get; set; }
    
    public string CollectionName { get; set; }
}