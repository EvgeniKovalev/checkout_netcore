using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace checkout_netcore
{
  public class Cryptographer
  {
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static string GenerateShortId()
    {
      var startDate = new DateTime(2018, 1, 1);
      var cache = new Dictionary<long, IList<long>>();

      string secondPart = string.Empty;
      var now = DateTime.Now.ToString("HHmmssfff");
      var daysDiff = (DateTime.Today - startDate).Days;
      var current = long.Parse(string.Format("{0}{1}", daysDiff, now));

      if (cache.Any() && cache.Keys.Max() < current)
      {
        cache.Clear();
      }

      if (!cache.Any())
      {
        cache.Add(current, new List<long>());
      }

      if (cache[current].Any())
      {
        var maxValue = cache[current].Max();
        cache[current].Add(maxValue + 1);
        secondPart = maxValue.ToString(CultureInfo.InvariantCulture);
      }
      else
      {
        cache[current].Add(0);
      }

      var nextValueFormatted = string.Format("{0}{1}", current, secondPart);
      return UInt64.Parse(nextValueFormatted).ToString("X");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string CalculateHMAC(string key, string message)
    {
      var hmac = string.Empty;
      if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(message))
      {
        var encoding = new ASCIIEncoding();
        var keyBytes = encoding.GetBytes(key);
        var messageBytes = encoding.GetBytes(message);

        var hash = new HMACSHA256(keyBytes);
        byte[] hs = hash.ComputeHash(messageBytes);
        hmac = BitConverter.ToString(hs).Replace("-", "").ToLower();
      }
      return hmac;
    }
  }
}
