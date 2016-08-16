using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetFlow
{
    public class ParametersError
    {
        public enum Error
        {
            ASSEMBLY_NOT_FOUND,
            CONTROLLER_EXCEPCTION_DEFINITION_CLASS_NOT_FOUND,
            HANDLER_EXCEPTION_LOCATOR_CLASS_NOT_FOUND,
            TYPE_DEFINITION_NOT_FOUND_IN_ASSEMBLY
        }
    }
}
