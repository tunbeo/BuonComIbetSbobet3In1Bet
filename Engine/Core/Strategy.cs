using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iBet.DTO
{
    class Strategy
    {
        public string Name
        {
            get;
            set;
        }
        public int StepID
        {
            get;
            set;
        }
        public string Field
        {
            get;
            set;
        }
        public string Operator
        {
            get;
            set;
        }
        public string ValueToCompare
        {
            get;
            set;
        }
        public string Obj
        {
            get;
            set;
        }
        public string FinalAction
        {
            get;
            set;
        }
        public bool Expected
        {
            get;
            set;
        }
        public bool Result
        {
            get;
            set;
        }
        public bool AdminAllowed
        {
            get;
            set;
        }
    }
}
