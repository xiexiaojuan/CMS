﻿#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Text;
using Kooboo.Common.IO;

namespace Kooboo.CMS.Web2.HealthMonitoring
{
    public static class TextFileLogger
    {
        private static string WebEventsDir = "WebEvents";
        private static object lockerHelper = new object();
        public static void Log(string message)
        {
            lock (lockerHelper)
            {
                var fileName = GetLogFile();

                IOUtility.EnsureDirectoryExists(Path.GetDirectoryName(fileName));

                File.AppendAllText(fileName, message.Replace("\n", Environment.NewLine));
                File.AppendAllText(fileName, "--------------------------------------------------------------------------------------------------------\r\n");
            }
        }
        private static string GetLogFile()
        {
            var baseDir = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<Kooboo.CMS.Common.IBaseDir>();
            var filePath = GetLogFile(baseDir.Cms_DataPhysicalPath);
            return filePath;
            //if (Site.Current == null)
            //{
            //    var baseDir = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<Kooboo.CMS.Common.IBaseDir>();
            //    var filePath = GetLogFile(baseDir.Cms_DataPhysicalPath);
            //    return filePath;
            //}
            //else
            //{
            //    var filePath = GetLogFile(Site.Current.PhysicalPath);
            //    return filePath;
            //}
        }
        private static string GetLogFile(string baseDir)
        {
            var webEventsDir = Path.Combine(baseDir, WebEventsDir);
            var filePath = Path.Combine(webEventsDir, DateTime.UtcNow.ToString("yyyy-MM-dd") + ".log");
            return filePath;
        }
    }

#if MONO
	
#else
    public class TextFileWebEventProvider : WebEventProvider
    {
        // Methods
        public TextFileWebEventProvider()
        {
        }

        public override void Flush()
        {
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);
        }

        public override void ProcessEvent(WebBaseEvent eventRaised)
        {
            TextFileLogger.Log(eventRaised.ToString());
        }

        public override void Shutdown()
        {

        }

    }
#endif
}