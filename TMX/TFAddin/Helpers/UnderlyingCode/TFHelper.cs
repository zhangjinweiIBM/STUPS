﻿/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 11/9/2012
 * Time: 6:44 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace TMX
{
    using System;
    using System.Management.Automation;
    using System.Net;
    using System.Collections.Specialized;
    using System.Globalization;
    //using Microsoft.TeamFoundation
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.TestManagement.Client;
    using Microsoft.TeamFoundation.WorkItemTracking.Client;
    using Microsoft.TeamFoundation.Client.Channels;
    using Microsoft.TeamFoundation.Framework.Client;
    
    /// <summary>
    /// Description of TFHelper.
    /// </summary>
    internal class TFHelper
    {
        public TFHelper()
        {
        }
        
        public static void ConnectTFServer(TFSCmdletBase cmdlet, string url, ICredentials credentials)
        {
            try
            {
                Uri uri =
                    new Uri(url);
                
                TfsConfigurationServer tfsServer =
                    new TfsConfigurationServer(
                        uri,
                        credentials);
                
                cmdlet.WriteVerbose(
                    cmdlet,
                    "Connected, checking the connection");
                
                try {
                    tfsServer.EnsureAuthenticated();
                }
                catch (Exception eTfsServerConnected) {
                    
                    cmdlet.WriteError(
                        cmdlet,
                        "Failed to connect to server '" +
                        url +
                        "'." +
                        eTfsServerConnected.Message,
                        "FailedToConnect",
                        ErrorCategory.InvalidResult,
                        true);
                }
                
                CurrentData.CurrentServer = tfsServer;
                
                cmdlet.WriteVerbose(cmdlet,
                                    "Connected to: '" +
                                    url + 
                                    "'");
                
                cmdlet.WriteObject(cmdlet, tfsServer);
            }
//            catch (TeamFoundationServerUnauthorizedException ex)
//            {
//                // handle access denied
//            }
//            catch (TeamFoundationServiceUnavailableException ex)
//            {
//                // handle service unavailable
//            }
            catch (WebException ex)
            {
                // handle other web exception
            }
        }
        
        public static void OpenProjectCollection(TFSCmdletBase cmdlet, string name)
        {
            try
            {
                var projectCollectionUri =
                    new Uri(
                        CurrentData.CurrentServer.Uri.OriginalString +
                        "/" +
                        name);
                
                cmdlet.WriteVerbose(
                    cmdlet,
                    "connecing to the collection: '" +
                    projectCollectionUri.ToString() +
                    "'");
                
                var projectCollection =
                    TfsTeamProjectCollectionFactory.GetTeamProjectCollection(projectCollectionUri);
                
                cmdlet.WriteVerbose(
                    cmdlet,
                    "Collection is connected, checking the connection");
                
                try {
                    projectCollection.EnsureAuthenticated();
                }
                catch (Exception eTfsCollectionConnected) {
                    
                    cmdlet.WriteError(
                        cmdlet,
                        "Failed to connect to collection '" +
                        projectCollectionUri.ToString() +
                        "'." +
                        eTfsCollectionConnected.Message,
                        "FailedToConnect",
                        ErrorCategory.InvalidResult,
                        true);
                }
                
                cmdlet.WriteVerbose(cmdlet, "connected to the collection");
                
                CurrentData.CurrentCollection = projectCollection;
                
                cmdlet.WriteObject(cmdlet, projectCollection);
            }
//            catch (TeamFoundationServerUnauthorizedException ex)
//            {
//                // handle access denied
//            }
//            catch (TeamFoundationServiceUnavailableException ex)
//            {
//                // handle service unavailable
//            }
            catch (WebException ex)
            {
                // handle other web exception
            }
            
        }
        
        public static void OpenProject(TFSCmdletBase cmdlet, string name)
        {

            try {
                
                ITestManagementService testMgmtSvc =
                    (ITestManagementService)CurrentData.CurrentCollection.GetService(typeof(ITestManagementService));

                WorkItemStore store =
                    (WorkItemStore)CurrentData.CurrentCollection.GetService(typeof(WorkItemStore));

                ITestManagementTeamProject project =
                    testMgmtSvc.GetTeamProject(name);
                
                cmdlet.WriteVerbose(
                    cmdlet,
                    "Connected to the project '" +
                    name +
                    "'");
                
                CurrentData.CurrentProject = project;
                
                cmdlet.WriteObject(cmdlet, project);
                
            }
            catch (Exception eOpenProject) {
                
                cmdlet.WriteError(
                    cmdlet,
                    "Failed to connect to project '" +
                    name +
                    "'." +
                    eOpenProject.Message,
                    "FailedToConnect",
                    ErrorCategory.InvalidResult,
                    true);
            }
        }
        
        public static void NewTestPlan(TFSCmdletBase cmdlet, string name)
        {
            try {
                ITestPlan testPlan =
                    CurrentData.CurrentProject.TestPlans.Create();
                testPlan.Name = name;
                testPlan.Save();
                
                CurrentData.CurrentTestPlan = testPlan;
                
                cmdlet.WriteObject(cmdlet, testPlan);
                
            }
            catch (Exception eNewTestPlan) {
                Console.WriteLine(eNewTestPlan.Message);
            }
        }
        
        public static ITestPlan GetTestPlan(TFSCmdletBase cmdlet, string name) // ??
        {
            throw new NotImplementedException();
        }
        
        public static ITestPlan OpenTestPlan(TFSCmdletBase cmdlet, string name)
        {
            throw new NotImplementedException();
        }
        
        public static void AddTestSuite(TFSCmdletBase cmdlet, string name)
        {
            //throw new NotImplementedException();
            try {
                //CurrentData.CurrentProject.TestPlans.
                //ITestSuiteEntry testSuite =
//                ITestSuiteBase testSuite =
//                    new stat
//                    CurrentData.CurrentTestPlan.RootSuite.Entries.Add("asdf");

//                string[] areas = full_area.Split('\\');
//                string full_path = string.Empty;
                IStaticTestSuite suite =// null;
                    CurrentData.CurrentProject.TestSuites.CreateStatic();
//                string current_area = string.Empty;
                
                suite.Title = name;
                //CurrentData.CurrentTestPlan.RootSuite.Entries.Add(suite);
                //ITestSuiteEntryCollection suites =
                //var suites =
                
                //CurrentData.CurrentTestPlan.RootSuite.Entries.Add(suite);
                

//                for (int i = 0; i < areas.Length; i++)
//                {
//                    if (!string.IsNullOrEmpty(areas[i]))
//                    {
//                        string area = areas[i].RemoveBadChars();
//                        current_area += area;
//
//                        //The first item, find it and assigned to suite object.
//                        if (i == 1)
//                        {
//                            ITestSuiteEntryCollection collection = CurrentData.CurrentTestPlan.RootSuite.Entries;
//                            suite = TestHelper.FindSuite(collection, area);
//                            if (suite.Id == 0)
//                            {
//                                suite.Title = area;
//                                TestHelper.AddTests(suite, current_area);
//                                CurrentData.CurrentTestPlan.RootSuite.Entries.Add(suite);
//                            }
//                        }
//                        else
//                        {
//                            ITestSuiteEntryCollection collection = suite.Entries;
//                            //* collection - Perform search only under the suite.Entries  - Duplicate items allowed. 
//                            IStaticTestSuite subSuite = TestHelper.FindSuite(collection, area);
//
//                            if (subSuite.Id == 0)
//                            {//Cannot find Test Suite
//                                subSuite.Title = area;
//                                suite.Entries.Add(subSuite);
//                                //After creating the Test Suite - Add the related TestCases based on the Area Path.
//                                TestHelper.AddTests(subSuite, current_area);
//                            }
//
//                            suite = subSuite;
//                        }
//                        current_area += "\\";
//                        CurrentData.CurrentTestPlan.Save();
//                    }
//                }
                CurrentData.CurrentTestPlan.Save();
                    
            }
            catch (Exception eAddTestSuite) {
                Console.WriteLine(eAddTestSuite.Message);
            }
        }
        
        public static IStaticTestSuite GetTestSuite(TFSCmdletBase cmdlet, string name)
        {
            throw new NotImplementedException();
        }
        
        public static IStaticTestSuite OpenTesSuite(TFSCmdletBase cmdlet, string name)
        {
            throw new NotImplementedException();
        }
        
        public static void AddTestSubSuite(TFSCmdletBase cmdlet, string name)
        {
            throw new NotImplementedException();
        }
        
        public static void GetTestSubSuite(TFSCmdletBase cmdlet, string name)
        {
            throw new NotImplementedException();
        }
        
        public static void OpenTestSubSuite(TFSCmdletBase cmdlet, string name)
        {
            throw new NotImplementedException();
        }
        
        public static void AddTestCase(TFSCmdletBase cmdlet, string name)
        {
            throw new NotImplementedException();
        }
        
        public static void GetTestCase(TFSCmdletBase cmdlet, string name)
        {
            throw new NotImplementedException();
        }
        
        public static void OpenTestCase(TFSCmdletBase cmdlet, string name)
        {
            throw new NotImplementedException();
        }
        

    }
}
