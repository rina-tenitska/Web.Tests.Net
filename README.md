# NSelene + NUnit tests project template

## Overview and general guidelines

This is a template project. It's supposed to be cloned or downloaded and edited according to your needs.

The project itself reflects an implementation of acceptance web ui tests for a "web", i.e. as "application under test" we consider here the "whole web", under "root pages" we mean "web sites", under "sub-pages" we mean "web site pages". To apply this template to your context, usually you would need to rename all "Web" entries in names or some option values in config files (like TBD) to "Your.ProjectName" with the following exceptions:

- you can rename `Www.cs` to `App.cs` instead of `YourProjectName.cs` for conciseness

Hence, download it, rename the project folder to something like ``MyProduct.Tests``, then rename the `.csproj` and other `.cs` files and namespaces correspondingly...

And you should be ready to go ;)

## Installation

TBD

## Details

Features supported:
- so far, see [CHANGELOG](https://github.com/yashaka/Web.Tests.Net/blob/master/CHANGELOG.md) for more details
- TBD

## Run Tests

### With local Chrome

To run tests on local machine WebDriver:Remote setting needs to be set to false:

```bash
env -S "WebDriver:Remote=false" dotnet test
```

or in PowerShell:

```ps
dotnet test -e WebDriver:Remote=false
```

Browser can be set by following:

```bash
env -S "WebDriver:browserName=chrome" dotnet test
```

or in PowerShell:

```ps
dotnet test -e WebDriver:browserName=chrome
```

#### overriding more than one param from appsettings.json

```bash
env -S "WebDriver:Local=chrome NSelene:Timeout=8" dotnet test
```

or in PowerShell

```ps
dotnet test -e WebDriver:browserName=chrome -e NSelene:Timeout=8
```

### Local Selenoid on Docker

#### Given

Check browser images in etc/selenoid/browsers.json and once performed `docker pull` for all corresponding images, like: 

```bash
docker pull selenoid/chrome:84.0 && docker pull selenoid/vnc_chrome:84.0
```

do either for "pure selenoid"

```bash
docker-compose -f etc/selenoid/compose.yaml up -d selenoid
```

or for "selenoid with selenoid UI"

```bash
docker-compose -f etc/selenoid/compose.yaml up -d
```

#### Then

```bash
env -S "WebDriver:Remote=true" dotnet test
```

or in PowerShell:

```ps
dotnet test --environment WebDriver:Remote=true
```

##### overriding some settings from appsettings.json

```bash
env -S "WebDriver:enableVNC=false NSelene:Timeout=8" dotnet test
```

#### Finally

```bash
docker-compose -f etc/selenoid/compose.yaml stop
```


### Managing different sets of settings

Settings for different environments and contexts (like running tests on local machine or remote server) can be specified in additional appsettings.*.json files, and by specifying context these settings will override those set in appsettings.json file. By default context is set for "local", switching between those files can be done by specifying context for test run:

```bash
env -S "context=prod" dotnet test
```

or in PowerShell:

```ps
dotnet test --environment context=prod
```

### Selecting tests for run

You can differentiate tests for separate test-runs by using `Category` attribute:

```cs
[Test]
[Category("MyCategory")]
```

```bash
dotnet test --filter|-f TestCategory="MyCategory"
```

### Running tests in parallel

Tests are set to run in parallel by using `Parallelizable` attribute:

```cs
[Parallelizable(ParallelScope.All)]
public class BrowserTest
{
    // ...
}
```

Maximum amount of tests running simultaneously can be set by following:

```bash
dotnet test <all other options like --filter or --environment> -- NUnit.NumberOfTestWorkers=<number>
```

Make sure to pass all dotnet test properties before using -- NUnit properties, otherwise they might be forfeight

### Report test results

#### Testomat.io

Arter creating project pn testomat app, as described [here](https://docs.testomat.io/getting-started/start-from-scratch/), you will need to setup project locally. For this you need to install testomat reporter:

```bash
npm i @testomatio/reporter
```

To create reports, which would be parsed by testomat.io reporter, add NunitXml.TestLogger package to the project and run tests with specified logger:

```bash
dotnet add package NunitXml.TestLogger --version 4.1.0
dotnet test --logger|-l:nunit
```

Then, add API key to system variables and import testresults to Testomat.io:

```bash
TESTOMATIO=key npx report-xml "/path-to-xml-reports/*.xml" --lang="c# 
```

or in PowerShell:

```ps
[Environment]::SetEnvironmentVariable('TESTOMATIO', 'key'); npx report-xml "/path-to-xml-reports/*.xml"  --lang="c#
// or
setx TESTOMATIO key /m; npx report-xml "/path-to-xml-reports/*.xml" --lang="c#
// or
$env:TESTOMATIO = "key"; npx report-xml "/path-to-xml-reports/*.xml" --lang="c#
```

You can use commands for test run with logging and importing test results sequentially:

```bash
dotnet test --logger|-l:nunit TESTOMATIO=key npx report-xml "/path-to-xml-reports/*.xml" --lang="c#
```

or in PowerShell:

```ps
dotnet test -l:nunit; $env:TESTOMATIO = "tstmt_ZkBAvUO-V5ze-kX5gWitypM0qVqEgrcfDA1733908621"; npx report-xml "/path-to-xml-reports/**.xml" --lang="c#"
```

You can also set any environment variables for test run:

```ps
dotnet test -l:nunit -e context=prod; $env:TESTOMATIO = "tstmt_ZkBAvUO-V5ze-kX5gWitypM0qVqEgrcfDA1733908621"; npx report-xml "/path-to-xml-reports/**.xml" --lang="c#"
```


## Resources

* [Accessing Configuration in .NET Core Test Projects. By Rick Strahl. From February 18, 2018](https://weblog.west-wind.com/posts/2018/Feb/18/Accessing-Configuration-in-NET-Core-Test-Projects)