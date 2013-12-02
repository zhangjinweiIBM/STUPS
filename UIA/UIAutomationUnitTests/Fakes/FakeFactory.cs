﻿/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 11/20/2013
 * Time: 12:39 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace UIAutomationUnitTests
{
    using System;
    using System.Windows.Automation;
    using UIAutomation;
    using NSubstitute;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections;
    using System.Collections.Generic;
    
    /// <summary>
    /// Description of FakeFactory.
    /// </summary>
    public class FakeFactory
    {
        public static void Init()
        {
            UnitTestingHelper.PrepareUnitTestDataStore();
            ObjectsFactory.InitUnitTests();
        }
        
        private static IMySuperValuePattern GetValuePattern(string txtValue)
        {
            IMySuperValuePattern valuePattern = Substitute.For<IMySuperValuePattern>();
            IValuePatternInformation valuePatternInformation = Substitute.For<IValuePatternInformation>();
            valuePatternInformation.Value.Returns(txtValue);
            valuePattern.Current.Returns(valuePatternInformation);
            return valuePattern;
        }
        
        public static  IMySuperWrapper GetAutomationElementExpected(ControlType controlType, string name, string automationId, string className, string txtValue)
        {
            return GetAutomationElement(controlType, name, automationId, className, txtValue, true);
        }
        
        public static  IMySuperWrapper GetAutomationElementNotExpected(ControlType controlType, string name, string automationId, string className, string txtValue)
        {
            return GetAutomationElement(controlType, name, automationId, className, txtValue, false);
        }
        
        private static IMySuperWrapper GetAutomationElement(ControlType controlType, string name, string automationId, string className, string txtValue, bool expected)
        {
            IMySuperWrapper element = Substitute.For<IMySuperWrapper>();
            element.Current.ProcessId.Returns(333);
            element.Current.ControlType.Returns(controlType);
            element.Current.Name.Returns(!string.IsNullOrEmpty(name) ? name : string.Empty);
            element.Current.AutomationId.Returns(!string.IsNullOrEmpty(automationId) ? automationId : string.Empty);
            element.Current.ClassName.Returns(!string.IsNullOrEmpty(className) ? className : string.Empty);
            IMySuperValuePattern valuePattern = FakeFactory.GetValuePattern(txtValue);
            element.GetSupportedPatterns().Returns(new AutomationPattern[] { ValuePattern.Pattern });
            element.GetCurrentPattern(ValuePattern.Pattern).Returns(valuePattern);
            object patternObject;
            element.TryGetCurrentPattern(ValuePattern.Pattern, out patternObject).Returns(true);
            if (expected) { element.Tag.Returns("expected"); }
            return element;
        }
        
        public static GetControlCmdletBase Get_GetControlCmdletBase(ControlType controlType, string name, string automationId, string className, string txtValue)
        {
            GetControlCmdletBase cmdlet = Substitute.For<GetControlCmdletBase>();
            if (null != controlType) {
                cmdlet.ControlType.Returns(
                    new[] {
                        controlType.ProgrammaticName.Substring(12)
                    }
                   );
            }
            cmdlet.Name.Returns(!string.IsNullOrEmpty(name) ? name : string.Empty);
            cmdlet.AutomationId.Returns(!string.IsNullOrEmpty(automationId) ? automationId : string.Empty);
            cmdlet.Class.Returns(!string.IsNullOrEmpty(className) ? className : string.Empty);
            cmdlet.Value.Returns(!string.IsNullOrEmpty(txtValue) ? txtValue : string.Empty);
            return cmdlet;
        }
        
        public static GetControlCmdletBase Get_GetControlCmdletBase(ControlType controlType, string searchString)
        {
            GetControlCmdletBase cmdlet = Substitute.For<GetControlCmdletBase>();
            if (null != controlType) {
                cmdlet.ControlType.Returns(
                    new[] {
                        controlType.ProgrammaticName.Substring(12)
                    }
                   );
            }
            cmdlet.ContainsText.Returns(!string.IsNullOrEmpty(searchString) ? searchString : string.Empty);
            return cmdlet;
        }
        
        public static IMySuperWrapper GetElement_ForFindAll(IEnumerable<IMySuperWrapper> elements, Condition conditions)
        {
            IMySuperWrapper element =
                GetAutomationElement(ControlType.Pane, string.Empty, string.Empty, string.Empty, string.Empty, false);
            IMySuperCollection descendants = ObjectsFactory.GetMySuperCollection(elements);
            
            Condition[] condCollection = null;
            if (null != conditions as AndCondition) {
                condCollection = (conditions as AndCondition).GetConditions();
            }
            
            if (null != conditions as OrCondition) {
                condCollection = (conditions as OrCondition).GetConditions();
            }
            
            IMySuperCollection descendants2 = ObjectsFactory.GetMySuperCollection();
            foreach (IMySuperWrapper elt in descendants
                .Cast<IMySuperWrapper>()
                .Where(elt => "expected" == elt.Tag))
            {
                descendants2.SourceCollection.Add(elt);
            }
            
            element.FindAll(TreeScope.Descendants, Arg.Any<Condition>()).Returns(descendants2);
            return element;
        }
    }
}
