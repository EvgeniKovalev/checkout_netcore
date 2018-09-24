using System.Collections.Generic;

namespace checkout_netcore.Models
{
  public class Order
  {
    public string stamp { get; set; }
    public string reference { get; set; }
    public int amount { get; set; }
    public string currency { get; set; }
    public string language { get; set; }
    public List<Item> items { get; set; } = new List<Item>();
    public Customer customer { get; set; } = new Customer();
    public PostAddress deliveryAddress { get; set; } = new PostAddress();
    public UrlPair redirectUrls { get; set; } = new UrlPair();

    public Order() { }
  }
}