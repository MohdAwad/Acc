using System;
using System.ComponentModel.DataAnnotations;

namespace Acc.Models
{
    internal class DisplayTypeAttribute : Attribute
    {
        private DataType date;

        public DisplayTypeAttribute(DataType date)
        {
            this.date = date;
        }
    }
}