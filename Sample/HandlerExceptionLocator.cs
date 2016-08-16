using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetFlow;

namespace DotNetFlow
{
    class HandlerExceptionLocator
    {
        [econtext(econtextAttribute.SiteTypeEnum.HandlingSite, "hsite1")]
        public void MetodoTratadorSqlException()
        {
            Console.WriteLine("Tratador do SqlException");
        }

        [econtext(econtextAttribute.SiteTypeEnum.HandlingSite, "hsite2")]
        public void MetodoTratadorInvalidOperationException()
        {
            Console.WriteLine("Tratador do InvalidOperationException");
        }

    }
}
