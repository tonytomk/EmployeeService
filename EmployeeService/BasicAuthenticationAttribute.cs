using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EmployeeService
{
    public class BasicAuthenticationAttribute :AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if(actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);
            }else
            {
                string authToken = actionContext.Request.Headers.Authorization
                    .Parameter;

                string decryptToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                string[] userpwdArray = decryptToken.Split(':');
                string uname = userpwdArray[0];
                string paswd = userpwdArray[1];

                if (EmployeeSecurity.Login(uname, paswd))
                {
                    // Creating generic principle and identity and setting that as the current principal
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(uname), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request
                   .CreateResponse(HttpStatusCode.Unauthorized);
                }

            }
        }
    }
}