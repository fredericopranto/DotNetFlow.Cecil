using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DotNetFlow;

namespace DotNetFlow
{
    /// <summary>
    /// 
    /// </summary>
    class ControllerExceptionDefinition
    {
        
        //TODO: Mudar para EnumAttribute
        enum ControllerException {

            //[econtext(econtextAttribute.SiteTypeEnum.RaisingSite, "rsite1")]
            //[echannel("eec1", typeof(System.InvalidOperationException), "rsite1")]
            //[ehandler("eec1", typeof(System.InvalidOperationException), "hsite1")]
            Sample_ClasseVisao_CadastrarUsuario,
            
            Sample_ClasseVisao_ListarUsuarios
        };


        [econtext(econtextAttribute.SiteTypeEnum.RaisingSite, "rsite1")]
        [echannel("eec1", typeof(System.InvalidOperationException), "rsite1")]
        [ehandler("eec1", typeof(System.InvalidOperationException), "hsite1")]
        object Sample_ClasseVisao_CadastrarUsuario { get; set; }

        [econtext(econtextAttribute.SiteTypeEnum.RaisingSite, "rsite2")]
        [echannel("eec2", typeof(System.InvalidOperationException), "rsite2")]
        [ehandler("eec2", typeof(System.InvalidOperationException), "hsite2")]
        object Sample_ClasseVisao_ListarUsuarios { get; set; }
    }
}
