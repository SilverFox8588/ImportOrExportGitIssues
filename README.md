# ImportOrExportGitIssues
Import or export git issues by C#.

## Overview

You can import or export git issues by C# console program. Only one thing you should do is setting the App.config file like below:

<applicationSettings>
        <ImportOrExportGitIssues.Properties.Settings>
            <setting name="FilePath" serializeAs="String">
                <value>d:\temp\gitissues.csv</value>
            </setting>
            <setting name="GitLogin" serializeAs="String">
                <value>Your git login account</value>
            </setting>
            <setting name="GitPassword" serializeAs="String">
                <value>Your git login password</value>
            </setting>
            <setting name="IssueLabels" serializeAs="String">
                <value>bug</value>
            </setting>
            <setting name="GitApiRootPath" serializeAs="String">
                <value>https://api.github.com</value>
            </setting>
            <setting name="RepositoryOwner" serializeAs="String">
                <value>SilverFox8588</value>
            </setting>
            <setting name="repositoryName" serializeAs="String">
                <value>Demo</value>
            </setting>
            <setting name="Operation" serializeAs="String">
                <value>import</value>
            </setting>
        </ImportOrExportGitIssues.Properties.Settings>
    </applicationSettings>
    
    In addition, your .csv file should look like the "gitissues.csv".
