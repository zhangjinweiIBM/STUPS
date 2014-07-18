﻿/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 7/14/2014
 * Time: 9:27 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace Tmx.Server.Commands
{
    using System;
    using System.Management.Automation;
	using TMX;
	using Tmx.Server.Helpers.Commands;
    
    /// <summary>
    /// Description of StopTmxServerCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Stop, "TmxServer")]
    public class StopTmxServerCommand : CommonCmdletBase
    {
        protected override void BeginProcessing()
        {
            var command = new StopServerCommand(this);
            command.Execute();
        }
    }
}
