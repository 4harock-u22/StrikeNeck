using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace u22_strikeneck.Camera
{
    public class PictureQuantity
    {
        private const int MIN_QUANTITY = 1;
        private const int DEFAULT_QUANTITY = 1;
        private const int MAX_QUANTITY = 100;
        private int quantity;

        public PictureQuantity(int quantity = DEFAULT_QUANTITY)
        {
            if (quantity < MIN_QUANTITY || quantity > MAX_QUANTITY)
                throw new ArgumentOutOfRangeException($"quantity must be between {MIN_QUANTITY} and {MAX_QUANTITY}");
            
            this.quantity = quantity;
        }

        public int Value
        {
            get
            {
                return quantity;
            }
        }
    }
}
