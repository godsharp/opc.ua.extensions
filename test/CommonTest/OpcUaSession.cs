using System;
using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;

namespace CommonTest
{
    public class OpcUaSession
    {
        private readonly string _url;

        private readonly ApplicationInstance _application;

        /// <summary> 
        /// Keeps a session with an UA server.
        /// </summary>
        private Session _session = null;

        /// <summary>
        /// Provides the session being established with an OPC UA server.
        /// </summary>
        public Session Session
        {
            get { return _session; }
        }

        public OpcUaSession(string url)
        {
            _url = url;
            //GodSharp.OpcUaExtensions.Client.Config.xml
            _application = new ApplicationInstance();
            _application.ApplicationType = ApplicationType.Client;
            _application.ConfigSectionName = "GodSharp.OpcUaExtensions.Client";
            try
            {
                // load the application configuration.
                _application.LoadApplicationConfiguration(false).Wait();
                _application.ApplicationConfiguration.CertificateValidator.CertificateValidation += CertificateValidator_CertificateValidation;
                // check the application certificate.
                _application.CheckApplicationInstanceCertificate(false, 0).Wait();
            }
            catch (Exception)
            {
            }
        }

        public void Connect()
        {
            // select the best endpoint.
            var endpointDescription = CoreClientUtils.SelectEndpoint(_url, false, 3000);

            EndpointConfiguration endpointConfiguration = EndpointConfiguration.Create(_application.ApplicationConfiguration);
            endpointConfiguration.UseBinaryEncoding=true;
            ConfiguredEndpoint endpoint = new ConfiguredEndpoint(null, endpointDescription, endpointConfiguration);
            endpoint.BinaryEncodingSupport = BinaryEncodingSupport.Required;

            _session = Session.Create(
                _application.ApplicationConfiguration,
                endpoint,
                false,
                false,
                _application.ApplicationConfiguration.ApplicationName,
                60000,
                new UserIdentity(),
                null).Result;
        }

        public void Disconnect()
        {
            _session.Close(3000);
        }
        
        private void CertificateValidator_CertificateValidation(CertificateValidator sender, CertificateValidationEventArgs e)
        {
            if (ServiceResult.IsGood(e.Error))
                e.Accept = true;
            else if (e.Error.StatusCode.Code == StatusCodes.BadCertificateUntrusted)
                e.Accept = true;
            else
                throw new Exception($"Failed to validate certificate with error code {e.Error.Code}: {e.Error.AdditionalInfo}");
        }
    }
}
