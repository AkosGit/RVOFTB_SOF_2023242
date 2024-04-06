namespace SnipSmart.Models;

public interface ITagModel
{
    public string SnippetID { get; set; }
    public string TagID { get; set; }
    public string UserID { get; set; }
    
    public string TagName{ get; set; }
}