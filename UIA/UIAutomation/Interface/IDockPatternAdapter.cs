﻿/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 12/8/2013
 * Time: 10:27 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace UIAutomation
{
	extern alias UIANET;
	using System.Windows.Automation;

	public interface IMySuperDockPattern : IBasePattern
	{
		void SetDockPosition(DockPosition dockPosition);
		// DockPattern.DockPatternInformation Cached { get; }
		IDockPatternInformation Cached { get; }
		// DockPattern.DockPatternInformation Current { get; }
		IDockPatternInformation Current { get; }
	}
}

