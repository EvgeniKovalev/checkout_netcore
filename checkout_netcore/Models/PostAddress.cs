namespace checkout_netcore.Models
{
  public class PostAddress
  {
    public string streetAddress { get; set; }
    public string postalCode { get; set; }
    public string city { get; set; }
    public string country { get; set; }

    public PostAddress() { }
  }
}