using System.Collections.Generic;

namespace checkout_netcore.Models
{
  public class PaymentProvider
    {
    public string url { get; set; }
    public string icon { get; set; }
    public string svg { get; set; }
    public string name { get; set; }
    public string group { get; set; }
    public string id { get; set; }
    public List<PaymentProviderParameter> parameters { get; set; } = new List<PaymentProviderParameter>();

    public PaymentProvider() { }
  }
}