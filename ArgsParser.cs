//------------------------------------------------------------------------------
// ArgsParser.cs
//
// <copyright from='2005' to='2012' company='Smartware Enterprises Inc'> 
// Copyright (c) Smartware Enterprises Inc. All Rights Reserved. 
// Information Contained Herein is Proprietary and Confidential. 
// </copyright>
//
// Created: 03/10/2016
// Owner: HongYin Wang
//
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace ImportOrExportGitIssues
{
    /// <summary>
    /// Args parser.
    /// </summary>
    public class ArgsParser
    {
        /// <summary>
        /// Parse method.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IDictionary<string, string> Parse(params string[] args)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-"))
                {
                    string value = (i + 1) > args.Length ? string.Empty : args[i + 1];
                    result.Add(args[i].Substring(1, args[i].Length - 1), value);
                    i++;
                }
            }
            return result;
        }
    }
}
