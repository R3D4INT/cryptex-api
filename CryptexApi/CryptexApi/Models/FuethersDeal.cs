using CryptexApi.Enums;
using CryptexApi.Models.Base;

namespace CryptexApi.Models
{
    public class FuethersDeal : DealBase
    {
        public TypeOfFuetersDeal TypeOfDeal { get; set; }
        public int Leverage { get; set; }
        public TypeOfMargin TypeOfMargin { get; set; }
        public double TakeProfit { get; set; }
        public double StopLoss { get; set; }
        public double EnterPrice { get; set; }
        public double MarginValue { get; set; }
        public Status Status { get; set; }
        public double Amount { get; set; }
    }
}
