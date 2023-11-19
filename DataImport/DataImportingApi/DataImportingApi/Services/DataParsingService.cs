using DataImportingApi.Models;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace DataImportingApi.Services
{
    public class DataParsingService
    {
        public ExpenseData ExtractAndCalculate(string data)
        {
            var expenseData = new ExpenseData();


            try
            {
                List<string> tags = ExtractTags(data);


                if (!HasMatchingTags(tags))
                {
                    throw new InvalidOperationException("Error: Opening tags without corresponding closing tags. Message rejected.");
                }

                string costCentre = ExtractTagValue(data, "cost_centre");
                string total = ExtractTagValue(data, "total");

                if (string.IsNullOrEmpty(total))
                {
                    throw new InvalidOperationException("Error: Missing <total>. Message rejected.");
                }
                if (string.IsNullOrEmpty(costCentre))
                {
                    costCentre = "UNKNOWN"; // Default cost centre value if missing
                }

                string totalAmount = ExtractTagValue(data, "total");


                int totalIncludingTax = -1;


                totalAmount = totalAmount.Replace(",", "").Trim();
                totalIncludingTax = Convert.ToInt32(totalAmount);
                expenseData.Total =  totalIncludingTax;
                expenseData.SalesTax = CalculateSalesTax(expenseData.Total);
                expenseData.TotalExcludingTax = expenseData.Total - expenseData.SalesTax;
                expenseData.CostCentre = costCentre;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error processing the text: {ex.Message}");
            }

            return expenseData;
        }



        private decimal CalculateSalesTax(decimal total)
        {
           
            decimal taxRate = 0.10m; // 10% tax rate

            decimal salesTax = total * taxRate;
            return salesTax;
        }
        public static string ExtractTagValue(string text, string tagName)
        {
            string openingTag = $"<{tagName}>";
            string closingTag = $"</{tagName}>";

            int startIndex = text.IndexOf(openingTag);
            int endIndex = text.IndexOf(closingTag);

            if (startIndex != -1 && endIndex != -1)
            {
                return text.Substring(startIndex + openingTag.Length, endIndex - startIndex - openingTag.Length);
            }

            return null;
        }
        public static bool HasMatchingTags(List<string> tags)
        {
            //string tagName;
            //foreach (string tag in text.Split())
            //{ 
            //string openingTag = $"<{tag}>";
            //string closingTag = $"</{tag}>";

            //int openingTagCount = text.Split(openingTag).Length - 1;
            //int closingTagCount = text.Split(closingTag).Length - 1;

            //return openingTagCount == closingTagCount;
            //}
            //return false;

            int openTagCount = 0;
            int closeTagCount = 0;

            foreach (var tag in tags)
            {
                if (tag.StartsWith('/'))
                {
                    closeTagCount++;
                    //if (openTagCount < 0)
                    //{
                    //    return true; // Closing tag without corresponding opening tag found
                    //}
                }
                else
                {
                    openTagCount++;
                }
            }
            if(openTagCount==closeTagCount)
                return true;
            else
                return false;
            //return openTagCount == 0;
        }

        public static List<string> ExtractTags(string text)
        {
            List<string> tags = new List<string>();

            int startIndex = -1;
            int endIndex = -1;

            while (true)
            {
                startIndex = text.IndexOf('<', startIndex + 1);
                endIndex = text.IndexOf('>', endIndex + 1);

                if (startIndex == -1 || endIndex == -1)
                {
                    break;
                }

                tags.Add(text.Substring(startIndex + 1, endIndex - startIndex - 1));
            }
            return tags;


        }
    }
}
