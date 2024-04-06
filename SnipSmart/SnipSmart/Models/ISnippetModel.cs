namespace SnipSmart.Models;

public interface ISnippetModel
{
    public string SnippetID { get; set; }
    public string UserID { get; set; }
    public string CollectionID { get; set; }
    
    public string Link { get; set; }
    //public byte[] File { get; set; }
    public string ContentType { get; set; }
    public string ContentSubType { get; set; }
    public string Content { get; set; }
    public string Description { get; set; }
}