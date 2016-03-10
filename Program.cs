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

using System;
using System.IO;
using System.Linq;
using System.Text;
using FileMode = System.IO.FileMode;

using CsvHelper;
using CsvHelper.Configuration;
using Octokit;

using ImportOrExportGitIssues.Properties;

namespace ImportOrExportGitIssues
{
    /// <summary>
    /// The main program.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var gitApiRootPath = Settings.Default.GitApiRootPath;
            var filePath = Settings.Default.FilePath;
            var gitLogin = Settings.Default.GitLogin;
            var gitPassword = Settings.Default.GitPassword;
            var repositoryOwner = Settings.Default.RepositoryOwner;
            var repositoryName = Settings.Default.repositoryName;
            var issueLabels = Settings.Default.IssueLabels;
            var operation = Settings.Default.Operation;

            var client = new GitHubClient(new ProductHeaderValue("bugs"), new Uri(gitApiRootPath))
            {
                Credentials = new Credentials(gitLogin, gitPassword)
            };
            var repositoryIssueRequest = new RepositoryIssueRequest
            {
                Labels = { issueLabels },
                State = ItemState.All,
                SortProperty = IssueSort.Created,
                SortDirection = SortDirection.Descending
            };

            if (!string.IsNullOrWhiteSpace(operation) && operation.ToLower() == "import")
            {
                ImportFromCsv(client, repositoryOwner, repositoryName, filePath);
            }
            else
            {
                ExportGitIssues(client, repositoryOwner, repositoryName, repositoryIssueRequest, filePath);
            }

            //var option = ArgsParser.Parse(args);

            //if (option.ContainsKey("o"))
            //{
            //    var value = option["o"];
            //    if (value.ToLower() == "i")
            //    {
            //        ImportFromCsv(client, repositoryOwner, repositoryName, filePath);
            //    }
            //    else
            //    {
            //        ExportGitIssues(client, repositoryOwner, repositoryName, repositoryIssueRequest, filePath);
            //    }
            //}
        }

        private static void ExportGitIssues(GitHubClient client, string repositoryOwner, string repositoryName,
            RepositoryIssueRequest repositoryIssueRequest, string filePath)
        {
            try
            {
                var issues = client.Issue.GetAllForRepository(repositoryOwner,
                    repositoryName, repositoryIssueRequest).Result;

                using (var streamWriter = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    streamWriter.AutoFlush = true;

                    // write header
                    var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration { Encoding = Encoding.UTF8 });
                    csvWriter.WriteField("Number");
                    csvWriter.WriteField("Title");
                    csvWriter.WriteField("Description");
                    csvWriter.WriteField("Labels");
                    csvWriter.NextRecord();

                    // write content
                    foreach (var issue in issues)
                    {
                        csvWriter.WriteField(issue.Number);
                        csvWriter.WriteField(issue.Title);
                        csvWriter.WriteField(issue.Body.Length > 4096 ? issue.Body.Substring(0, 4096) : issue.Body);

                        var labels = string.Empty;
                        if (issue.Labels.Any())
                        {
                            issue.Labels.ToList()
                                .ForEach(x => labels = !string.IsNullOrEmpty(labels)
                                    ? string.Format("{0} | {1}", labels, x.Name)
                                    : x.Name);
                        }
                        csvWriter.WriteField(labels);
                        csvWriter.NextRecord();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void ImportFromCsv(GitHubClient client, string repositoryOwner, string repositoryName, string filePath)
        {
            try
            {
                using (var streamReader = new StreamReader(new FileStream(filePath, FileMode.Open)))
                {
                    var csvReader = new CsvReader(streamReader);
                    while (csvReader.Read())
                    {
                        var newIssue = new NewIssue(csvReader.GetField("Title"))
                        {
                            Body = csvReader.GetField("Description")
                        };
                        var issue = client.Issue.Create(repositoryOwner, repositoryName, newIssue).Result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
