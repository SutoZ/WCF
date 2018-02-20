using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCF
{
    class Rate
    {
        //Doksi mengézése

        public string Currency { get; private set; }
        public double Unit { get; private set; }
        public double Value { get; private set; }
        public DateTime Date { get; private set; }

        public Rate(string Currency /*a pénz neve*/,DateTime date, double value, double unit )
        {
            this.Currency = Currency;
            Date = date;
            Value = value;
            Unit = unit;
        }
    }
}
