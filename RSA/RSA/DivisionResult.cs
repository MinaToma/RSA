using System;
using System.Collections.Generic;
using System.Text;

namespace RSA
{
    public class DivisionResult
    {
        public List<int> quotient { get; set; }
        public List<int> remainder { get; set; }

        public DivisionResult(List<int> q, List<int> r)
        {
            quotient = q;
            remainder = r;
        }
    }
}
