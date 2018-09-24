using checkout_netcore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace checkout_netcore.Controllers
{
  public class CartController : Controller
  {
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public ActionResult Index()
    {
      // Building test order
      var order = new Order();
      order.stamp = Cryptographer.GenerateShortId();
      order.reference = "123456";
      order.currency = "EUR";
      order.language = "FI";

      // Items
      var itemA = new Item();
      itemA.units = 7;
      itemA.unitPrice = 155;
      itemA.vatPercentage = 24;
      itemA.productCode = "#927502759";
      itemA.deliveryDate = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd");
      order.items.Add(itemA);

      var itemB = new Item();
      itemB.units = 11;
      itemB.unitPrice = 5350;
      itemB.vatPercentage = 13;
      itemB.productCode = "#98274242";
      itemB.deliveryDate = DateTime.Now.AddDays(5).ToString("yyyy-MM-dd");
      order.items.Add(itemB);

      // Order sum
      order.amount = order.items.Sum(i => (i.unitPrice * i.units));

      // Customer
      order.customer.email = "john.doe@example.org";

      // Address
      order.deliveryAddress.streetAddress = "Fake street 123";
      order.deliveryAddress.postalCode = "00100";
      order.deliveryAddress.city = "Lulea";
      order.deliveryAddress.country = "Sweden";

      // Redirect urls
      order.redirectUrls.success = "https://example.org/success";
      order.redirectUrls.cancel = "https://example.org/cancel";

      return View(order);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    [ValidateAntiForgeryToken, HttpPost]
    public ActionResult StartTransaction(Order order)
    {
      var ACCOUNT = "375917";
      var SECRET = "SAIPPUAKAUPPIAS";
      var API_URL = "https://api.checkout.fi/payments";

      // New payment transaction
      var transaction = new PaymentTransaction();
      var client = new HttpClient();
      try
      {
        var json = JsonConvert.SerializeObject(order);
        var buffer = Encoding.UTF8.GetBytes(json);
        var content = new ByteArrayContent(buffer);

        // Headers without signature
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var headers = new Dictionary<string, string>();
        headers.Add("checkout-account", ACCOUNT);
        headers.Add("checkout-algorithm", "sha256");
        headers.Add("checkout-method", "POST");
        headers.Add("checkout-nonce", "564635208570151");
        headers.Add("checkout-timestamp", "2018-03-08T10:02:31.904Z");

        var headersStr = string.Empty;
        foreach (var headeKvp in headers)
        {
          headersStr = string.Format("{0}{1}:{2}\n", headersStr, headeKvp.Key, headeKvp.Value);
          content.Headers.Add(headeKvp.Key, headeKvp.Value);
        }

        var hmacPayload = string.Format("{0}{1}", headersStr, json);
        var hmac = Cryptographer.CalculateHMAC(SECRET, hmacPayload);

        //add signature to headers
        content.Headers.Add("signature", hmac);

        var response = client.PostAsync(API_URL, content);
        var responseText = response.Result.Content.ReadAsStringAsync();
        transaction = JsonConvert.DeserializeObject<PaymentTransaction>(responseText.Result);
      }
      catch (Exception ex)
      {
      }
      return View(transaction);
    }
  }
}