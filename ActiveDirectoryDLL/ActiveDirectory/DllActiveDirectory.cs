using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ActiveDirectory
{
    [ComVisible(true)]
    [Guid("88EBBF9D-D25B-40CF-A9FD-0FE8A0172FDB")]
    public interface IdllActiveDirectory
    {
        [DispId(1)]
        string UnlockUser(string strNumberEmployee, string strNip);
    }

    [ComVisible(true)]
    [Guid("34863CF8-EB3D-4716-8BDB-8B78F060C875")]
    [ProgId("ProgId.DllActiveDirectory")]
    public class DllActiveDirectory : IdllActiveDirectory
    {
        private ServiceActiveDirectory.IActiveDirectory objClient;
        private ServiceActiveDirectory.ActiveDirectoryRequest objReq;
        private ServiceActiveDirectory.ActiveDirectoryInfo objInfo;
        private CustomBinding binding;
        private EndpointAddress endpoint;

        public DllActiveDirectory()
        {
            SecurityBindingElement securityElement = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
            securityElement.IncludeTimestamp = false;
            TextMessageEncodingBindingElement encodingElement = new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8);
            HttpsTransportBindingElement transportElement = new HttpsTransportBindingElement();

            binding = new CustomBinding(encodingElement, transportElement);
            endpoint = new EndpointAddress("http://mga-bj05-00/wsActiveDirectory/Service.svc/ActiveDirectory");
            objClient = new ServiceActiveDirectory.ActiveDirectoryClient(binding, endpoint);


            objReq = new ServiceActiveDirectory.ActiveDirectoryRequest();
        }
        string IdllActiveDirectory.UnlockUser(string strNumberEmployee, string strNip)
        {
            string result=string.Empty;
            string code=string.Empty;

            Dictionary<string, string> objDataCenter= new Dictionary<string,string>();
            objDataCenter= GetDataCenter(strNumberEmployee);

            objReq.domainActDir = objDataCenter["Domain"];
            objReq.password = "AxtelA4011692d10";
            objReq.passwordActDir = objDataCenter["AdminPass"];
            objReq.pathActDir = objDataCenter["Ldap"];
            objReq.userService = "AxtelA104D102194";
            objReq.usernameActDir = objDataCenter["Admin"];
            objReq.userAuthenticate = string.Empty;
            objReq.passwordAuthenticate = string.Empty;
            objReq.numberEmployee = strNumberEmployee;
            objReq.nip = strNip;
            objReq.confirmNip = strNip;
            objReq.flag = 1;

            /*
            objInfo = objClient.UnlockEmployee(objReq);
            result = objInfo.result;
            code = objInfo.code;
             */

            WSHttpBinding myBinding = new WSHttpBinding();
            myBinding.Security.Mode = SecurityMode.None;
            myBinding.Security.Message.ClientCredentialType =  MessageCredentialType.None;

            // Create the endpoint address. 
            EndpointAddress ea = new  EndpointAddress("http://mga-bj05-00/wsActiveDirectory/Service.svc/ActiveDirectory");

            // Create the client. 
            ServiceActiveDirectory.IActiveDirectory objCliente = new ServiceActiveDirectory.ActiveDirectoryClient(myBinding, ea);
            objInfo = objCliente.UnlockEmployee(objReq);
            result = objInfo.result;
            code = objInfo.code;

            if (code.Equals("8"))
                code = "1";
            else
                code = "0";

            return code;
        }
        
        public Dictionary<string, string> GetDataCenter(string strNumberEmployee)
        {
            Dictionary<string, string> objDataCenter = new Dictionary<string, string>();

            if(strNumberEmployee.Substring(0,1)=="0")//Alestra
            {
                objDataCenter.Add("Ldap", "LDAP://172.26.0.81/DC=e-alestra,DC=com");
                objDataCenter.Add("Domain", "e-alestra.com");
                objDataCenter.Add("Admin", "_ivrcts@e-alestra.com");
                objDataCenter.Add("AdminPass", "1VRCt3Ct5");
            }
            else//Axtel
            {
                objDataCenter.Add("Ldap", "LDAP://DOMCTI.com:389/dc=DOMCTI,dc=com");
                objDataCenter.Add("Domain", "DOMCTI.com");
                objDataCenter.Add("Admin", "ctiuserd");
                objDataCenter.Add("AdminPass", "4xt3l.2016");
            }

            return objDataCenter;
        }
    }
}
