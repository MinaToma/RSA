using System;
using System.Collections.Generic;
using System.Text;

namespace RSA
{
    public class DivisionResult
    {
        public List<char> quotient { get; set; }
        public List<char> remainder { get; set; }

        public DivisionResult(List<char> q, List<char> r)
        {
            quotient = q;
            remainder = r;
        }
    }
}
