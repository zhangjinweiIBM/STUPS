﻿/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 7/18/2014
 * Time: 11:37 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace Tmx.Client
{
    using System;
	using System.Net;
    using RestSharp;
	using TMX.Interfaces.Exceptions;
	using Tmx.Interfaces;
	using Tmx.Interfaces.Remoting;
	using Tmx.Server;
    
    /// <summary>
    /// Description of Registration.
    /// </summary>
    public class Registration
    {
        readonly RestRequestCreator _restRequestCreator = new RestRequestCreator();
        
        public int SendRegistrationInfoAndGetClientId()
		{
			var request = _restRequestCreator.GetRestRequest(UrnList.TestClients_Root + UrnList.TestClients_Clients, Method.POST);
			request.AddBody(getNewTestClient());
			var registrationResponse = _restRequestCreator.RestClient.Execute<TestClientInformation>(request);
			
			if (HttpStatusCode.Created == registrationResponse.StatusCode)
				return registrationResponse.Data.Id;
			throw new Exception("Failed to register a client"); // TODO: new type!
		}
        
        public void UnregisterClient()
        {
			var request = _restRequestCreator.GetRestRequest(UrnList.TestClients_Root + "/" + ClientSettings.ClientId, Method.DELETE);
			var unregistrationResponse = _restRequestCreator.RestClient.Execute(request);
			if (HttpStatusCode.OK != unregistrationResponse.StatusCode)
                throw new ClientDeregistrationException("Failed to unregister the client");
			cleanUpClientData();
        }
        
        IClientInformation getNewTestClient()
        {
            return new TestClientInformation {
                Hostname = Environment.MachineName,
                Username = Environment.UserName,
                UserDomainName = Environment.UserDomainName,
                IsInteractive = Environment.UserInteractive,
                // EnvironmentVersion = Environment.Version.Major + "." + Environment.Version.MajorRevision + "." + Environment.Version.Minor + "." + Environment.Version.MinorRevision + "." + Environment.Version.Build,
                Fqdn = string.Empty,
                OsVersion = Environment.OSVersion.VersionString,
                UptimeSeconds = Environment.TickCount / 1000
            };
        }
        
		void cleanUpClientData()
		{
			ClientSettings.ClientId = 0;
			ClientSettings.CurrentTask = null;
			ClientSettings.ServerUrl = string.Empty;
		}
    }
}
